using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float deadTime;

    public bool IsShooting { get; set; }
    
    private Rigidbody _rigid;
    private float timer;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();    
    }


    public void InitAndFire(Vector3 dir, Quaternion rotation)
    {
        _rigid.useGravity = false;
        IsShooting = true;
        timer = 0;  
        transform.rotation = rotation;
        _rigid.AddForce(dir * force, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (timer >= deadTime)
            Dead();
        
        if (IsShooting)
            timer += Time.deltaTime;
    }

    private void Dead()
    {
        IsShooting = false;
        _rigid.useGravity = true;
        gameObject.SetActive(false);
    }
}
