using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static void SetPlayerPref<T>(string property, object value)
    {
        property = property.ToLower();

        if(typeof(T) == typeof(string))
        {
            PlayerPrefs.SetString(property, (string)value);
        } else if(typeof(T) == typeof(int))
        {
            PlayerPrefs.SetInt(property, (int)value);
        } else if(typeof(T) == typeof(float))
        {
            PlayerPrefs.SetFloat(property, (float)value);
        }

        PlayerPrefs.Save();
    }

    public static T GetPlayerPref<T>(string property)
    {
        property = property.ToLower();

        if(typeof(T) == typeof(string))
        {
            return (T)(object)PlayerPrefs.GetString(property);
        } else if(typeof(T) == typeof(int))
        {
            return (T)(object)PlayerPrefs.GetInt(property);
        } else if(typeof(T) == typeof(float))
        {
            return (T)(object)PlayerPrefs.GetFloat(property);
        }
        return default(T);
    }

    public static object GetPlayerPref(string property)
    {
        return PlayerPrefs.GetString(property, PlayerPrefs.GetFloat(property, (float)PlayerPrefs.GetInt(property)).ToString());
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
            } else
            {
                target = target.parent;
            }
        }
        return default(T);
    }
}