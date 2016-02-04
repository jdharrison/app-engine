using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public class AppEngine : MonoBehaviour
    {
        public const string STAGING_SCENE = "_Staging";

        public static AppEngine instance
        { get; private set; }

        public eState start = eState.MainMenu;

        public Core.StateController controller
        { get; private set; }

        private eState _state;
        public eState state
        {
            get
            {
                return _state;
            }
            set
            {
                if(_state == value)
                    return;

                _state = value;
                Setup();
            }
        }

        private void Awake()
        {
            if(instance != null)
            {
                Debug.LogWarning("Only one AppEngine may be active at once!");
                Destroy(gameObject);
                return;
            }

            Debug.Log("AppEngine ready!");
            instance = this;
            state = start;
        }

        private void Start()
        {
            GameObject resource = GameObject.Find(state.ToString());
            controller = (resource != null) ? (resource as GameObject).GetComponentInChildren<Core.StateController>() : null;

            if(controller != null)
            {
                controller.gameObject.transform.parent = gameObject.transform;
                Debug.Log("Changed to state: " + _state.ToString());
            } else if(resource != null)
            {
                Debug.LogError("Could not find StateController; make sure one is attached, active and at root!");
                Core.Assets.DestroyObject(resource);
            } else
            {
                Debug.LogError("Could not find resource: " + _state.GetPrefab());
            }
        }

        private void OnDestroy()
        {
            state = eState.Inactive;

            if(instance == this)
            {
                instance = null;
            }
        }

        private void Setup()
        {
            Cleanup();

            if(_state == eState.Inactive)
            {
                Debug.Log("AppEngine is now inactive.");
                return;
            }

            Assets.CreateObject(state.GetPrefab(), eAssetType.Scenes);
        }

        [ContextMenu("TEST")]
        public void Test()
        {
            if(GameObject.Find(state.ToString()) == null)
            {
                Debug.LogError(">>>>>.<<<<<");
            } else
            {
                Debug.LogError("FUCK");
            }
        }

        private void Cleanup()
        {
            if(controller != null)
            {
                Core.Assets.DestroyObject(controller.gameObject);
                controller = null;
            }

            System.GC.Collect();
        }
    }
}