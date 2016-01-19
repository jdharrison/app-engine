using UnityEngine;
using System.Collections;
using System;

namespace Core
{
	public class AppEngine : MonoBehaviour 
	{
		public const string STAGING_SCENE = "_Staging";
		
		public static AppEngine instance
		{
			get;
			private set;
		}
		
		public eState start = eState.MainMenu;
		
		public Core.StateController controller
		{
			get;
			private set;
		}

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
		
		private void Start()
		{
			if (instance != null)
			{
				Debug.LogWarning("Only one AppEngine may be active at once!");
				Destroy(gameObject);
				return;
			}
			
			Debug.Log ("AppEngine ready!");
			instance = this;
			state = start;
		}
		
		private void OnDestroy()
		{
			state = eState.Inactive;
			
			if (instance == this)
			{
				instance = null;
			}
		}
		
		private void Setup()
		{
			Cleanup ();
			
			if (_state == eState.Inactive) 
			{
				Debug.Log ("AppEngine is now inactive.");
				return;
			}

			GameObject resource = Core.Assets.CreateObject(_state.GetPrefab()) as GameObject;
			controller = (resource != null) ? resource.GetComponent<Core.StateController> () : null;
			
			if (controller != null) 
			{
				controller.gameObject.transform.parent = gameObject.transform;
				Debug.Log("Changed to state: " + _state.ToString());
			} 
			else 
			{
				Debug.LogError ("Could not find StateController; make sure one is attached, active and at root!");
				
				if (resource != null)
					Core.Assets.DestroyObject(resource);
			}
		}
		
		private void Cleanup()
		{
			if (controller != null) 
			{
				Core.Assets.DestroyObject(controller.gameObject);
				controller = null;
			}
			
			System.GC.Collect ();
		}
	}
}