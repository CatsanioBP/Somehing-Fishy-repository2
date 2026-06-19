using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waves : MonoBehaviour
{
    [SerializeField] float spinSpeed, verticalSpin, horizontalSpin;
    [SerializeField] Transform waveSprt;
    private AudioSource waveAudioSource;

    void Awake()
    {
        // Automatically finds the AudioSource attached to this same GameObject
        waveAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        waveSprt.localPosition = new Vector2(Mathf.Cos(Time.time * spinSpeed) * horizontalSpin, Mathf.Sin(Time.time * spinSpeed) * verticalSpin);
    }
}