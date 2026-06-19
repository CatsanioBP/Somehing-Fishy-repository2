using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] Transform gfx;
    public float bob, waitTime = -16,bobberHelp = 0;
    public bool onWater;
    [SerializeField] LayerMask myself;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip actionSoundClip,lurring;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.2f, ~myself);

        onWater = hit;

        if (onWater)
        {

            if (waitTime <= 0)
            {
                if (waitTime > -15)
                {
                    bob = 0.75f;
                }

                waitTime = Random.Range(0.9f, 10.2f - bobberHelp);
            }

            waitTime -= Time.deltaTime;

            if (bob > 0)
            {
                bob -= Time.deltaTime;
            }

            gfx.localPosition = new Vector2(0, -Mathf.Sin(bob * Mathf.PI / 0.75f) * 0.6f);
        }
        else
        {
            waitTime = -16;
            gfx.localPosition = new Vector2(0, 0);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Waves"))
        {
            audioSource.PlayOneShot(actionSoundClip);
            audioSource.PlayOneShot(lurring);
        }
    }
}
