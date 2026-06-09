using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinny : MonoBehaviour
{
    public float donePercent, failPercent, direction = 1;
    float angle, timer;
    [SerializeField] Transform[] props; // 0 is the spinner gfx, 1 are the arrows
    [SerializeField] float rotsneeded = 1, failedRotsNeeded = 1, dirgo;

    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;

        Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float ngl = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(Mathf.Abs(ngl - angle) < 80){
            donePercent += Mathf.Abs(ngl - angle) * direction / (3.6f * rotsneeded);

            if(((ngl - angle) * direction) / ((ngl - angle) * direction) != direction)
            {
                failPercent += Mathf.Abs(ngl - angle) * direction / (3.6f * failedRotsNeeded);
            }
            else
            {
                failPercent -= Mathf.Abs(ngl - angle) * direction / (3.6f * failedRotsNeeded);
            }
        }
        angle = ngl;

        props[1].rotation = Quaternion.Euler(0, 0, Mathf.Cos(timer * 3) * 30);
        props[1].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, -Mathf.Sin(timer * 3));

        props[0].rotation = Quaternion.Euler(0, 0, angle);

        if(donePercent >= 100) // sucess
        {
            FindObjectOfType<World>().StartCoroutine(FindObjectOfType<World>().endGame(true));
            Destroy(gameObject);
        }

        if(failPercent >= 100) // fail!
        {
            FindObjectOfType<World>().StartCoroutine(FindObjectOfType<World>().endGame(false));
            Destroy(gameObject);
        }
    }
}