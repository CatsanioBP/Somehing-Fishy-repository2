using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MyExposedMP", Mathf.Log10(level)*20f);
    }
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("ExposedMusicP", Mathf.Log10(level) * 20f);
    }
    public void SetSFVolume(float level)
    {
        audioMixer.SetFloat("ExposedSEffectsP", Mathf.Log10(level) * 20f);
    }
}
