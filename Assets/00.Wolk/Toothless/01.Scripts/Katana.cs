using UnityEngine;
using UnityEngine.Events;

public class Katana : MonoBehaviour, IPlayerComponent
{
    [SerializeField] private LayerMask _whatIsTarget;
    [SerializeField] private Material _sliceMaterial;

    [SerializeField] private Transform _normalPoint;

    public UnityEvent OnHitEvent;

    private TrailRenderer _trail;
    private AnimationEndTrigger _animaTrigger;
    private Player _player;

    private bool _fi = false;

    public void Initialize(Player player)
    {
        _player = player;
        _animaTrigger = _player.GetComp<AnimationEndTrigger>();
        _trail = GetComponentInChildren<TrailRenderer>();

        _animaTrigger.OnAnimationPlaying.OnValueChanged += HandleEffectPaly;
        _trail.emitting = false;
    }

    private void HandleEffectPaly(bool prev, bool next)
    {
        _trail.emitting = next;
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

                Debug.Log(point.normal);

                Vector3 normal = (_normalPoint.position - transform.position);

                Debug.Log(normal);

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

    // public void OnHitEvent()
    // {
    //     
    // }
    
    private void OnCollisionExit(Collision other)
    {
        if (_fi == true)
            _fi = false;
    }
}