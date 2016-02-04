using UnityEngine;
using System.Collections;

namespace Core
{
    public class ObjectLoader : MonoBehaviour
    {
        public string resourcePath;
        public eAssetType assetType;

        private void Awake()
        {
            if(resourcePath != "")
                Core.Assets.CreateObject(resourcePath, assetType); else
                Debug.LogError("Could not load object with an empty resource!");

            Destroy(gameObject);
        }
    }
}