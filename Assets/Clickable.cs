using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    [SerializeField] GameObject inventory,description;
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
    private void OnMouseDown()
    {
        World world = Object.FindAnyObjectByType<World>();
        if (world.inGame == false && inventory.activeInHierarchy == false)
        {
            inventory.SetActive(!inventory.activeSelf);
            description.SetActive(false);
            world.inGame = true;
        }
        else if(inventory.activeInHierarchy == true)
        {
            inventory.SetActive(!inventory.activeSelf);
            description.SetActive(false);
            world.inGame = false;
        }

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