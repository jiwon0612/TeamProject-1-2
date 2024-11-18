using UnityEngine;
using System;

public enum AudioType
{
    SFX,
    BGM
}

[CreateAssetMenu(menuName = "SO/SoundData")]
public class SoundDataSO : ScriptableObject
{
    public AudioType audioType;
    public AudioClip clip;
    public bool loop = false;
    public bool randomPitch = false;
    
    [Range(0,1)]
    public float randomPitchModifier = 0.1f;
    [Range(0, 1)] 
    public float volume = 1f;
    [Range(0.1f,3f)]
    public float basePitch = 1f;
}
