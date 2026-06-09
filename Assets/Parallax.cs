using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float howHard; // 0 is it doesnt move at all, 1 is is just follows the camera
    Transform cam;
    float camPrev;

    void Start()
    {
        cam = Camera.main.transform;
    }
    void Update()
    {
        transform.position = new Vector2(transform.position.x + (cam.position.x - camPrev) * howHard, transform.position.y);
        camPrev = cam.transform.position.x;
    }
}