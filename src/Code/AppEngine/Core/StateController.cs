using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Core
{
	public class StateController : StateController<StateModule> { }

	public abstract class StateController<T> : MonoBehaviour
	{
		public eState state;

		private AppEngine _engine;
		public AppEngine engine 
		{
			get 
			{
				if (_engine == null) 
				{
					_engine = Utils.FindParentComponent<AppEngine>(gameObject);
				}
				return _engine;
			}

			private set
			{
				_engine = value;
			}
		}

		private Dictionary<Type,List<T>> modules;

		private void Awake()
		{
			modules = new Dictionary<Type,List<T>>();
		}
		
		private void OnDestroy()
		{
			modules = null;
		}

		public void ChangeState(eState state)
		{
			if (engine == null)
			{
				Debug.LogWarning ("Can not change state while in debug mode.");
				return;
			}
			engine.state = state;
		}

		public virtual void Register(T module)
		{
			Type type = module.GetType();
			modules[type] = modules[type] ?? new List<T>();
			modules[type].Add(module);
		}

		public bool Remove(T module)
		{
			Type type = module.GetType();
			if (modules.ContainsKey(type)) 
			{
				bool result = modules[type].Remove(module);
				if(modules[type].Count == 0)
				{
					modules.Remove(type);
				}
				return result;
			}
			return false;
		}

		public bool HasModule(T module)
		{
			return modules.ContainsKey(module.GetType());
		}

		public bool HasModule<J>()
		{
			return modules.ContainsKey(typeof(J));
		}

		public List<J> GetModules<J>()
		{
			Type type = typeof(J);
			if(modules.ContainsKey(type))
		   	{
				return (List<J>)Convert.ChangeType(modules[type], typeof(List<J>));
			}
			return default(List<J>);
		}

		public J GetModule<J>()
		{
			Type type = typeof(J);
			if(modules.ContainsKey(type))
			{
				return GetModules<J>()[0];
			}
			return default(J);
		}
	}
}