using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [field : SerializeField] public InputReader InputCompo { get; private set; }

    public Dictionary<Type, IPlayerComponent> _components;

    private void Awake()
    {
        _components = new Dictionary<Type, IPlayerComponent>();

        GetComponentsInChildren<IPlayerComponent>().ToList().ForEach(x => _components.Add(x.GetType(), x));

        _components.Add(InputCompo.GetType(), InputCompo);

        _components.Values.ToList().ForEach(x => x.Initialize(this));
    }

    public T GetComp<T>() where T : class
    {
        Type type = typeof(T);
        if (_components.TryGetValue(type, out IPlayerComponent compo))
        {
            return compo as T;
        }
        return default;
    }

}