using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour, IPoolable
{
    [SerializeField] private AudioMixerGroup _sfxGroup, _bgmGroup;
    [SerializeField] private string _poolName = "SoundPlayer";
    private AudioSource _audioSource;
    
    public string PoolName => _poolName;
    public GameObject ObjectPrefab => gameObject;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundDataSO soundData)
    {
        if (soundData.audioType == AudioType.SFX)
        {
            _audioSource.outputAudioMixerGroup = _sfxGroup;
        }
        else if (soundData.audioType == AudioType.BGM)
        {
            _audioSource.outputAudioMixerGroup = _bgmGroup;
        }
        
        _audioSource.volume = soundData.volume;
        _audioSource.pitch = soundData.basePitch;

        if (soundData.randomPitch)
        {
            _audioSource.pitch += Random.Range(-soundData.randomPitchModifier, soundData.randomPitchModifier);
        }

        _audioSource.clip = soundData.clip;
        _audioSource.loop = soundData.loop;

        if (!soundData.loop)
        {
            float time = _audioSource.clip.length + 0.2f;
            DOVirtual.DelayedCall(time, () => PoolManager.Instance.Push(this));
        }
        
        _audioSource.Play();
    }

    public void StopAndGoToPool()
    {
        _audioSource.Stop();
        PoolManager.Instance.Push(this);
    }

    public void ResetItem()
    {
        
    }
}
