using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Katana : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsTarget;
    [SerializeField] private Material _sliceMaterial;

    [SerializeField] private Transform _normalPoint;

    private bool _fi = false;

    private void OnCollisionEnter(Collision collision)
    {
        LayerMask colisionMask = 1 << collision.gameObject.layer;
        if ((_whatIsTarget & colisionMask) != 0)
        {
            if (_fi == false)
            {
                _fi = true;

                ContactPoint point = collision.contacts[0];
                Vector3 hitPoint = point.point;
                
                Debug.Log(point.normal);

                Vector3 normal = (_normalPoint.position - transform.position).normalized;
                
                Debug.Log(normal);
                
                GameObject[] objects = ObjectCut.Slicer(collision.gameObject,normal, hitPoint, _sliceMaterial);

                foreach (GameObject obj in objects)
                {
                    obj.AddComponent<Rigidbody>();
                    var objCol = obj.AddComponent<MeshCollider>();
                    objCol.convex = true;
                }
            }
        }
    }
}