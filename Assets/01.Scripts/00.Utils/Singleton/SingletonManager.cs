using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public enum LifeTime
{
    Scene,
    Application
}

[AttributeUsage(AttributeTargets.Class)]
public class SingletonLifeTimeAttribute : Attribute
{
    public LifeTime LifeTime { get; }

    public SingletonLifeTimeAttribute(LifeTime lifeTime)
    {
        LifeTime = lifeTime;
    }

    public SingletonLifeTimeAttribute()
    {
        LifeTime = LifeTime.Scene;
    }
}


public static class SingletonManager
{
    private static readonly Dictionary<LifeTime, Dictionary<Type, MonoBehaviour>> InstanceDictionary = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadSingleton()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        foreach (LifeTime lifeTime in Enum.GetValues(typeof(LifeTime)))
        {
            InstanceDictionary.Add(lifeTime, new());
        }
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        InstanceDictionary[LifeTime.Scene].Clear();
    }

    public static void Register(MonoBehaviour instance)
    {
        Attribute att = Attribute.GetCustomAttribute(instance.GetType(), typeof(SingletonLifeTimeAttribute));

        if (att is not SingletonLifeTimeAttribute singletonLifeTimeAttribute)
        {
            Debug.LogError($"{instance.GetType()} haven't SingletonLifeTimeAttribute");
            return;
        }
        
        if (InstanceDictionary[singletonLifeTimeAttribute.LifeTime].TryGetValue(instance.GetType(), out MonoBehaviour oldInstance))
        {
            Debug.LogError($"{instance.GetType()} Destroyed");
            Object.Destroy(oldInstance.gameObject);
            InstanceDictionary[singletonLifeTimeAttribute.LifeTime].Remove(instance.GetType());
        }

        InstanceDictionary[singletonLifeTimeAttribute.LifeTime].Add(instance.GetType(), instance);

        if (singletonLifeTimeAttribute.LifeTime is not LifeTime.Scene)
            Object.DontDestroyOnLoad(instance.transform.root);
    }
}