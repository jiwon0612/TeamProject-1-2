using System;
using UnityEngine;
using UnityEngine.Events;

public class Katana : MonoBehaviour, IPlayerComponent
{
    [SerializeField] private LayerMask _whatIsTarget;
    [SerializeField] private Material _sliceMaterial;
    [SerializeField] private PhysicMaterial slicePhysicMaterial;
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
    private Vector3 _normal;

    public void Initialize(Player player)
    {
        _player = player;
        _animaTrigger = _player.GetComp<AnimationEndTrigger>();
        _trail = GetComponentInChildren<TrailRenderer>();
        _collider = new Collider[maxColliderCount];

        _animaTrigger.OnAnimationPlaying.OnValueChanged += HandleEffectPlay;
        _trail.emitting = false;
        _isAttack = false;
        
    }

    private void HandleEffectPlay(bool prev, bool next)
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
        _normal = _normalPoint.position - transform.position;
        
        for (int i = 0; i < _collider.Length; i++)
        {
            if (_collider[i].TryGetComponent(out IHitable hitable))
            {
                Vector3 hitPoint = _collider[i].ClosestPointOnBounds(center.position);
                
                EffectPlayer hitEffect = PoolManager.Instance.Pop("HitEffect") as EffectPlayer;
                
                hitEffect.SetPositionAndPlay(hitPoint);
                Transform[] obj = hitable.Hit();
                float minDist = Vector3.Distance(hitPoint, obj[0].position);
                int minIndex = 0;
                for (int j = 1; j < obj.Length; j++)
                {
                    //Debug.Log(obj[j].name);
                    float dis = Vector3.Distance(hitPoint, obj[j].position);
                    if (dis < minDist)
                    {
                        minDist = dis;
                        minIndex = j;
                    }
                }

                try
                {
                    GameObject[] cutObjs = MeshCut.Cut(obj[minIndex].gameObject, hitPoint,_normal, _sliceMaterial);
                    MeshCollider mesCol = cutObjs[0].GetComponent<MeshCollider>();
                    mesCol.sharedMesh = cutObjs[0].GetComponent<MeshFilter>().mesh;
                    mesCol.material = slicePhysicMaterial;
                
                    cutObjs[1].transform.position = cutObjs[0].transform.position;
                    cutObjs[1].transform.rotation = cutObjs[0].transform.rotation;
                    cutObjs[1].transform.localScale = cutObjs[0].transform.localScale;
                    cutObjs[1].gameObject.layer = cutObjs[0].gameObject.layer;
                
                    cutObjs[1].gameObject.AddComponent<Rigidbody>();
                    //rigi.AddForce(_normal * 10, ForceMode.Impulse);
                    cutObjs[0].GetComponent<Rigidbody>().AddForce(-_normal * 5, ForceMode.Impulse);
                    MeshCollider meshColl = cutObjs[1].gameObject.AddComponent<MeshCollider>();
                    
                    meshColl.convex = true;
                    meshColl.material = slicePhysicMaterial;
                }
                catch (Exception e)
                {
                    Debug.Log("까비");
                }
                
                OnHitEvent?.Invoke();
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