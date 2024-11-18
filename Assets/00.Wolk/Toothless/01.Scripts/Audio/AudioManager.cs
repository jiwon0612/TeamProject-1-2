using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup _bgmGroup;
    [SerializeField] private AudioMixerGroup _sfxGroup;
    
    private void Awake()
    {
        Debug.Log(_bgmGroup.name);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(_sfxGroup.name, Mathf.Log10(volume) * 20);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat(_bgmGroup.name, Mathf.Log10(volume) * 20);
    }
}
