using System;
using System.Collections;
using UnityEngine;

namespace Core
{
	public abstract class StateModule : MonoBehaviour
	{
		private ArrayList intervalIds = new ArrayList();

		private StateController _controller;
		public StateController controller
		{
			get
			{
				if(_controller == null)
				{
					_controller = Utils.FindParentComponent<StateController>(gameObject);
					_controller.Register(this);
				}
				return _controller;
			}

			private set
			{
				_controller = value;
			}
		}

		protected int SetInterval(float time, System.Action callback)
		{
			int id = 0;
			while(intervalIds.Contains(id)) 
				id++;

			intervalIds.Add(id);
			StartCoroutine(_SetInterval(id, time, callback));

			return id;
		}

		protected bool ClearInterval(int id)
		{
			bool contains = intervalIds.Contains(id); 
			if (contains) 
				intervalIds.Remove(id);
			return contains;
		}

		private IEnumerator _SetInterval(int id, float time, System.Action callback)
		{
			while (intervalIds.Contains(id)) 
			{
				callback();
				yield return new WaitForSeconds (time);
			}
		}

		protected void SetTimeout(float time, System.Action callback)
		{
			StartCoroutine(_SetTimeout(time,callback));
		}
		
		private IEnumerator _SetTimeout(float time, System.Action callback)
		{
			yield return new WaitForSeconds(time);
			
			callback();
		}

		protected virtual void OnDestroy()
		{
			controller.Remove(this);
		}
	}
}