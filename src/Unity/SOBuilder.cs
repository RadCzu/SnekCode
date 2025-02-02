using System;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectBuilder<T> where T : ScriptableObject
{
    public readonly List<T> objects;

    public ScriptableObjectBuilder(string resourcePath)
    {
        objects = LoadObjects(resourcePath);
    }

    public T GetObject(string name)
    {
        T foundObject = objects.Find(obj => obj.name == name);
        if (foundObject != null)
        {
            return foundObject;
        }
        else
        {
            Debug.LogError($"Object with name: {name} does not exist in the loaded resources.");
            return null;
        }
    }

    private List<T> LoadObjects(string path)
    {
        return new List<T>(Resources.LoadAll<T>(path));
    }
}