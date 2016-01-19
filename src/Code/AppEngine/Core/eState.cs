using System;

//TODO: Make a editor that allows to customize this
//      by procedurally creating the file
namespace Core
{
	public enum eState
	{
		Inactive,
		MainMenu,
		Loading,
		CharacterSelect,
		Game,
		Results
	}

	public static class StateConfig
	{
		public static string GetPrefab(this eState state)
		{
			return "State/" + state.ToString();
		}
	}
}