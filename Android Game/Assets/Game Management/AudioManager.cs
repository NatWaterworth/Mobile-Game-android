using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        #region Singleton

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        DontDestroyOnLoad(gameObject);

        foreach(Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = 0;
            sound.source.pitch = sound.pitch;

            sound.source.loop = sound.loop;
        }
    }

    public void Play(string name, bool randomPitch)
    {
        Debug.Log("Playing:" + name);
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s==null)
        {
            Debug.LogWarning(this + " cannot find sound with name: " + name +"!");
            return;
        }
        if (randomPitch)
            s.source.pitch = UnityEngine.Random.Range(0.65f,1.35f) * s.pitch;

        s.source.Play();
        StartCoroutine(StartFade(s.source, 1f, s.volume));
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning(this + " cannot find sound with name: " + name + "!");
            return;
        }

        StartCoroutine(StartFade(s.source, 1f, 0));
    }

    IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        if (audioSource.volume == 0)
            audioSource.Stop();
        yield return null;
    }
}
