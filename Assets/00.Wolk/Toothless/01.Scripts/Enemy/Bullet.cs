using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField] private float force;
    [SerializeField] private float deadTime;
    [SerializeField] private string poolName;
    [SerializeField] private int damage;

    public bool IsShooting { get; set; }
    
    private Rigidbody _rigid;
    private float timer;
    
    public string PoolName => poolName;
    public GameObject ObjectPrefab => gameObject;

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
        PoolManager.Instance.Push(this);
    }
    
    public void ResetItem()
    {
        _rigid.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        IsShooting = false;
        _rigid.useGravity = false;
        timer = 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(damage);
        }
        
        Dead();
    }
}
