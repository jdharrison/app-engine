using Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProfileEditor : EditorWindow
{
    [MenuItem("AppEngine/Profiles")]
    public static void ConfigureProfiles()
    {
        ConfigureProfilesEditor editor = EditorWindow.GetWindow<ConfigureProfilesEditor>();
        editor.title = "Profiles";
        editor.Show();
    }
}

public class ConfigureProfilesEditor : EditorWindow
{
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Clear Profiles"))
        {
            if(EditorUtility.DisplayDialog("Clear Profiles Settings", "Do you want to clear all profile settings?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Current Profile:");
        eProfile currentProfile = (eProfile)EditorGUILayout.EnumPopup(Profile.current);
        if(Profile.current != currentProfile)
        {
            currentProfile.Select();
        }
        EditorGUILayout.EndHorizontal();

        List<string> keys = currentProfile.GetKeys();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Settings:");
        if(keys.Count > 0 && GUILayout.Button("Reset"))
        {
            if(EditorUtility.DisplayDialog("Clear Settings", "Do you want to clear profile settings for: " + currentProfile + "?", "Yes", "No"))
            {
                currentProfile.ClearProfile();
            }
        }
        EditorGUILayout.EndHorizontal();

        if(keys.Count == 0)
        {
            EditorGUILayout.LabelField("- No settings saved.");
        } else
        {

            currentProfile.GetKeys().ForEach(delegate(string key)
            {
                EditorGUILayout.LabelField(" - " + key + " = " + currentProfile.GetValue(key));
            });
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }
}