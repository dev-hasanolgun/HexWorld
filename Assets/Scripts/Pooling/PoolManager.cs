using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Pooling
{
    public static class PoolManager<T> where T : UnityObject
    {
        #region Fields

        private static readonly Dictionary<string,Pool<T>> s_poolDictionary = new Dictionary<string,Pool<T>>();

        #endregion

        #region Constructors

        static PoolManager()
        {
            PoolInstance.Create();
            PoolInstance.CreateParent();
            PoolManagerTracker.AllPoolManagers.Add(ClearPool);
        }

        #endregion

        #region PushString

        public static void PoolObject(string poolName, T objectToPool)
        {
            if (s_poolDictionary.TryGetValue(poolName, out var thisPool))
            {
                thisPool.PoolQueue.Enqueue(objectToPool);
                thisPool.CallOnPush?.Invoke(objectToPool);
            }
            else
            {
                var newPool = new Pool<T>(poolName, objectToPool);
                s_poolDictionary.Add(poolName, newPool);
                newPool.PoolQueue.Enqueue(objectToPool);
            }
        }

        #endregion

        #region PushPoolClass

        public static void PoolObject(Pool<T> pool, T objectToPool)
        {
            if (s_poolDictionary.TryGetValue(pool.Name, out var thisPool))
            {
                thisPool.PoolQueue.Enqueue(objectToPool);
                thisPool.CallOnPush?.Invoke(objectToPool);
            }
            else
            {
                s_poolDictionary.Add(pool.Name, pool);
                pool.PoolQueue.Enqueue(objectToPool);
                pool.CallOnPush?.Invoke(objectToPool);
            }
        }

        #endregion

        #region PullString

        private static T GetObjectFromPool(string poolName, T objectToGet)
        {
            if (s_poolDictionary.TryGetValue(poolName, out var thisPool))
            {
                if (thisPool.PoolQueue.Count == 0)
                {
                    return CreateNewObject(objectToGet);
                }
            
                var obj = thisPool.PoolQueue.Dequeue();
                thisPool.CallOnPull?.Invoke(obj);
                return obj;
            }
        
            return CreateNewObject(objectToGet);
        }

        #endregion

        #region PullPoolClass

        private static T GetObjectFromPool(Pool<T> pool, T objectToGet)
        {
            var poolCheck = s_poolDictionary.TryGetValue(pool.Name, out _);

            if (!poolCheck) s_poolDictionary.Add(pool.Name, pool);
            
            if (pool.PoolQueue.Count == 0)
            {
                return CreateNewObject(objectToGet);
            }
            
            var obj = pool.PoolQueue.Dequeue();
            pool.CallOnPull?.Invoke(obj);
            return obj;
        }

        #endregion

        #region RetrieveString

        public static T RetrieveObject(string poolName, T prefab, bool autoParent = false)
        {
            var obj = GetObjectFromPool(poolName, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return obj;
        }
    
        public static T RetrieveObject(string poolName, T prefab, Vector3 position, bool autoParent = false)
        {
            var obj = GetObjectFromPool(poolName, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return obj;
        }
    
        public static T RetrieveObject(string poolName, T prefab, Vector3 position, Quaternion rotation, bool autoParent = false)
        {
            var obj = GetObjectFromPool(poolName, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            go.transform.rotation = rotation;
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return obj;
        }
    
        public static T RetrieveObject(string poolName, T prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var obj = GetObjectFromPool(poolName, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.transform.SetParent(parent);
            return obj;
        }

        #endregion

        #region RetrievePoolClass

        public static T RetrieveObject(Pool<T> pool, T prefab, bool autoParent = false)
        {
            var obj = GetObjectFromPool(pool, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return obj;
        }
    
        public static T RetrieveObject(Pool<T> pool, T prefab, Vector3 position, bool autoParent = false)
        {
            var obj = GetObjectFromPool(pool, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return obj;
        }
    
        public static T RetrieveObject(Pool<T> pool, T prefab, Vector3 position, Quaternion rotation, bool autoParent = false)
        {
            var obj = GetObjectFromPool(pool, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            go.transform.rotation = rotation;
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return obj;
        }
    
        public static T RetrieveObject(Pool<T> pool, T prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var obj = GetObjectFromPool(pool, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.transform.SetParent(parent);
            return obj;
        }

        #endregion

        #region RetrieveGOString

        public static GameObject RetrieveGameObject(string poolName, T prefab, bool autoParent = false)
        {
            var obj = GetObjectFromPool(poolName, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return go;
        }
    
        public static GameObject RetrieveGameObject(string poolName, T prefab, Vector3 position, bool autoParent = false)
        {
            var obj = GetObjectFromPool(poolName, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return go;
        }
    
        public static GameObject RetrieveGameObject(string poolName, T prefab, Vector3 position, Quaternion rotation, bool autoParent = false)
        {
            var obj = GetObjectFromPool(poolName, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            go.transform.rotation = rotation;
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return go;
        }
    
        public static GameObject RetrieveGameObject(string poolName, T prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var obj = GetObjectFromPool(poolName, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.transform.SetParent(parent);
            return go;
        }

        #endregion

        #region RetrieveGOPoolClass

        public static GameObject RetrieveGameObject(Pool<T> pool, T prefab, bool autoParent = false)
        {
            var obj = GetObjectFromPool(pool, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return go;
        }
    
        public static GameObject RetrieveGameObject(Pool<T> pool, T prefab, Vector3 position, bool autoParent = false)
        {
            var obj = GetObjectFromPool(pool, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return go;
        }
    
        public static GameObject RetrieveGameObject(Pool<T> pool, T prefab, Vector3 position, Quaternion rotation, bool autoParent = false)
        {
            var obj = GetObjectFromPool(pool, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            go.transform.rotation = rotation;
            if (autoParent) go.transform.SetParent(PoolInstance.NonPersistentParent);
            return go;
        }
    
        public static GameObject RetrieveGameObject(Pool<T> pool, T prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var obj = GetObjectFromPool(pool, prefab);
            var go = obj.GameObject();
            go.SetActive(true);
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.transform.SetParent(parent);
            return go;
        }

        #endregion

        #region Utility

        private static T CreateNewObject(T objectToCreate)
        {
            var obj = UnityObject.Instantiate(objectToCreate);
            obj.name = objectToCreate.name;
            return obj;
        }

        public static void ClearPool()
        {
            s_poolDictionary.Clear();
        }

        #endregion
    }

    #region GenericClassTracker

    public static class PoolManagerTracker
    {
        public static List<Action> AllPoolManagers = new List<Action>();

        public static void ClearPoolDictionary()
        {
            AllPoolManagers.ForEach(m => m());
        }
    }

    #endregion

    #region GlobalGenericClassInstance

    public static class PoolInstance
    {
        public static PoolComponent Instance;
        public static Transform NonPersistentParent;

        public static void Create()
        {
            if (Instance != null) return;
            
            var target = new GameObject("[PoolManager]");
            UnityObject.DontDestroyOnLoad(target);
            Instance = target.AddComponent<PoolComponent>();
        }
        
        public static void CreateParent()
        {
            if (NonPersistentParent != null) return;
            
            var parent = new GameObject("[PoolParent]");
            NonPersistentParent = parent.transform;
        }
    }

    #endregion
}