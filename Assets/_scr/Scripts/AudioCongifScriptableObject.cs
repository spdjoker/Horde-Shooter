using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Config", menuName = "Guns/Audio Config", order = 5)]

public class AudioCongifScriptableObject : ScriptableObject
{
    [Range(0, 1f)]
    public float Volume = 1f;
    public AudioClip[] FireClips;
    public AudioClip EmptyClip;
    public AudioClip ReloadClip;


    public void PlayOutOfAmmoClip(AudioSource AudioSource)
    {
        if (EmptyClip != null)
        {
            AudioSource.PlayOneShot(EmptyClip, Volume);
        }
    }

    public void PlayReloadClip(AudioSource AudioSource)
    {
        if (ReloadClip != null) 
        {
            AudioSource.PlayOneShot(ReloadClip, Volume)
        }
    }
}
