using System;
using Android.Content;
using Android.Preferences;

namespace debtsTracker.Managers
{
    public class DroidLocalStorage
    {
        private readonly ISharedPreferences mSharedPrefs;
        private readonly ISharedPreferencesEditor mPrefsEditor;
        private readonly Context context;

        public DroidLocalStorage()
        {
			context = Android.App.Application.Context;
			mSharedPrefs = PreferenceManager.GetDefaultSharedPreferences(context);
			mPrefsEditor = mSharedPrefs.Edit();
        }

		public string GetStringValue(string key)
		{
			return mSharedPrefs.GetString(key, string.Empty);
		}

		public int GetIntValue(string key)
		{
			return mSharedPrefs.GetInt(key, 0);
		}

		public bool GetBoolValue(string key)
		{
			return mSharedPrefs.GetBoolean(key, false);
		}

		public void SetStringValue(string key, string value)
		{
			mPrefsEditor.PutString(key, value);
			mPrefsEditor.Commit();
		}

		public void SetIntValue(string key, int value)
		{
			mPrefsEditor.PutInt(key, value);
			mPrefsEditor.Commit();
		}

		public void SetBoolValue(string key, bool value)
		{
			mPrefsEditor.PutBoolean(key, value);
			mPrefsEditor.Commit();
		}
    }
}
