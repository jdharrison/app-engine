using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class Assets
	{
		private static Dictionary<string,int> counts = new Dictionary<string,int>();
		private static Dictionary<string,Object> references = new Dictionary<string,Object>();

		public static Dictionary<string,Object> LoadAssetMap(List<string> assetPaths)
		{
			Dictionary<string,Object> assetMap = new Dictionary<string,Object>();
			
			if (assetPaths == null) {
				Debug.LogWarning("Trying to load a asset map with null asset paths!");
				return assetMap;
			}

			foreach (string assetPath in assetPaths) 
			{
				Object asset = LoadAsset(assetPath);
				if(asset != null)
					assetMap.Add(assetPath,asset);
			}
			return assetMap;
		}

		public static Dictionary<string,Object> UnloadAssetMap(Dictionary<string,Object> assetMap)
		{
			if (assetMap == null) {
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

		public static Object LoadAsset(string path)
		{
			if (!references.ContainsKey (path))
			{
				int watchId = Core.Stopwatch.Start();

				Object reference = Resources.Load (path);
				if(reference == null)
				{
					Debug.LogError("Unable to find asset: " + path);
					return null;
				}
				references[path] = reference;

				double time = Core.Stopwatch.End(watchId);
				Debug.Log ("Loaded asset: " + path + " (" + time + " ms)");
			}

			int current = counts.ContainsKey (path) ? counts [path] : 0;
			counts [path] = current++;

			return references[path];
		}

		public static void UnloadAsset(string path)
		{
			if (references.ContainsKey (path) && counts.ContainsKey(path)) 
			{
				counts[path]--;
				if(counts[path] <= 0)
				{
					int watchId = Core.Stopwatch.Start();

					counts.Remove(path);
					references.Remove(path);

					Resources.UnloadUnusedAssets();

					double time = Core.Stopwatch.End(watchId);
					Debug.Log ("Unloaded asset: " + path + "(" + time + ") ms");
				}
			}
		}
		
		public static Object CreateObject(string path)
		{
			Object resource = LoadAsset(path);
			if(resource != null)
			{
				Object instance = Object.Instantiate(resource);
				instance.name = instance.name.Replace("(Clone)","");
				return instance;
			}
			return null;
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

			if (assetName != "")
				UnloadAsset (assetName);

			Object.Destroy (target);
		}
	}
}