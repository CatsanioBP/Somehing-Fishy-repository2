using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    float timer;
    public int state; // 0 is chillin, 1 is fick fuckup
    Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        anim.SetInteger("State", state);

        if(state == 0)
        {
            timer+=Time.deltaTime;

            anim.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(timer * 0.6f) * 10);
        }
        else
        {
            timer = 0;
        }
    }
}