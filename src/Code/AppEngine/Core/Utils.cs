using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Utils 
{
	public static string FormatSecondsToTime(TimeSpan time, string empty = "--")
	{
		if (time.TotalMinutes == 0) 
			return empty;

		string days = time.Days == 0 ? "" : time.Days + "d ";
		string hours = time.Hours == 0 ? "" : time.Hours + "h ";
		string minutes = time.Minutes == 0 ? "" : time.Minutes + "m ";
		return days + hours + minutes;
	}

	public static T FindParentComponent<T>(GameObject gameObject)
	{
		Transform target = (gameObject == null) ? null : gameObject.transform;
		while (target != null) 
		{
			T result = target.GetComponent<T>();
			if(result != null)
			{
				return result;
			} 
			else
			{
				target = target.parent;
			}
		}
		return default(T);
	}
}