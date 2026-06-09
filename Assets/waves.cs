using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waves : MonoBehaviour
{
    [SerializeField] float spinSpeed, verticalSpin, horizontalSpin;
    [SerializeField] Transform waveSprt;

    void Start()
    {
        
    }

    void Update()
    {
        waveSprt.localPosition = new Vector2(Mathf.Cos(Time.time * spinSpeed) * horizontalSpin, Mathf.Sin(Time.time * spinSpeed) * verticalSpin);
    }
}