using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Katana : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsTarget;
    [SerializeField] private Material _sliceMaterial;

    private bool _fi = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("시발");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Qyd");
        LayerMask colisionMask = 1 << collision.gameObject.layer;

        if ((_whatIsTarget & colisionMask) != 0)
        {
            if (_fi == false)
            {
                _fi = true;

                ContactPoint point = collision.contacts[0];
                Vector3 hitPoint = point.point;
                
                Vector3 dir = hitPoint - transform.position;
                
                Vector3 normalPoint = point.normal;
                
                ObjectCut.CutObject(collision.gameObject, new Vector3(-1,0,0), hitPoint, _sliceMaterial);
                
                Debug.Log("자름");
            }
        }
    }
}
