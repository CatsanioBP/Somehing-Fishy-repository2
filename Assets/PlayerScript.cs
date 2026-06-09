using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool didGameStarted;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         didGameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && didGameStarted == false)
        {
             didGameStarted = true;
            //Some minigame function that choosen with Rand function

        }
    }
}
