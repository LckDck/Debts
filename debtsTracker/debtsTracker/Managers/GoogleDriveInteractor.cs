using System;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Drive;
using Android.Gms.Drive.Query;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Java.IO;
using Plugin.CurrentActivity;

namespace debtsTracker.Managers
{
    public class GoogleDriveInteractor : Java.Lang.Object, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, IResultCallback
    {
        DriveId _driveId;
        private GoogleApiClient _googleApiClient;
        private String FolderName;

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

        public void SaveFile()
        {
            UploadToDrive();
        }


        public void OnResult(Java.Lang.Object result)
        {
            System.Diagnostics.Debug.WriteLine("asdf");
            try
            {
                var res = result.JavaCast<IDriveApiMetadataBufferResult>();
                if (res != null)
                {
                    if (!res.Status.IsSuccess)
                    {
                        System.Diagnostics.Debug.WriteLine("Cannot create folder in the root.");
                    }
                    else
                    {
                        bool isFound = false;
                        foreach (Metadata m in res.MetadataBuffer)
                        {
                            if (m.Title.Equals(FolderName))
                            {
                                System.Diagnostics.Debug.WriteLine("Folder exists");
                                isFound = true;
                                var driveId = m.DriveId;
                                CreateFileInFolder(driveId);
                                break;
                            }
                        }

                        if (!isFound)
                        {
                            System.Diagnostics.Debug.WriteLine("Folder not found; creating it.");
                            MetadataChangeSet changeSet = new MetadataChangeSet.Builder().SetTitle(FolderName).Build();
                            DriveClass.DriveApi.GetRootFolder(_googleApiClient)
                                      .CreateFolder(_googleApiClient, changeSet)
                                      .SetResultCallback(this);
                        }
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("miscast");
            }

            try
            {
                var res1 = result.JavaCast<IDriveFolderDriveFolderResult>();
                if (res1 != null)
                {
                    if (!res1.Status.IsSuccess)
                    {
                        System.Diagnostics.Debug.WriteLine("U AR A MORON! Error while trying to create the folder");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Created a folder");
                        DriveId driveId = res1.DriveFolder.DriveId;
                        CreateFileInFolder(driveId);
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("miscast");
            }

            try
            {
                var res2 = result.JavaCast<IDriveApiDriveContentsResult>();
                if (res2 != null)
                {

                    if (!res2.Status.IsSuccess)
                    {
                        System.Diagnostics.Debug.WriteLine("U AR A MORON! Error while trying to create new file contents");
                        return;
                    }

                    var outputStream = res2.DriveContents.OutputStream;

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
                    Toast.MakeText(MainActivity.Current, "Uploading to drive. If you didn't fucked up something like usual you should see it there", ToastLength.Long).Show();
                    var theFile = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/xtests/tehfile.txt"); //>>>>>> WHAT FILE ?
                    try
                    {
                        var fileInputStream = new FileInputStream(theFile);
                        byte[] buffer = new byte[1024];
                        int bytesRead;
                        while ((bytesRead = fileInputStream.Read(buffer)) != -1)
                        {
                            outputStream.Write(buffer, 0, bytesRead);
                        }
                    }
                    catch (Java.IO.IOException e1)
                    {
                        System.Diagnostics.Debug.WriteLine("U AR A MORON! Unable to write file contents.");
                    }

                    MetadataChangeSet changeSet = new MetadataChangeSet.Builder().SetTitle(theFile.Name).SetMimeType("text/plain").SetStarred(false).Build();
                    var folder = _driveId.AsDriveFolder();
                    folder.CreateFile(_googleApiClient, changeSet, res2.DriveContents)
                          .SetResultCallback(this);

                    return;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("miscast");
            }

            try
            {
                var res3 = result.JavaCast<IDriveFolderDriveFileResult>();
                if (res3 != null)
                {
                    if (!res3.Status.IsSuccess)
                    {
                        System.Diagnostics.Debug.WriteLine("U AR A MORON!  Error while trying to create the file");
                        return;
                    }
                    System.Diagnostics.Debug.WriteLine("Created a file: " + res3.DriveFile.DriveId);
                    return;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("miscast");
            }
        }




        private void UploadToDrive()
        {
            //async check if folder exists... if not, create it. continue after with create_file_in_folder(driveId);
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
            System.Diagnostics.Debug.WriteLine("Google API connection failed!");
            if (result.HasResolution)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Google API start asking");
                    result.StartResolutionForResult(MainActivity.Current, 2);

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
            System.Diagnostics.Debug.WriteLine("Google API connected!");
            SaveFile();

        }

        public void OnConnectionSuspended(int cause)
        {
            System.Diagnostics.Debug.WriteLine("Google API connection suspended");
        }


    }
}
