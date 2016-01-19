using UnityEngine;
using System.Collections;

namespace Core
{
	public class ObjectLoader : MonoBehaviour 
	{
		public string resourcePath;
		
		private void Start ()
		{
			if (resourcePath != "") 
				Core.Assets.CreateObject(resourcePath);
			else
				Debug.LogError ("Could not load object with an empty resource!");
			
			Destroy (gameObject);
		}
	}
}