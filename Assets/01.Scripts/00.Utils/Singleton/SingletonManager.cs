using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public enum SingletonLifeTime
{
    Scene,
    Application
}

[AttributeUsage(AttributeTargets.Class)]
public class SingletonLifeTimeAttribute : Attribute
{
    public SingletonLifeTime LifeTime { get; }

    public SingletonLifeTimeAttribute(SingletonLifeTime lifeTime)
    {
        LifeTime = lifeTime;
    }

    public SingletonLifeTimeAttribute()
    {
        LifeTime = SingletonLifeTime.Scene;
    }
}


public static class SingletonManager
{
    private static readonly Dictionary<SingletonLifeTime, List<MonoBehaviour>> InstanceDictionary = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadSingleton()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        foreach (SingletonLifeTime lifeTime in Enum.GetValues(typeof(SingletonLifeTime)))
        {
            InstanceDictionary.Add(lifeTime, new());
        }
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        InstanceDictionary[SingletonLifeTime.Scene].Clear();
    }

    public static void Register(MonoBehaviour instance)
    {
        Attribute att = Attribute.GetCustomAttribute(instance.GetType(), typeof(SingletonLifeTimeAttribute));

        if (att is not SingletonLifeTimeAttribute singletonLifeTimeAttribute)
        {
            Debug.LogError($"{instance.GetType()} haven't SingletonLifeTimeAttribute");
            return;
        }

        InstanceDictionary[singletonLifeTimeAttribute.LifeTime].Add(instance);

        if (singletonLifeTimeAttribute.LifeTime is not SingletonLifeTime.Scene)
            Object.DontDestroyOnLoad(instance.transform.root);
    }
}