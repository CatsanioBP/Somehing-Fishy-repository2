using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject particle;
    [SerializeField] Transform point;
    AudioSource noise;
    BoxCollider2D box;
    bool good;

    void Start()
    {
        anim = GetComponent<Animator>();
        if(!point)
            point = transform;
        noise = GetComponent<AudioSource>();
        box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (box.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            if(!good){
                good = false;
                anim.SetTrigger("moved");

            if(particle)
                Instantiate(particle, point.position, Quaternion.identity);

            if(noise)
                noise.Play();
            }
        }else   
            good = true;
    }

    void OnMouseEnter()
    {
        anim.SetTrigger("moved");

        if(particle)
            Instantiate(particle, point.position, Quaternion.identity);

        if(noise)
            noise.Play();
    }
}