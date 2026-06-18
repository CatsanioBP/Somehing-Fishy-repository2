using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cloud : MonoBehaviour
{
    public float speed;
    SpriteRenderer sprite;
    float lifetime;

    void Start()
    {

        sprite = GetComponent<SpriteRenderer>();
        speed = Random.Range(0.05f, 0.5f);
        lifetime = Random.Range(10f, 20f);

        transform.localScale = new Vector3(Random.Range(0.8f, 1.5f), Random.Range(0.8f, 1.5f), 1);

        StartCoroutine(fadeIn());
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);

        if(lifetime < 5)
        {
            // fade out
            sprite.color = new Color(1, 1, 1, lifetime / 5);
        }
    }

    IEnumerator fadeIn()
    {
        float timr = 0f;

        while(timr < 3)
        {
            timr += Time.deltaTime;
            sprite.color = new Color(1, 1, 1, timr / 3);
            yield return null;
        }

        sprite.color = new Color(1, 1, 1, 1);
    }
}