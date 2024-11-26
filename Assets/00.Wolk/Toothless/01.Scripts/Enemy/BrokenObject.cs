    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BrokenObject : MonoBehaviour, IHitable
{
    private void Start()
    {
        
    }

    public Transform[] Hit()
    {
        Transform[] hit = new Transform[1];
        hit[0] = transform;

        Destroy(this);
        
        return hit;
    }
}
