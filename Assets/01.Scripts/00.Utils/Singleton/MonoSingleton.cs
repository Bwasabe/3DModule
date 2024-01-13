using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    protected virtual void Awake()
    {
        _instance = this as T;

        SingletonManager.Register(_instance);
    }

    public static bool IsExist() => _instance != null;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType<T>();

            if (_instance == null)
            {
                Debug.LogWarning($"{typeof(T)} is not exist. Creating new instance.");
                _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
            }

            _instance.transform.SetParent(null);

            SingletonManager.Register(_instance);

            return _instance;
        }
    }

    protected virtual void OnDestroy()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
        _instance = null;
    }
}