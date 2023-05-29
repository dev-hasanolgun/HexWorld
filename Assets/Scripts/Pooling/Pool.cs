using System;
using Object = UnityEngine.Object;

namespace Pooling
{
    public class Pool<T> where T : Object
    {
        public readonly UniqueQueue<T> PoolQueue;

        public T Prefab;
        public string Name;
        public int UniqueId;
        public Action<T> CallOnPull;
        public Action<T> CallOnPush;

        private static int s_instanceCounter;

        public Pool(T obj, int preSpawnAmount = 0)
        {
            Prefab = obj;
            UniqueId = ++s_instanceCounter;
            Name = UniqueId.ToString();
            PoolQueue = new UniqueQueue<T>();
            Spawn(obj, preSpawnAmount);
        }
        public Pool(string poolName, T obj, int preSpawnAmount = 0)
        {
            Prefab = obj;
            UniqueId = ++s_instanceCounter;
            Name = poolName;
            PoolQueue = new UniqueQueue<T>();
            Spawn(obj, preSpawnAmount);
        }
    
        public Pool(T obj, Action<T> callOnPull, Action<T> callOnPush, int preSpawnAmount = 0)
        {
            Prefab = obj;
            UniqueId = ++s_instanceCounter;
            Name = UniqueId.ToString();
            CallOnPull = callOnPull;
            CallOnPush = callOnPush;
            PoolQueue = new UniqueQueue<T>();
            Spawn(obj, preSpawnAmount);
        }
    
        public Pool(string poolName, T obj, Action<T> callOnPull, Action<T> callOnPush, int preSpawnAmount = 0)
        {
            UniqueId = ++s_instanceCounter;
            Name = poolName;
            CallOnPull = callOnPull;
            CallOnPush = callOnPush;
            PoolQueue = new UniqueQueue<T>();
            Spawn(obj, preSpawnAmount);
        }

        public void Spawn(T prefab, int spawnAmount)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                var obj = Object.Instantiate(prefab);
                obj.name = prefab.name;
                obj.GameObject().SetActive(false);
                PoolQueue.Enqueue(obj);
            }
        }
    }
}