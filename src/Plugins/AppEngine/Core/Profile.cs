using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Core
{
    public enum eProfile
    {
        Development,
        Production
    }

    public static class Profile
    {
        public const string SELECTED_PROFILE = "profile.selected";

        public static void ClearAllProfiles()
        {
            foreach (eProfile profile in eProfile.GetValues(typeof(eProfile)))
            {
                profile.ClearProfile();
            }
        }

        public static eProfile current
        {
            get
            {
                if(PlayerPrefs.HasKey(SELECTED_PROFILE))
                {
                    string selection = PlayerPrefs.GetString(SELECTED_PROFILE);
                    return (eProfile)Enum.Parse(typeof(eProfile), selection, true);
                }
                return eProfile.Development;
            }
        }

        public static void Select(eProfile profile)
        {
            Utils.SetPlayerPref<string>(SELECTED_PROFILE, profile.ToString());
        }
    }

    public static class eProfileExtensions
    {
        private const string PROFILE_PROPERTY = "profile.{0}.{1}";
        private const string PROFILE_KEYS = "profile.{0}.keys";

        private const char PROFILE_KEY_DELIMITER = '|';

        public static bool IsSelected(this eProfile profile)
        {
            return Profile.current == profile;
        }

        public static void Select(this eProfile profile)
        {
            Profile.Select(profile);
        }

        public static string ProfileProperty(this eProfile profile, string name)
        {
            return string.Format(PROFILE_PROPERTY, profile.ToString(), name).ToLower();
        }

        public static void ClearProfile(this eProfile profile)
        {
            foreach (string key in profile.GetKeys())
            {
                if(PlayerPrefs.HasKey(key))
                {
                    PlayerPrefs.DeleteKey(key);
                }
            }

            if(PlayerPrefs.HasKey(profile.ProfileProperty(PROFILE_KEYS)))
            {
                PlayerPrefs.DeleteKey(profile.ProfileProperty(PROFILE_KEYS));
            }
        }

        public static List<string> GetKeys(this eProfile profile)
        {
            List<string> result = new List<string>();
            if(profile.HasValue(PROFILE_KEYS))
            {
                string allKeys = profile.GetValue<string>(PROFILE_KEYS);
                foreach (string key in allKeys.Split(PROFILE_KEY_DELIMITER))
                {
                    if(!string.IsNullOrEmpty(key))
                    {
                        result.Add(key);
                    }
                }
            }
            return result;
        }

        public static void SetValue<T>(this eProfile profile, string name, object value)
        {
            List<string> keys = profile.GetKeys();
            if(!keys.Contains(name))
            {
                string allKeys = !profile.HasValue(PROFILE_KEYS) ? string.Empty : profile.GetValue<string>(PROFILE_KEYS);
                allKeys += PROFILE_KEY_DELIMITER + name;
                Utils.SetPlayerPref<string>(profile.ProfileProperty(PROFILE_KEYS), allKeys);
            }

            Utils.SetPlayerPref<T>(profile.ProfileProperty(name), value);
        }

        public static object GetValue(this eProfile profile, string name)
        {
            return Utils.GetPlayerPref(profile.ProfileProperty(name));
        }

        public static T GetValue<T>(this eProfile profile, string name)
        {
            return Utils.GetPlayerPref<T>(profile.ProfileProperty(name));
        }

        public static bool HasValue(this eProfile profile, string name)
        {
            string property = profile.ProfileProperty(name);
            return PlayerPrefs.HasKey(property);
        }
    }
}