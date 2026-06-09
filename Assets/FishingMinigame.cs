using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform fishIcon;
    public RectTransform playerBar;
    public Slider progressBar;

    [Header("Balancing Settings")]
    public float fishMoveSpeed = 3f;
    public float barMoveSpeed = 400f;
    public float gravity = 300f;
    public float progressFillSpeed = 0.15f;
    public float progressDrainSpeed = 0.1f;

    private FishingSystem fishingSystem;
    private float fishTargetY;
    private float fishTimer;
    private float barVelocity;
    private float currentProgress = 0.3f;
    private bool isGameActive = false;

    public void StartMinigame(FishingSystem system)
    {
        fishingSystem = system;
        gameObject.SetActive(true);
        currentProgress = 0.3f;
        isGameActive = true;
    }

    void Update()
    {
        if (!isGameActive) return;

        MoveFishRandomly();
        MovePlayerBar();
        CheckOverlap();
    }

    void MoveFishRandomly()
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer <= 0)
        {
            // Pick a random vertical spot within UI boundaries
            fishTargetY = Random.Range(-450f, 250f);
            fishTimer = Random.Range(0.5f, 2f);
        }

        Vector3 pos = fishIcon.localPosition;
        pos.y = Mathf.MoveTowards(pos.y, fishTargetY, fishMoveSpeed * Time.deltaTime * 100);
        fishIcon.localPosition = pos;
    }

    void MovePlayerBar()
    {
        // Hold Space or Click to ascend, release to descend
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            barVelocity += barMoveSpeed * Time.deltaTime;
        }
        else
        {
            barVelocity -= gravity * Time.deltaTime;
        }

        Vector3 pos = playerBar.localPosition;
        pos.y += barVelocity * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, -450f, 250f); // Keep inside borders

        if (pos.y == -450f || pos.y == 250f) barVelocity = 0; // Reset speed at edges
        playerBar.localPosition = pos;
    }

    void CheckOverlap()
    {
        // Simple distance overlap validation between the fish icon and catch zone
        float distance = Mathf.Abs(fishIcon.localPosition.y - playerBar.localPosition.y);

        if (distance < 50f) // Threshold size of your green bar
        {
            currentProgress += progressFillSpeed * Time.deltaTime;
        }
        else
        {
            currentProgress -= progressDrainSpeed * Time.deltaTime;
        }

        currentProgress = Mathf.Clamp01(currentProgress);
        progressBar.value = currentProgress;

        if (currentProgress >= 1f) EndMinigame(true);
        if (currentProgress <= 0f) EndMinigame(false);
    }

    void EndMinigame(bool success)
    {
        isGameActive = false;
        gameObject.SetActive(false);
        fishingSystem.CompleteMinigame(success);
    }
}

