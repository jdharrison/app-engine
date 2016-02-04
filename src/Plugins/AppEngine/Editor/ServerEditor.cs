using Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ServerEditor
{
    [MenuItem("AppEngine/Server")]
    public static void ConfigureServer()
    {
        ConfigureServerEditor editor = EditorWindow.GetWindow<ConfigureServerEditor>();
        editor.title = "Server";
        editor.Show();
    }
}

public class ConfigureServerEditor : EditorWindow
{
    private bool configure = false;
    private string saved;

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Current Server [" + Profile.current + "] :");
        eServer currentServer = (eServer)EditorGUILayout.EnumPopup(Server.current);
        if(Server.current != currentServer)
        {
            currentServer.Select();

            if(configure)
            {
                configure = false;
                if(!string.IsNullOrEmpty(saved))
                {
                    currentServer.SetRoute(saved);
                }
            }
        }

        EditorGUILayout.BeginHorizontal();
        if(configure)
        {
            if(GUILayout.Button("Cancel"))
            {
                configure = false;
                currentServer.SetRoute(saved);
            }

            if(GUILayout.Button("Save"))
            {
                configure = false;
            }
        } else
        {
            if(GUILayout.Button("Reset"))
            {
                currentServer.SetRoute(currentServer.GetDefaultRoute());
            }

            if(GUILayout.Button("Edit"))
            {
                configure = true;
                saved = currentServer.GetRoute();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if(configure)
        {
            string route = EditorGUILayout.TextField("URL: ", currentServer.GetRoute());
            currentServer.SetRoute(route);
        } else
        {
            EditorGUILayout.LabelField("URL: " + currentServer.GetRoute());
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }
}