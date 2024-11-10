using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : MonoSingleton<PoolManager>
{
    public PoolListSO poolList;

    private Dictionary<string, Pool> pools;

    private void Awake()
    {
        pools = new Dictionary<string, Pool>();
        foreach (PoolItemSO item in poolList.list)
        {
            CreatePool(item);
        }
    }

    private void CreatePool(PoolItemSO item)
    {
        IPoolable poolable = item.prefab.GetComponent<IPoolable>();
        if (poolable == null)
        {
            Debug.LogWarning($"풀 이름 못찾음 {item.prefab.name}");
            return;
        }
        
        Pool pool = new Pool(poolable, transform, item.count);
        pools.Add(poolable.PoolName, pool);
    }
    
    public IPoolable Pop(string poolName)
    {
        if (pools.ContainsKey(poolName))
        {
            IPoolable item = pools[poolName].Pop();
            item.ResetItem();
            return item;

        }
        Debug.LogError($"그런 이름 없어{poolName}");
        return null;

    }

    public void Push(IPoolable item)
    {
        if (pools.ContainsKey(item.PoolName))
        {
            pools[item.PoolName].Push(item);
            return;
        }
        Debug.LogError($"그런 이름 없어{item.PoolName}");
    }
}
