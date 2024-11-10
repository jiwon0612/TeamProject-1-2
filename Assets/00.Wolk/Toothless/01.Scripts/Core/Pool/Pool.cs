using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private Stack<IPoolable> _pool;
    private Transform _parentTrm;
    private IPoolable _poolable;
    private GameObject _prefab;

    public Pool(IPoolable poolable, Transform parentTrm, int count)
    {
        _pool = new Stack<IPoolable>(count);
        _parentTrm = parentTrm;
        _poolable = poolable;
        _prefab = poolable.ObjectPrefab;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = GameObject.Instantiate(_prefab, parentTrm);
            obj.SetActive(false);
            obj.name = _poolable.PoolName;
            IPoolable item = obj.GetComponent<IPoolable>();
            _pool.Push(item);
        }
    }

    public IPoolable Pop()
    {
        IPoolable item = null;

        if (_pool.Count == 0)
        {
            GameObject obj = GameObject.Instantiate(_prefab, _parentTrm);
            obj.name = _poolable.PoolName;
            item = obj.GetComponent<IPoolable>();
        }
        else
        {
            item = _pool.Pop();
            item.ObjectPrefab.SetActive(true);
        }
        return item;
    }

    public void Push(IPoolable item)
    {
        item.ObjectPrefab.SetActive(false);
        _pool.Push(item);
    }
}
