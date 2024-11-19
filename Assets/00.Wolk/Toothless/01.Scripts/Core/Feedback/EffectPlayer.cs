using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectPlayer : MonoBehaviour, IPoolable
{
    [SerializeField] private string poolName = "EffectPlayer";
    public string PoolName => poolName;
    public GameObject ObjectPrefab => gameObject;

    private ParticleSystem _particle;
    private float _duration;
    private WaitForSeconds _particleDuration;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
        _duration = _particle.main.duration;
        _particleDuration = new WaitForSeconds(_duration);
    }

    public void SetPositionAndPlay(Vector3 position)
    {
        transform.position = position;
        _particle.Play();
        StartCoroutine(DelayAndGotoPool());
    }

    private IEnumerator DelayAndGotoPool()
    {
        yield return _particleDuration;
        PoolManager.Instance.Push(this);
    }

    public void ResetItem()
    {
        _particle.Stop();
        _particle.Simulate(0); 
    }
}
