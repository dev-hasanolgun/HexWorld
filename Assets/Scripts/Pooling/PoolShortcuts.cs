using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Pooling
{
    public static class PoolShortcuts
    {
        #region PoolString

        public static void Pool<T>(this T objectToPool, string poolName, bool autoParent = false) where T : UnityObject
        {
            PoolManager<T>.PoolObject(poolName, objectToPool);
        }

        #endregion

        #region PoolPoolClass

        public static void Pool<T>(this T objectToPool, Pool<T> pool) where T : UnityObject
        {
            PoolManager<T>.PoolObject(pool, objectToPool);
        }

        #endregion

        #region RetrieveString

        public static T Retrieve<T>(this T objectToGet, string poolName, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveObject(poolName, objectToGet, autoParent);
        }
    
        public static T Retrieve<T>(this T objectToGet, string poolName, Vector3 position, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveObject(poolName, objectToGet, position, autoParent);
        }
    
        public static T Retrieve<T>(this T objectToGet, string poolName, Vector3 position, Quaternion rotation, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveObject(poolName, objectToGet, position, rotation, autoParent);
        }
    
        public static T Retrieve<T>(this T objectToGet, string poolName, Vector3 position, Quaternion rotation, Transform parent) where T : UnityObject
        {
            return PoolManager<T>.RetrieveObject(poolName, objectToGet, position, rotation, parent);
        }

        #endregion

        #region RetrievePoolClass

        public static T Retrieve<T>(this T objectToGet, Pool<T> pool, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveObject(pool, objectToGet, autoParent);
        }
    
        public static T Retrieve<T>(this T objectToGet, Pool<T> pool, Vector3 position, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveObject(pool, objectToGet, position, autoParent);
        }
    
        public static T Retrieve<T>(this T objectToGet, Pool<T> pool, Vector3 position, Quaternion rotation, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveObject(pool, objectToGet, position, rotation, autoParent);
        }
    
        public static T Retrieve<T>(this T objectToGet, Pool<T> pool, Vector3 position, Quaternion rotation, Transform parent) where T : UnityObject
        {
            return PoolManager<T>.RetrieveObject(pool, objectToGet, position, rotation, parent);
        }

        #endregion

        #region RetrieveGOString

        public static GameObject RetrieveGameObject<T>(this T objectToGet, string poolName, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveGameObject(poolName, objectToGet, autoParent);
        }
    
        public static GameObject RetrieveGameObject<T>(this T objectToGet, string poolName, Vector3 position, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveGameObject(poolName, objectToGet, position, autoParent);
        }
    
        public static GameObject RetrieveGameObject<T>(this T objectToGet, string poolName, Vector3 position, Quaternion rotation, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveGameObject(poolName, objectToGet, position, rotation, autoParent);
        }
    
        public static GameObject RetrieveGameObject<T>(this T objectToGet, string poolName, Vector3 position, Quaternion rotation, Transform parent) where T : UnityObject
        {
            return PoolManager<T>.RetrieveGameObject(poolName, objectToGet, position, rotation, parent);
        }

        #endregion

        #region RetrieveGOPoolClass

        public static GameObject RetrieveGameObject<T>(this T objectToGet, Pool<T> pool, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveGameObject(pool, objectToGet, autoParent);
        }
    
        public static GameObject RetrieveGameObject<T>(this T objectToGet, Pool<T> pool, Vector3 position, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveGameObject(pool, objectToGet, position, autoParent);
        }
    
        public static GameObject RetrieveGameObject<T>(this T objectToGet, Pool<T> pool, Vector3 position, Quaternion rotation, bool autoParent = false) where T : UnityObject
        {
            return PoolManager<T>.RetrieveGameObject(pool, objectToGet, position, rotation, autoParent);
        }
    
        public static GameObject RetrieveGameObject<T>(this T objectToGet, Pool<T> pool, Vector3 position, Quaternion rotation, Transform parent) where T : UnityObject
        {
            return PoolManager<T>.RetrieveGameObject(pool, objectToGet, position, rotation, parent);
        }

        #endregion

        #region PoolHelper

        public static GameObject GameObject(this UnityObject uo)
        {
            switch (uo)
            {
                case GameObject gameObject:
                    return gameObject;
                case Component component:
                    return component.gameObject;
                default:
                    return null;
            }
        }

        #endregion
    }
}