using System;
using UnityEngine;
using UnityEngine.Events;

public class Katana : MonoBehaviour, IPlayerComponent
{
    [SerializeField] private LayerMask _whatIsTarget;
    [SerializeField] private Material _sliceMaterial;
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private Transform center;
    [SerializeField] private int maxColliderCount = 15;
    [SerializeField] private Transform _normalPoint;

    public UnityEvent OnHitEvent;

    private TrailRenderer _trail;
    private AnimationEndTrigger _animaTrigger;
    private Player _player;
    private Collider[] _collider;

    private bool _fi = false;
    private bool _isAttack;

    public void Initialize(Player player)
    {
        _player = player;
        _animaTrigger = _player.GetComp<AnimationEndTrigger>();
        _trail = GetComponentInChildren<TrailRenderer>();
        _collider = new Collider[maxColliderCount];

        _animaTrigger.OnAnimationPlaying.OnValueChanged += HandleEffectPaly;
        _trail.emitting = false;
        _isAttack = false;
    }

    private void HandleEffectPaly(bool prev, bool next)
    {
        _trail.emitting = next;
    }

    private void Update()
    {
        if (_isAttack)
        {
            _collider = Physics.OverlapBox(center.position, boxSize, Quaternion.identity, _whatIsTarget);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        LayerMask colisionMask = 1 << collision.gameObject.layer;
        if ((_whatIsTarget & colisionMask) != 0)
        {
            if (_fi == false && _animaTrigger.OnAnimationPlaying.Value)
            {
                _fi = true;

                OnHitEvent?.Invoke();

                ContactPoint point = collision.contacts[0];
                Vector3 hitPoint = point.point;

                Vector3 normal = (_normalPoint.position - transform.position);

                //GameObject[] objects = ObjectCut.Slicer(collision.gameObject,normal, hitPoint, _sliceMaterial);
                GameObject[] objects = MeshCut.Cut(collision.gameObject, hitPoint, normal, _sliceMaterial);

                objects[1].transform.position = objects[0].transform.position;
                objects[1].transform.rotation = objects[0].transform.rotation;
                objects[1].transform.localScale = objects[0].transform.localScale;
                objects[1].gameObject.layer = objects[0].gameObject.layer;

                objects[1].gameObject.AddComponent<Rigidbody>();
                var obj = objects[1].gameObject.AddComponent<MeshCollider>();
                obj.convex = true;

                var obj2 = objects[0].GetComponent<MeshCollider>();
                obj2.sharedMesh = objects[0].GetComponent<MeshFilter>().mesh;
            }
        }
    }

    public void StartAttack()
    {
        _isAttack = true;
        _collider = Physics.OverlapBox(center.position, boxSize, Quaternion.identity, _whatIsTarget);

        for (int i = 0; i < _collider.Length; i++)
        {
            if (_collider[i].TryGetComponent(out IHitable hitable))
            {
                EffectPlayer hitEffect = PoolManager.Instance.Pop("HitEffect") as EffectPlayer;
                
                hitEffect.SetPositionAndPlay(_collider[i].ClosestPointOnBounds(center.position));
                hitable.Hit();
                OnHitEvent?.Invoke();
                Debug.Log(_collider[i].name);
            }
        }
    }

    public void EndAttack()
    {
        _isAttack = false;
        
    }

    private void OnDrawGizmos()
    {
        if (center != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = center.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, boxSize);
            Gizmos.color = Color.white;
        }
    }
}