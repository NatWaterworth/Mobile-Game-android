using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    [Range(0.3f,2)]
    public float pitch;
    [Range(0, 1)]
    public float volume;

    [HideInInspector]
    public AudioSource source;

    public bool loop;
}
