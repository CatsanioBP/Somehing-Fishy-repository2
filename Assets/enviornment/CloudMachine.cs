using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudMachine : MonoBehaviour
{
    [SerializeField] GameObject[] clouds;
    float spauner;
    void Start()
    {
        for(int i = 0; i < 15; i++)
        {
            GameObject cloud = Instantiate(clouds[Random.Range(0, clouds.Length)], new Vector2(transform.position.x + Random.Range(0, 60), transform.position.y + Random.Range(-4, 2)), Quaternion.identity);
            cloud.GetComponent<Parallax>().howHard = Random.Range(0.85f, 0.95f);
        }
    }

    void Update()
    {
        if(spauner <= 0){
            GameObject cloud = Instantiate(clouds[Random.Range(0, clouds.Length)], new Vector2(transform.position.x + Random.Range(0, 60), transform.position.y + Random.Range(-4, 2)), Quaternion.identity);
            cloud.GetComponent<Parallax>().howHard = Random.Range(0.9f, 0.99f);
            spauner = 1;
        }

        spauner-=Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("CloudDeleter"))
        {
            Destroy(collision.gameObject);
        }
    }
}