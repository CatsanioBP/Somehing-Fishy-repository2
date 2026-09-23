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
    [SerializeField] float failNeeded = 0f;

    [SerializeField] float progressSpeed = 5f;
    [SerializeField] float failSpeed = 10f;
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

        progressSlider.value = 30;
        failSlider.value = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;

        UpdateSpinner();
        UpdateArrow();

        progressSlider.value -= failSpeed * Time.deltaTime;
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
           // Debug.Log(direction);
            //Debug.Log(correctDirection);

            if (correctDirection)
            {
                progressSlider.value +=
                    Mathf.Abs(delta) * progressSpeed * Time.deltaTime;
            }
           //else
           //{
           //    progressSlider.value -=
           //        Mathf.Abs(delta) * failSpeed;
           //}
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
        if (progressSlider.value >= successNeeded - 1)
        {
            World world = FindAnyObjectByType<World>();

            world.StartCoroutine(world.EndGame(true));

            Destroy(gameObject);
        }

        if (progressSlider.value <= 0)
        {
            World world = FindAnyObjectByType<World>();

            world.StartCoroutine(world.EndGame(false));

            Destroy(gameObject);
        }
    }
}