using System.Collections;
using UnityEngine;

public class FishingSystem : MonoBehaviour
{
    public enum FishingState { Idle, Casting, WaitingForBite, Minigame, Catching }
    public FishingState currentState = FishingState.Idle;

    [Header("Settings")]
    public float minBiteTime = 2f;
    public float maxBiteTime = 6f;

    [Header("References")]
    public GameObject bobberPrefab;
    public Transform castPoint;
    public FishingMinigame minigameUI;

    private GameObject currentBobber;

    private void Start()
    {
        TriggerBite();
    }
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        HandleFishingInput();
    //    }
    //}

    //void HandleFishingInput()
    //{
    //    switch (currentState)
    //    {
    //        //case FishingState.Idle:
    //        //    StartCoroutine(CastLine());
    //        //    break;
    //        case FishingState.WaitingForBite:
    //            // Player reeled in too early
    //            ResetFishing();
    //            break;
    //        case FishingState.Minigame:
    //            // Minigame controls take over here
    //            break;
    //    }
    //}

    //IEnumerator CastLine()
    //{
    //    currentState = FishingState.Casting;
    //    currentBobber = Instantiate(bobberPrefab, castPoint.position, Quaternion.identity);
    //
    //    // Simulating simple physics toss
    //    Vector2 throwVelocity = new Vector2(5f, 3f);
    //    currentBobber.GetComponent<Rigidbody2D>().velocity = throwVelocity;
    //
    //    yield return new WaitForSeconds(1.5f); // Wait for splash
    //
    //    StartCoroutine(WaitForBite());
    //}
    //
    //IEnumerator WaitForBite()
    //{
    //    currentState = FishingState.WaitingForBite;
    //    float waitTime = Random.Range(minBiteTime, maxBiteTime);
    //    yield return new WaitForSeconds(waitTime);
    //
    //    // Trigger Bite Signal (e.g., Bobber splash animation/sound)
    //    TriggerBite();
    //}

    void TriggerBite()
    {
        currentState = FishingState.Minigame;
        minigameUI.StartMinigame(this);
    }

    public void CompleteMinigame(bool success)
    {
        //ResetFishing();
        if (success)
        {
            Debug.Log("Fish Caught!");
            FindObjectOfType<World>().StartCoroutine(FindObjectOfType<World>().endGame(true));
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("The fish got away!");
            FindObjectOfType<World>().StartCoroutine(FindObjectOfType<World>().endGame(false));
            Destroy(gameObject);
        }
    }

    //void ResetFishing()
    //{
    //    if (currentBobber != null) Destroy(currentBobber);
    //    currentState = FishingState.Idle;
    //}
}

