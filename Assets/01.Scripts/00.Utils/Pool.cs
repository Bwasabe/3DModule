using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public interface ISpawnable
{
    public void OnSpawned();
}

public interface IDespawnable
{
    public void OnDespawned();
}
//
// public interface IPoolable
// {
//     public void OnSpawned();
//     public void OnDespawned();
// }

public static class Pool
{
    private static readonly Dictionary<GameObject, PoolContainer> Pools = new();

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        Application.quitting += () => OnExiting?.Invoke();
        SceneManager.sceneUnloaded += scene => OnSceneUnloaded?.Invoke(scene);
    }

    private static event Action OnExiting;
    private static event Action<Scene> OnSceneUnloaded;

    /// <summary>
    ///     오브젝트를 풀에서 가져옵니다.
    /// </summary>
    /// <param name="prefab">프리팹</param>
    /// <param name="parent">부모</param>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    /// <returns>풀링된 오브젝트</returns>
    public static T Get<T>(T prefab, Transform parent = null) where T : Object
    {
        if (prefab == null) throw new ArgumentNullException(nameof(prefab));

        PoolContainer<T> pool = PoolContainer<T>.Instance;
        return pool.Get(prefab, parent);
    }

    /// <summary>
    ///     오브젝트를 풀에서 가져옵니다.
    /// </summary>
    /// <param name="prefab">프리팹</param>
    /// <param name="position">위치</param>
    /// <param name="rotation">회전</param>
    /// <param name="parent">부모</param>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    /// <returns>풀링된 오브젝트</returns>
    public static T Get<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Object
    {
        if (prefab == null) throw new ArgumentNullException(nameof(prefab));

        PoolContainer<T> pool = PoolContainer<T>.Instance;
        T obj = pool.Get(prefab, parent);

        GameObject gameObject = GetGameObject(obj);
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.SetPositionAndRotation(position, rotation);

        return obj;
    }

    /// <summary>
    ///     오브젝트를 풀에 반환합니다.
    /// </summary>
    /// <param name="obj">반환할 오브젝트</param>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    public static void Release<T>(T obj) where T : Object
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        GameObject gameObject = GetGameObject(obj);

        if (!Pools.TryGetValue(gameObject, out PoolContainer pool))
        {
            PoolContainer<GameObject>.Instance.Release(gameObject);
            return;
        }

        pool.Release(gameObject);
    }

    /// <summary>
    ///     오브젝트를 풀에 미리 생성합니다.
    /// </summary>
    /// <param name="prefab">프리팹</param>
    /// <param name="count">생성할 개수</param>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    public static void Preload<T>(T prefab, int count) where T : Object
    {
        if (prefab == null) throw new ArgumentNullException(nameof(prefab));

        PoolContainer<T> pool = PoolContainer<T>.Instance;
        for (int i = 0; i < count; i++)
        {
            T obj = pool.Get(prefab);
            pool.Release(obj);
        }
    }

    /// <summary>
    ///     수동으로 모든 풀을 비웁니다.
    /// </summary>
    public static void Clear()
    {
        foreach (PoolContainer pool in Pools.Values) pool.ClearPool();

        Pools.Clear();
    }

    private static GameObject GetGameObject<T>(T prefab) where T : Object
    {
        return prefab as GameObject ?? (prefab as Component)?.gameObject;
    }

    private abstract class PoolContainer
    {
        public abstract void Release(GameObject gameObject);
        public abstract void ClearPoolInScene(Scene scene);
        public abstract void ClearPool();
    }

    private class PoolContainer<T> : PoolContainer where T : Object
    {
        private readonly Dictionary<GameObject, T> _components = new();
        private readonly Dictionary<GameObject, ISpawnable[]> _spawnables = new();
        private readonly Dictionary<GameObject, IDespawnable[]> _despawnables = new();
        private readonly Dictionary<GameObject, T> _prefabs = new();
        private readonly Dictionary<T, List<T>> _lists = new();

        static PoolContainer()
        {
            OnExiting += Instance.ClearPool;
            OnSceneUnloaded += Instance.ClearPoolInScene;
        }

        public static PoolContainer<T> Instance { get; } = new();

        public T Get(T prefab, Transform parent = null)
        {
            if (_lists.TryGetValue(prefab, out List<T> list) == false)
            {
                list = new();
                _lists.Add(prefab, list);
            }

            T obj;
            GameObject gameObject;

            if (list.Count > 0)
            {
                obj = list[^1];
                list.RemoveAt(list.Count - 1);
                gameObject = GetGameObject(obj);
            }
            else
            {
                obj = Object.Instantiate(prefab, parent);
                gameObject = GetGameObject(obj);
                AddToManagedObject(prefab, gameObject, obj);
            }

            gameObject.transform.SetParent(parent, false);
            gameObject.hideFlags = HideFlags.None;
            gameObject.transform.SetAsLastSibling();

            if (GetGameObject(prefab).activeSelf)
                gameObject.SetActive(true);

            foreach (ISpawnable poolable in _spawnables[gameObject]) poolable.OnSpawned();

            return obj;
        }

        private void AddToManagedObject(T prefab, GameObject gameObject, T obj)
        {
            _prefabs.Add(gameObject, prefab);
            _components.Add(gameObject, obj);
            _spawnables.Add(gameObject, gameObject.GetComponentsInChildren<ISpawnable>(true));
            _despawnables.Add(gameObject, gameObject.GetComponentsInChildren<IDespawnable>(true));
            Pools.Add(gameObject, this);

            gameObject.name = $"[Pooled #{Pools.Count}] {prefab.name}";
        }

        public void Release(T obj)
        {
            Release(GetGameObject(obj));
        }

        public override void Release(GameObject gameObject)
        {
            if (_prefabs.TryGetValue(gameObject, out T prefab) == false)
            {
                gameObject.GetComponentsInChildren<IDespawnable>(true).ToList()
                    .ForEach(poolable => poolable.OnDespawned());

                Object.Destroy(gameObject);
                Debug.LogWarning($"[PoolManager] {gameObject.name} is not pooled object.");
                return;
            }

            if (_despawnables.TryGetValue(gameObject, out IDespawnable[] despawnables))
                foreach (IDespawnable poolable in despawnables)
                    poolable.OnDespawned();

            if (gameObject != null) 
                gameObject.SetActive(false);
            
            gameObject.hideFlags = HideFlags.HideInHierarchy;
            _lists[prefab].Add(_components[gameObject]);
        }

        public override void ClearPoolInScene(Scene scene)
        {
            List<T> liveObjects = new List<T>();

            foreach (T obj in _lists.Values.SelectMany(stack => stack))
            {
                if (obj == null) continue;
                GameObject gameObject = GetGameObject(obj);
                if (gameObject.scene != scene)
                {
                    liveObjects.Add(obj);
                    continue;
                }

                Object.Destroy(gameObject);
            }

            _lists.Clear();
            _prefabs.Clear();
            _spawnables.Clear();
            _despawnables.Clear();
            _components.Clear();

            foreach (T prefab in liveObjects.Select(Object.Instantiate))
            {
                AddToManagedObject(prefab, GetGameObject(prefab), prefab);
            }
        }

        public override void ClearPool()
        {
            foreach (T obj in _lists.Values.SelectMany(stack => stack))
            {
                if (obj == null) continue;
                GameObject gameObject = GetGameObject(obj);

                Object.Destroy(gameObject);
            }

            _lists.Clear();
            _prefabs.Clear();
            _spawnables.Clear();
            _despawnables.Clear();
            _components.Clear();
        }
    }
}