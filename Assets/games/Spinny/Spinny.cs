using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spinny : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Slider progressSlider;
    [SerializeField] Slider failSlider;

    [Header("Settings")]
    [SerializeField] float successNeeded = 100f;
    [SerializeField] float failNeeded = 100f;

    [SerializeField] float progressSpeed = 30f;
    [SerializeField] float failSpeed = 40f;
    [SerializeField] float failDecayPerSecond = 15f;

    [SerializeField] int direction = 1; // 1 = clockwise, -1 = counterclockwise

    [Header("Visuals")]
    [SerializeField] Transform spinnerGraphic;
    [SerializeField] Transform arrowGraphic;

    float lastAngle;
    float timer;

    void Start()
    {
        Vector2 dir = transform.position -
                      Camera.main.ScreenToWorldPoint(Input.mousePosition);

        lastAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        progressSlider.maxValue = successNeeded;
        failSlider.maxValue = failNeeded;

        progressSlider.value = 0;
        failSlider.value = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;

        UpdateSpinner();
        UpdateArrow();

        failSlider.value = Mathf.Max(
            0,
            failSlider.value - failDecayPerSecond * Time.deltaTime
        );

        CheckWinLose();
    }

    void UpdateSpinner()
    {
        Vector2 dir = transform.position -
                      Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float currentAngle =
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float delta =
            Mathf.DeltaAngle(lastAngle, currentAngle);

        spinnerGraphic.rotation =
            Quaternion.Euler(0, 0, currentAngle);

        if (Mathf.Abs(delta) > 0.1f)
        {
            bool correctDirection =
                Mathf.Sign(delta) == direction;

            if (correctDirection)
            {
                progressSlider.value +=
                    Mathf.Abs(delta) * progressSpeed * Time.deltaTime;
            }
            else
            {
                failSlider.value +=
                    Mathf.Abs(delta) * failSpeed * Time.deltaTime;
            }
        }

        lastAngle = currentAngle;
    }

    void UpdateArrow()
    {
        arrowGraphic.rotation =
            Quaternion.Euler(
                0,
                0,
                Mathf.Cos(timer * 3f) * 30f
            );

        SpriteRenderer sr =
            arrowGraphic.GetComponent<SpriteRenderer>();

        Color c = sr.color;
        c.a = (Mathf.Sin(timer * 3f) + 1f) / 2f;
        sr.color = c;
    }

    void CheckWinLose()
    {
        if (progressSlider.value >= successNeeded)
        {
            World world = FindAnyObjectByType<World>();

            world.StartCoroutine(world.EndGame(true));

            Destroy(gameObject);
        }

        if (failSlider.value >= failNeeded)
        {
            World world = FindAnyObjectByType<World>();

            world.StartCoroutine(world.EndGame(false));

            Destroy(gameObject);
        }
    }
}