using LN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
namespace Ln
{
    public interface IRecyclable
    {
        public bool IsRecycled { get; set; }
        public int MaxCacheSize { get; }
        public RecyclableType RecyclableType { get; }
        public void OnRecycle();
        public void OnInitialize();
    }
    public enum RecyclableType
    {
        Bullet,
        ExplosionParticle,
        Unknow,

    }
    public class RecyclableFactory
    {
        public static IRecyclable Create(RecyclableType poolType)
        {
            if (poolType == RecyclableType.Bullet)
            {
                //Create()

            }

            return null;

        }
        public static IRecyclable Create(GameObject prefab)
        {
            GameObject gameObject = GameObject.Instantiate<GameObject>(prefab);
            return gameObject.GetComponent<IRecyclable>();

        }
    }
    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {

        Dictionary<RecyclableType, Stack<IRecyclable>> pools = new();
        Dictionary<RecyclableType, Func<IRecyclable>> createFuncs = new();


        
        public void RegisterCreateFunc(RecyclableType type, Func<IRecyclable> func)
        {
            createFuncs.TryAdd(type, func);
        }

        public IRecyclable PopAndInitializeObject(RecyclableType poolType)
        {
            Stack<IRecyclable> pool;
            IRecyclable recyclableObject = null;
            pool = GetPool(poolType);
            while (pool.Count > 0)
            {
                recyclableObject = pool.Pop();
                if (recyclableObject == null)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            if (recyclableObject == null)
            {
                recyclableObject = createFuncs[poolType].Invoke();
            }

            recyclableObject.OnInitialize();
            recyclableObject.IsRecycled = false;

            return recyclableObject;


        }

        //public bool Recycle(IRecyclable recyclableObject, float time = 0)
        //{
        //    Stack<IRecyclable> pool = GetPool(recyclableObject.PoolType);
        //    if (pool.Count >= MaxCount) return false;
        //    IEnumerator RecyCleCoroutine()
        //    {
        //        yield return new WaitForSeconds(time);
        //        if (recyclableObject != null)
        //        {
        //            //gameObject.SetActive(false);
        //            recyclableObject.OnRecycle();
        //            pool.Push(recyclableObject);
        //        }
        //    }
        //    StartCoroutine(RecyCleCoroutine());
        //    return true;

        //}
        public bool Recycle(IRecyclable recyclableObject)
        {
            Stack<IRecyclable> pool = GetPool(recyclableObject.RecyclableType);
            if (pool.Count >= recyclableObject.MaxCacheSize)
            {
                return false;
            }
            if (recyclableObject != null)
            {
                recyclableObject.OnRecycle();
                recyclableObject.IsRecycled = true;
                pool.Push(recyclableObject);
            }
            return true;

        }
        private Stack<IRecyclable> GetPool(RecyclableType poolType)
        {
            if (!pools.TryGetValue(poolType, out Stack<IRecyclable> pool))
            {
                pool = new Stack<IRecyclable>();
                pools.Add(poolType, pool);
            }
            return pool;
        }

        public void Clear()
        {

            foreach (Stack<IRecyclable> pool in pools.Values)
            {
                pool.Clear();
            }
            pools.Clear();

        }
    }
}




