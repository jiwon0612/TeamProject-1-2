using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Katana : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsTarget;
    [SerializeField] private Material _sliceMaterial;

    [SerializeField] private Transform _originPos;
    [SerializeField] private Transform _bradePos;

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

                Vector3 normal = new Vector3(point.normal.y, point.normal.x, point.normal.z);
                
                Debug.Log(normal);
                
                ObjectCut.Slicer(collision.gameObject,new Vector3(-1,0,0), hitPoint, _sliceMaterial);

                Debug.Log("자름");
            }
        }
    }
}