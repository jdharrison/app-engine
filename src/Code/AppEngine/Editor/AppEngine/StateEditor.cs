using UnityEngine;
using UnityEditor;
using System.Collections;

public class StateEditor : EditorWindow
{
	[MenuItem("AppEngine/Profiles/Clear All")]
	public static void ResetProfile()
	{
		PlayerPrefs.DeleteAll();
	}

	[MenuItem("AppEngine/States/Configure")]
	public static void EditStates()
	{
		StateEditor editor = EditorWindow.GetWindow<StateEditor>();
		editor.title = "State Editor";
		editor.Show();
	}
}