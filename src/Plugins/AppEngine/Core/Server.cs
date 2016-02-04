using System;
using UnityEngine;

namespace Core
{
    public enum eServer
    {
        Develop,
        Local,
        Custom
    }

    public static class Server
    {
        private const string SELECTED = "server.selected";

        public static eServer current
        {
            get
            {
                if(Profile.current.HasValue(SELECTED))
                {
                    return (eServer)Enum.Parse(typeof(eServer), Profile.current.GetValue<string>(SELECTED));
                }
                return eServer.Develop;
            }
        }

        public static void Select(eServer server)
        {
            Profile.current.SetValue<string>(SELECTED, server.ToString());
        }
    }

    public static class eServerExtensions
    {
        private const string SERVERS = "server.{0}.url";

        public static bool IsSelected(this eServer server)
        {
            return Server.current == server;
        }

        public static void Select(this eServer server)
        {
            Server.Select(server);
        }

        public static string GetRoute(this eServer server)
        {
            return PlayerPrefs.GetString(ServerURLKey(server), server.GetDefaultRoute());
        }

        public static void SetRoute(this eServer server, string route)
        {
            PlayerPrefs.SetString(ServerURLKey(server), route);
            PlayerPrefs.Save();
        }

        public static string GetDefaultRoute(this eServer server)
        {
            switch (server)
            {
                case eServer.Develop:
                return "http://104.155.103.95:7076";

                case eServer.Local:
                default:
                return "http://localhost:7076";

            }
        }

        private static string ServerURLKey(this eServer server)
        {
            return string.Format(SERVERS, server.ToString());
        }
    }
}