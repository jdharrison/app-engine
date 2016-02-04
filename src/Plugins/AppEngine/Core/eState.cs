using System;

namespace Core
{
    public enum eState
    {
        Inactive,
        MainMenu,
        Loading,
        Selection,
        Game,
        Results
    }

    public static class StateConfig
    {
        public static string GetPrefab(this eState state)
        {
            return state.ToString();
        }
    }
}