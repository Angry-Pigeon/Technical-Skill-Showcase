using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class SerializedResourcedScriptableObject<T> : SerializedScriptableObject where T : SerializedResourcedScriptableObject<T>
{
    private const string path = "SingletonDataSO/";
    public static T Instance()
    {
        if (_instance != null) return _instance;
        #if UNITY_EDITOR
        string fullPath = $"{path}{typeof(T).Name}";
        _instance = Resources.Load<T>(fullPath);
        if (_instance == null)
        {
            _instance = CreateInstance<T>();
            Directory.CreateDirectory($"{Application.dataPath}/Resources/{path}");
            UnityEditor.AssetDatabase.CreateAsset(_instance, $"Assets/Resources/{fullPath}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
        }
        #endif

        _instance = Resources.Load<T>($"{path}{typeof(T).Name}");
        return _instance;
    }

    private static T _instance;
}
