using System;
using System.IO;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Drive;
using Android.Gms.Drive.Query;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using debtsTracker.Utilities;
using Java.IO;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Debug = System.Diagnostics.Debug;

namespace debtsTracker.Managers
{
    public class GoogleDriveInteractor : Java.Lang.Object, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, IResultCallback
    {
        DriveId _driveId;
        private GoogleApiClient _googleApiClient;
        private String FolderName;

        public const int RESULT_CODE = 2113;

        public void Init(string folderName)
        {
            FolderName = folderName;

            _googleApiClient = new GoogleApiClient.Builder(MainActivity.Current)
            .AddApi(DriveClass.API)
            .AddScope(DriveClass.ScopeFile)
            .AddScope(DriveClass.ScopeAppfolder)
            .AddConnectionCallbacks(this)
            .AddOnConnectionFailedListener(this)
            .Build();
        }
        public void Connect()
        {
            if (!_googleApiClient.IsConnected)
            {
                _googleApiClient.Connect();
            }
        }

        private DriveAction _driveAction;

        public void GoogleDriveAction() {
            var connected = _googleApiClient.IsConnected;

            if (!connected)
            {
                _googleApiClient.Connect();
            }
            else
            {
                switch (_driveAction)
                {
                    case DriveAction.Read:
                        ReadFileFromDrive();
                        break;

                    case DriveAction.Write:
                        UploadToDrive();
                        break;
                }
            }
        }


        public void SaveFile()
        {
            _driveAction = DriveAction.Write;
            if (_googleApiClient.IsConnected) { 
                GoogleDriveAction();
                return;
            }
            Connect();
            
        }

        public void ReadFile() {
			_driveAction = DriveAction.Read;
			if (_googleApiClient.IsConnected)
			{
				GoogleDriveAction();
				return;
			}
            Connect();
        }

        private void ReadFileFromDrive()
        {
			var query = new QueryClass.Builder()
                                      .AddFilter(Filters.Contains(SearchableField.Title, Constants.BackupFileName)).Build();
            DriveClass.DriveApi.Query(_googleApiClient, query)
					.SetResultCallback(this);
        }

        void CheckIDriveApiMetadataBufferResult(Java.Lang.Object result) {
            try
            {
                var res = result.JavaCast<IDriveApiMetadataBufferResult>();
                if (res != null)
                {
                    if (!res.Status.IsSuccess)
                    {
                        Debug.WriteLine("Cannot create folder in the root.");
                    }
                    else
                    {
                        bool isFound = false;
                        bool readFileExists = false;
                        foreach (var m in res.MetadataBuffer)
                        {
                            if (m.Title.Equals(FolderName) && _driveAction == DriveAction.Write)
                            {
                                Debug.WriteLine("Folder exists");

                                var driveId = m.DriveId;

                                if (res.MetadataBuffer.Count > 0)
                                {
                                    var folder = driveId.AsDriveFolder();
                                    folder.Delete(_googleApiClient);
                                }
                                else
                                {
                                    isFound = true;
                                    CreateFileInFolder(driveId);
                                }
                                break;
                            }

                            if (m.Title.Equals(Constants.BackupFileName) && !m.IsTrashed)
                            {
                                Debug.WriteLine("file exists");
                                readFileExists = true;
                                var file = m.DriveId.AsDriveFile();
                                file.Open(_googleApiClient, DriveFile.ModeReadOnly, null).SetResultCallback(this);

							}
                        }

                        if (_driveAction == DriveAction.Read && !readFileExists)
						{
                            Utils.ShowNothigToRead(Relogin);
							return;
						}


                        if (_driveAction == DriveAction.Write && !isFound)
                        {
                            Debug.WriteLine("Folder not found; creating it.");
                            MetadataChangeSet changeSet = new MetadataChangeSet.Builder().SetTitle(FolderName).Build();
                            DriveClass.DriveApi.GetRootFolder(_googleApiClient)
                                      .CreateFolder(_googleApiClient, changeSet)
                                      .SetResultCallback(this);
                            
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("miscast 2", e);
            }
        }


        void CheckIDriveFolderDriveFolderResult(Java.Lang.Object result) {
			try
			{
                var res = result.JavaCast<IDriveFolderDriveFolderResult>();
				if (res != null)
				{
					if (!res.Status.IsSuccess)
					{
						Debug.WriteLine("U AR A MORON! Error while trying to create the folder");
					}
					else
					{
						Debug.WriteLine("Created a folder");
						DriveId driveId = res.DriveFolder.DriveId;
						CreateFileInFolder(driveId);
					}
				}
			}
            catch (Exception e)
			{
				Debug.WriteLine("miscast 3", e);
			}

		}


        void CheckIDriveApiDriveContentsResult(Java.Lang.Object result) {
			try
			{
				var res = result.JavaCast<IDriveApiDriveContentsResult>();
				if (res != null)
				{
                    Stream stream = null;
					if (!res.Status.IsSuccess)
					{
						Debug.WriteLine("U AR A MORON! Error while trying to create new file contents");
						return;
					}

                    try
                    {
                        stream = res.DriveContents.OutputStream;
                        WriteDriveFile(stream, res);
                        _driveAction = DriveAction.None;
                    }
                    catch (Exception e)
                    {
                        stream = res.DriveContents.InputStream;
                        ReadDriveFile(stream);
                        _driveAction = DriveAction.None;
                    }
				}
			}
            catch (Exception e)
			{
				Debug.WriteLine("miscast", e);
			}
        }

		DebtsManager _debtsManager;
		protected DebtsManager DebtsManager
		{
			get
			{
				return _debtsManager ?? (_debtsManager = ServiceLocator.Current.GetInstance<DebtsManager>());
			}
		}

		InterfaceUpdateManager _interfaceUpdateManager;
		protected InterfaceUpdateManager InterfaceUpdateManager
		{
			get
			{
				return _interfaceUpdateManager ?? (_interfaceUpdateManager = ServiceLocator.Current.GetInstance<InterfaceUpdateManager>());
			}
		}

		IExtendedNavigationService _navigationService;
		protected IExtendedNavigationService NavigationService
		{
			get
			{
				return _navigationService ?? (_navigationService = ServiceLocator.Current.GetInstance<IExtendedNavigationService>());
			}
		}

        public void ReadDriveFile(Stream stream)
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                var json = streamReader.ReadToEnd();

                Debug.WriteLine(json);
                DebtsManager.RestoreDebts(json);

                InterfaceUpdateManager.InvokeUpdateMainScreen();
                NavigationService.PopToRoot();
                var message = Utils.GetStringFromResource(Resource.String.read);
				Utils.ShowToast(message);
            }
        }

        private void WriteDriveFile(Stream stream, IDriveApiDriveContentsResult result)
        {
			//------ THIS IS AN EXAMPLE FOR PICTURE ------
			//ByteArrayOutputStream bitmapStream = new ByteArrayOutputStream();
			//image.compress(Bitmap.CompressFormat.PNG, 100, bitmapStream);
			//try {
			//  outputStream.write(bitmapStream.toByteArray());
			//} catch (IOException e1) {
			//  Log.i(TAG, "Unable to write file contents.");
			//}
			//// Create the initial metadata - MIME type and title.
			//// Note that the user will be able to change the title later.
			//MetadataChangeSet metadataChangeSet = new MetadataChangeSet.Builder()
			//    .setMimeType("image/jpeg").setTitle("Android Photo.png").build();

			//------ THIS IS AN EXAMPLE FOR FILE --------
			var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			var filename = Path.Combine(path, Constants.BackupFileName);

			using (var streamWriter = new StreamWriter(filename, true))
			{
                var json = JsonConvert.SerializeObject(DebtsManager.CurrentDebts);
				streamWriter.WriteLine(json);
			}

			var message = String.Format(Utils.GetStringFromResource(Resource.String.upload), Utils.GetStringFromResource(Resource.String.app_name));
            Utils.ShowToast(message);
			var theFile = new Java.IO.File(filename); //>>>>>> WHAT FILE ?
			try
			{
				var fileInputStream = new FileInputStream(theFile);
				byte[] buffer = new byte[1024];
				int bytesRead;
				while ((bytesRead = fileInputStream.Read(buffer)) != -1)
				{
					stream.Write(buffer, 0, bytesRead);
				}
			}
			catch (Java.IO.IOException e1)
			{
				Debug.WriteLine("U AR A MORON! Unable to write file contents.", e1);
			}

			MetadataChangeSet changeSet = new MetadataChangeSet.Builder().SetTitle(theFile.Name).SetMimeType("text/plain").SetStarred(false).Build();
			var folder = _driveId.AsDriveFolder();
			folder.CreateFile(_googleApiClient, changeSet, result.DriveContents)
				  .SetResultCallback(this);

			ClearDirectory(path);


		}

        void CheckIDriveFolderDriveFileResult(Java.Lang.Object result) {
			try
			{
                var res = result.JavaCast<IDriveFolderDriveFileResult>();
				if (res != null)
				{
					if (!res.Status.IsSuccess)
					{
						Debug.WriteLine("U AR A MORON!  Error while trying to create the file");
						return;
					}
					Debug.WriteLine("Created a file: " + res.DriveFile.DriveId);
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("miscast 1", e);
			}
        }


		public void OnResult(Java.Lang.Object result)
        {
            Debug.WriteLine("OnResult!");

            CheckIDriveApiMetadataBufferResult(result);
            CheckIDriveFolderDriveFolderResult(result);
            CheckIDriveApiDriveContentsResult(result);
            CheckIDriveFolderDriveFileResult(result);

        }


        private void ClearDirectory(string path)
        {
			foreach (var file in Directory.GetFiles(path))
			{
				System.IO.File.Delete(file);
			}
        }

        private void UploadToDrive()
        {
            CheckFolderExists();
        }



        private void CheckFolderExists()
        {
            var query =
                new QueryClass.Builder().AddFilter(Filters.And(Filters.Eq(SearchableField.Title, FolderName), Filters.Eq(SearchableField.Trashed, false)))
                    .Build();
            DriveClass.DriveApi.Query(_googleApiClient, query).SetResultCallback(this);
        }


        private void CreateFileInFolder(DriveId driveId)
        {
            _driveId = driveId;
            DriveClass.DriveApi.NewDriveContents(_googleApiClient).SetResultCallback(this);

        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            Debug.WriteLine("Google API connection failed!");
            if (result.HasResolution)
            {
                try
                {
                    Debug.WriteLine("Google API start asking");
                    result.StartResolutionForResult(MainActivity.Current, RESULT_CODE);

                }
                catch (IntentSender.SendIntentException e)
                {
                    // Unable to resolve, message user appropriately
                }
            }
            else
            {
                GooglePlayServicesUtil.ShowErrorDialogFragment(result.ErrorCode, MainActivity.Current, 0);
            }
        }

        public void OnConnected(Bundle connectionHint)
        {
            Debug.WriteLine("Google API connected!");
            GoogleDriveAction();

        }

        public void OnConnectionSuspended(int cause)
        {
            Debug.WriteLine("Google API connection suspended");
        }

        void Relogin() {
            if (_googleApiClient.HasConnectedApi(DriveClass.API)) { 
                _googleApiClient.ClearDefaultAccountAndReconnect();
            }
		}

    }
}
