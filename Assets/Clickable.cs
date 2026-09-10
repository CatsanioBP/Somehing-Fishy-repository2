using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    [SerializeField] GameObject inventory,description,baitMenu,baitDesc;
    Animator anim;
    [SerializeField] GameObject particle;
    [SerializeField] Transform point;
    [SerializeField] bool isBucket;
    AudioSource noise;
    BoxCollider2D box;
    bool good;

    void Start()
    {
        if (isBucket == true)
        {
            anim = GetComponent<Animator>();
            if (!point)
                point = transform;
            noise = GetComponent<AudioSource>();
        }
        box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isBucket == true)
        {
            if (box.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                if (!good)
                {
                    good = false;
                    anim.SetTrigger("moved");

                    if (particle)
                        Instantiate(particle, point.position, Quaternion.identity);

                    if (noise)
                        noise.Play();
                }
            }
            else
                good = true;
        }
    }
    private void OnMouseDown()
    {
        World world = Object.FindAnyObjectByType<World>();
        if (gameObject.CompareTag("Inventory"))
        {
            if (world.inGame == false && inventory.activeInHierarchy == false)
            {
                world.menued = true;
                inventory.SetActive(!inventory.activeSelf);
                description.SetActive(false);
                world.inGame = true;
            }
            else if (inventory.activeInHierarchy == true)
            {
               
                inventory.SetActive(!inventory.activeSelf);
                description.SetActive(false);
                world.inGame = false;
                world.menued = false;
            }
        }
        if (gameObject.CompareTag("BaitMenu"))
        {
           
            if (world.inGame == false && baitMenu.activeInHierarchy == false)
            {
                world.menued = true;
                baitMenu.SetActive(!baitMenu.activeSelf);
                baitDesc.SetActive(false);
                world.inGame = true;
            }

            else if (baitMenu.activeInHierarchy == true)
            {
                world.menued = false;
                baitMenu.SetActive(!baitMenu.activeSelf);
                baitDesc.SetActive(false);
                world.inGame = false;
            }
        }
    }
}