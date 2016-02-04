using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public enum eAssetType
    {
        Resources,
        Bundles,
        Scenes
    }

    //TODO: replace asset
    //    public class Asset
    //    {
    //        public string path
    //        { get; private set; }
    //        public eAssetType type
    //        { get; private set; }
    //        public Object reference
    //        { get; private set; }
    //
    //        public Asset(string path, eAssetType type)
    //        {
    //            Load(path, type);
    //        }
    //
    //        public void Load(string path, eAssetType type)
    //        {
    //            this.path = path;
    //            this.type = type;
    //
    //            //TODO: load reference
    //        }
    //    }

    public class Assets
    {
        private static Dictionary<string,int> counts = new Dictionary<string,int>();
        private static Dictionary<string,Object> references = new Dictionary<string,Object>();

        public static void LoadAssetMap(List<string> assetPaths, eAssetType assetType, System.Action<Dictionary<string,Object>> callback)
        {
            Dictionary<string,Object> assetMap = new Dictionary<string,Object>();

            if(assetPaths == null)
            {
                Debug.LogWarning("Trying to load a asset map with null asset paths!");
                if(callback != null)
                {
                    callback(assetMap);
                }
                return;
            }

            foreach (string assetPath in assetPaths)
            {
                LoadAsset(assetPath, assetType, delegate(Object asset)
                {
                    if(asset != null)
                    {
                        assetMap.Add(assetPath, asset);
                    }

                    if(callback != null && assetPaths.Count - 1 == assetPaths.IndexOf(assetPath))
                    {
                        callback(assetMap);
                    }
                });
            }
        }

        public static Dictionary<string,Object> UnloadAssetMap(Dictionary<string,Object> assetMap)
        {
            if(assetMap == null)
            {
                Debug.LogWarning("Trying to unload a null asset map!");
                return assetMap;
            }

            foreach (KeyValuePair<string,Object> entry in assetMap)
            {
                UnloadAsset(entry.Key);
            }

            assetMap.Clear();
            return assetMap;
        }

        public static void LoadAsset(string path, eAssetType assetType, System.Action<Object> callback = null)
        {
            if(!references.ContainsKey(path))
            {
                int watchId = Core.Stopwatch.Start();
                Object reference = Resources.Load(path);
                double time = Core.Stopwatch.End(watchId);

                if(reference == null)
                {
                    Debug.LogError("Unable to find asset: " + path);
                    if(callback != null)
                    {
                        callback(null);
                    }
                    return;
                } else
                {
                    references[path] = reference;
                    Debug.Log("Loaded asset: " + path + " (" + time + " ms)");
                }
            }

            int current = counts.ContainsKey(path) ? counts[path] : 0;
            counts[path] = current++;

            if(callback != null)
            {
                callback(references[path]);
            }
        }

        public static void UnloadAsset(string path)
        {
            if(references.ContainsKey(path) && counts.ContainsKey(path))
            {
                counts[path]--;
                if(counts[path] <= 0)
                {
                    int watchId = Core.Stopwatch.Start();

                    counts.Remove(path);
                    references.Remove(path);

                    Resources.UnloadUnusedAssets();

                    double time = Core.Stopwatch.End(watchId);
                    Debug.Log("Unloaded asset: " + path + "(" + time + ") ms");
                }
            }
        }

        public static void CreateObject(string path, eAssetType assetType, System.Action<Object> callback = null)
        {
            if(assetType == eAssetType.Scenes)
            {
                Application.LoadLevelAdditive(path);

                if(callback != null)
                {
                    callback(GameObject.Find(path));
                }
            } else
            {
                LoadAsset(path, assetType, delegate(Object resource)
                {
                    Object instance = null;
                    if(resource != null)
                    {
                        instance = Object.Instantiate(resource);
                        instance.name = instance.name.Replace("(Clone)", "");
                    }

                    if(callback != null)
                    {
                        callback(instance);
                    }
                });
            }
        }

        public static void DestroyObject(Object target)
        {
            string assetName = "";
            foreach (KeyValuePair<string,Object> pair in references)
            {
                if(pair.Value.name == target.name)
                {
                    assetName = pair.Key;
                    break;
                }
            }

            if(assetName != "")
                UnloadAsset(assetName);

            Object.Destroy(target);
        }
    }
}