using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class World : MonoBehaviour
{
    [Header("Game State")]
    public bool inGame;
    public bool canFish = true;
    public bool reeling;
    public bool CanGo = true;
    public bool isBaitUsed;
    [HideInInspector] public bool cursorHover, throwing, menued;
    public static int dayOn = 1;

    [Header("Fishing Stats")]
    public float chanceFromBait = 0;
    public float sizeHelper = 0;
    public int upgradeNum = 0;

    [Header("Fishing Line Settings")]
    [Tooltip("How many points the line has. Higher = smoother curve.")]
    public int lineSegments = 30;
    [Tooltip("How far down the line hangs due to gravity.")]
    public float lineSag = 2f;

    [Header("UI References")]
    public GameObject congratsMenu;
    public GameObject congratsReturn;
    public GameObject enceclopedia;
    [SerializeField] private TMP_Text congratsText;
    [SerializeField] private Image congratsImage;
    [SerializeField] private TextMeshProUGUI dayDisplayer;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private TMP_Text errorMessage;

    [Header("Object References")]
    public GameObject[] fish; // 0 is polderfish
    public Transform[] props; // 0: bobber, 1: fish bucket, 2: boy's rod point, 3: encyclopedia
    public Transform[] rodpoints; // 0: regular, 1: reeling, 2: throwing
    public GameObject[] miniGames; // 0: spinny, 1: pully
    public Inventory inventoryHolder;

    [Header("Fish Pools")]
    public FishSCRobject[] comFishesPool;
    public FishSCRobject[] uncFishesPool;
    public FishSCRobject[] rarFishesPool;
    public FishSCRobject[] specFishesPool;
    private FishSCRobject currentFihStats;

    [Header("Audio & Animation")]
    //public string[] listOfAdvices;
    [SerializeField] private Animator[] anims; // 0: boy, 1: bucket, 2: sun, 3: FadeOut/FadeIn
    [SerializeField] private Animator erorrAnim;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip actionBucket, swing, pushSound, releaseFSound, fishFall;

    private bool rewardClaimed = true;
    private bool dialogueSkipped = false;
    private Camera mainCam;
    private Vector3 defaultCamPos;
    private LineRenderer lr;
    [SerializeField] private LayerMask ignoreClickLayer;

    void Start()
    {
        rewardClaimed = true;
        isBaitUsed = true;
        lr = GetComponent<LineRenderer>();
        props[0].gameObject.SetActive(false);

        mainCam = Camera.main; // Cache camera for better performance
        defaultCamPos = mainCam.transform.position;
    }

    void Update()
    {
        HandleInput();
        HandleLineAndCamera();
    }

    private void HandleInput()
    {
        // 1. Guard Clause: If we can't fish, or menus are open, exit immediately.
        if (!canFish || inGame || menued)
        {
            return;
        }

        // 2. Prevent clicking through UI buttons/sliders
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // 3. Calculate mouse position and Raycast exactly ONCE for the whole method
        Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        bool hittingIgnoredObject = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, ignoreClickLayer).collider != null;

        // --- Start Throwing Windup ---
        if (Input.GetButtonDown("Fire1") && !hittingIgnoredObject)
        {
            anims[0].SetInteger("state", 1);
            throwing = true;
        }

        // --- Release Throw ---
        if (Input.GetButtonUp("Fire1") && throwing)
        {
            // If the player can't cast yet, or released the mouse over a shop, cancel the throw
            if (!rewardClaimed || hittingIgnoredObject)
            {
                anims[0].SetInteger("state", 0);
                throwing = false;
                return;
            }

            // Successful throw (Mouse is far enough right)
            if (mousePos.x > props[2].position.x + 1)
            {
                CanGo = false;
                canFish = false;
                throwing = false; // Reset throwing state!

                // Start the coroutine and pass the target mouse position
                StartCoroutine(CastLine(mousePos));
            }
            else // Cancelled throw (Didn't aim far enough)
            {
                anims[0].SetInteger("state", 0);
                throwing = false;
            }
        }
    }

    private void HandleLineAndCamera()
    {
        if (!canFish)
        {
            // Draw the curved fishing line
            lr.enabled = true;
            DrawCurvedLine();

            // Follow bobber with camera
            float clampedX = Mathf.Clamp(props[0].position.x, 0, 80);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, new Vector3(clampedX, defaultCamPos.y, -10), Time.deltaTime * 5);

            // Reel in / Catch
            if (!inGame && Input.GetButtonDown("Fire1") && !reeling)
            {
                CanGo = false;
                if (props[0].GetComponent<Bobber>().bob > 0)
                {
                    DetermineFishCaught();
                    StartCoroutine(StartGame());
                }
                else
                {
                    StartCoroutine(EndGame(false));
                }
            }
        }
        else
        {
            lr.enabled = false;
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, defaultCamPos, Time.deltaTime * 5);
        }
    }

    // --- Curved Line Logic ---

    private void DrawCurvedLine()
    {
        // Tell the LineRenderer how many points it will have
        lr.positionCount = lineSegments;

        Vector3 bobberPos = props[0].position;
        Vector3 rodPos = props[2].position;

        // Calculate the middle point between the rod and the bobber
        Vector3 midPoint = (rodPos + bobberPos) / 2f;

        // If we are reeling a fish in, the line is tight (0 sag). Otherwise, it sags.
        float currentSag = reeling ? 0f : lineSag;

        // Pull the middle point down to simulate gravity
        midPoint.y -= currentSag;

        // Draw the curve using the calculated points
        for (int i = 0; i < lineSegments; i++)
        {
            // t goes from 0.0 to 1.0 depending on the current segment
            float t = i / (float)(lineSegments - 1);

            // Calculate the exact curve position for this segment
            Vector3 curvePos = CalculateQuadraticBezierPoint(t, bobberPos, midPoint, rodPos);

            lr.SetPosition(i, curvePos);
        }
    }

    // The math formula for a Quadratic Bezier Curve
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; // (1-t)^2 * P0
        p += 2 * u * t * p1; // 2*(1-t)*t * P1
        p += tt * p2;        // t^2 * P2

        return p;
    }

    // --- Fish Catch Logic ---

    private void DetermineFishCaught()
    {
        float distanceBoyBobber = Vector2.Distance(props[0].position, props[2].position);
        float poolRoll = Random.Range(0, 50) + chanceFromBait + distanceBoyBobber;
        chanceFromBait = 0;

        if (poolRoll < 0)
        {
            int randFish = Random.Range(0, 1000);
            int index = (randFish < 333) ? 0 : (randFish < 666) ? 1 : (randFish < 999) ? 2 : 3;
            SetupFishStats(specFishesPool[index], upgradeNum);
        }
        else if (poolRoll <= 50)
        {
            SetupFishStats(comFishesPool[Random.Range(0, comFishesPool.Length)], sizeHelper);
        }
        else if (poolRoll <= 85)
        {
            SetupFishStats(uncFishesPool[Random.Range(0, uncFishesPool.Length)]);
        }
        else // > 85
        {
            SetupFishStats(rarFishesPool[Random.Range(0, rarFishesPool.Length)]);
        }
    }

    private void SetupFishStats(FishSCRobject baseFish, float extraSize = 0)
    {
        currentFihStats = baseFish.Clone();

        float currentFishSize = Random.Range(currentFihStats.minFishSize, currentFihStats.minFishSize + 2f + extraSize);
        float roundedNum = Mathf.Round(currentFishSize * 10f) / 10f;
        int finalPrice = Mathf.RoundToInt(currentFihStats.fishPrice * (roundedNum / currentFihStats.minFishSize));

        currentFihStats.minFishSize = roundedNum;
        currentFihStats.fishPrice = finalPrice;
    }

    // --- Coroutines ---

    public IEnumerator StartGame()
    {
        audioSource.PlayOneShot(pushSound, 0.2f);
        inGame = true;
        int randomMinigame = Random.Range(0, 2);
        Instantiate(miniGames[randomMinigame], new Vector3(mainCam.transform.position.x, 0, 0), Quaternion.identity);
        yield return null;
    }

    public IEnumerator CastLine(Vector2 targetPos)
    {

        props[2].position = rodpoints[2].position;
        Debug.Log(props[2].position);
        //anims[0].SetTrigger("go");
        anims[0].SetInteger("state", 3);
        
        // UPDATE THE ROD POINT POSITION IMMEDIATELY HERE
        //props[2].position = rodpoints[2].position;

        // Wait briefly for the throw animation to reach the "release" frame
        //yield return new WaitForSeconds(0.15f);
        //Debug.Log(props[2].position);

        // Launch the bobber AFTER the delay
        audioSource.PlayOneShot(swing);
        props[0].gameObject.SetActive(true);
        props[0].transform.position = props[2].transform.position;

        Rigidbody2D bobberRb = props[0].GetComponent<Rigidbody2D>();
        bobberRb.velocity = (targetPos - (Vector2)props[2].position) * 1.25f;

        Bobber bobberScript = props[0].GetComponent<Bobber>();
        //anims[0].SetInteger("state", 0);
        //anims[0].SetInteger("state", 0);
        //props[2].position = rodpoints[0].position;
        while (!bobberScript.onWater)
        {
            yield return null;
        }
        anims[0].SetInteger("state", 0);
        //anims[0].SetInteger("state", 0);
        props[2].position = rodpoints[0].position;
    }

    public IEnumerator EndGame(bool win, int fishType = 0)
    {
        menued = true;
        reeling = true;
        inGame = false;
        float timer = 0;
        anims[0].SetInteger("state", 1);
        yield return new WaitForSeconds(0.2f);
        props[2].position = rodpoints[1].position;
       

        if (win)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(fishFall);

            GameObject caughtFish = Instantiate(fish[fishType], props[0].position, Quaternion.identity);
            caughtFish.GetComponent<SpriteRenderer>().sprite = currentFihStats.fishSprite;
            Vector2 fishStartPos = caughtFish.transform.position;

            // Fish flies up to the player
            while (timer <= 1f)
            {
                timer += Time.deltaTime;
                caughtFish.transform.position = Vector2.Lerp(fishStartPos, props[0].position + new Vector3(0, 20, 0), timer);
                props[0].position = Vector2.Lerp(props[0].position, props[2].position, timer / 2f);
                caughtFish.transform.Rotate(0, 0, 360 * Time.deltaTime);

                if (Vector2.Distance(props[0].position, props[2].position) < 0.35f)
                    props[0].gameObject.SetActive(false);

                yield return null;
            }

            timer = 0;
            caughtFish.transform.position = new Vector2(props[1].position.x, props[1].position.y + 20);
            fishStartPos = caughtFish.transform.position;
            yield return new WaitForSeconds(0.5f);

            // Fish drops into bucket
            while (timer <= 1f)
            {
                timer += Time.deltaTime;
                props[0].position = Vector2.Lerp(props[0].position, props[2].position, timer / 2f);

                if (Vector2.Distance(props[0].position, props[2].position) < 0.35f)
                    props[0].gameObject.SetActive(false);

                caughtFish.transform.Rotate(0, 0, 360 * Time.deltaTime);
                caughtFish.transform.position = Vector2.Lerp(fishStartPos, props[1].position, timer);
                yield return null;
            }

            props[3].GetComponent<enceclopediaSCR>().AddFish(currentFihStats.fishIndexNum);
            isBaitUsed = true;
            Destroy(caughtFish);

            anims[1].SetTrigger("catch");
            audioSource.PlayOneShot(actionBucket);

           //// Setup dialogue
           //Referencer diagRefs = dialogue.GetComponent<Referencer>();
           //diagRefs.things[0].GetComponent<TextMeshPro>().text = listOfAdvices[Random.Range(0, listOfAdvices.Length)];
           //diagRefs.things[1].GetComponent<SpriteRenderer>().sprite = currentFihStats.fishSprite;
           //diagRefs.things[2].GetComponent<TextMeshPro>().text = currentFihStats.fishName;
           //
           //dialogueSkipped = false;
           //StartCoroutine(SlideDialogue(1));

            //if (dialogueSkipped)
            //{
                ShowCongratsMenu();
            //}
        }
        else
        {
            CanGo = true;
            audioSource.Stop();
            anims[2].SetTrigger("SunSad");

            while (timer <= 1f)
            {
                timer += Time.deltaTime;
                props[0].position = Vector2.Lerp(props[0].position, props[2].position, timer);
                yield return null;
            }

            props[0].gameObject.SetActive(false);
            menued = false;
        }

        anims[0].SetInteger("state", 0);
        canFish = true;
        reeling = false;
    }

    // --- UI Button Methods ---
    //IEnumerator SlideDialogue(float directionMultipler)
    //{
    //    if (directionMultipler == 1) dialogue.SetActive(true);
    //
    //    float timer = 0;
    //    Vector2 targetOffset = new Vector2(0, -5 + directionMultipler * 2);
    //
    //    while (timer < 1.5f)
    //    {
    //        dialogue.transform.position = Vector2.Lerp(dialogue.transform.position,
    //            new Vector2(mainCam.transform.position.x + targetOffset.x, mainCam.transform.position.y + targetOffset.y),
    //            Time.deltaTime * 2);
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //
    //    if (directionMultipler == -1)
    //    {
    //        dialogue.SetActive(false);
    //        ShowCongratsMenu();
    //    }
    //
    //    if (directionMultipler == 1)
    //    {
    //        bool skipped = false;
    //        while (!skipped)
    //        {
    //            canFish = false;
    //            CanGo = false;
    //            if (Input.GetButtonDown("Fire1"))
    //            {
    //                dialogueSkipped = true;
    //                skipped = true;
    //            }
    //            yield return null;
    //        }
    //        StartCoroutine(SlideDialogue(-1));
    //    }
    //
    //    canFish = true;
    //}

    private void ShowCongratsMenu()
    {
        congratsImage.sprite = currentFihStats.fishSprite;
        congratsText.text = $"{currentFihStats.fishName}\n{currentFihStats.fishRarity}\n{currentFihStats.minFishSize}\n{currentFihStats.fishPrice}";
        congratsMenu.SetActive(true);
        anims[2].SetTrigger("SunSurprised");
        isBaitUsed = true;
        CanGo = true;
        rewardClaimed = false;
        inGame = false;
    }

    public void TrashFish()
    {
        congratsReturn.SetActive(false);
        audioSource.PlayOneShot(releaseFSound);
        congratsMenu.SetActive(false);
        CanGo = true;
        rewardClaimed = true;
        menued = false;
    }

    public void KeepFish()
    {
        if (inventoryHolder.sizeOfInventory >= currentFihStats.minFishSize)
        {
            congratsReturn.SetActive(false);
            inventoryHolder.AddItem(currentFihStats);
            congratsMenu.SetActive(false);
            CanGo = true;
            rewardClaimed = true;
            menued = false;
        }
        else
        {
            errorMessage.text = "No place in Inventory!";
            erorrAnim.SetTrigger("Yes");
            congratsReturn.SetActive(true);
            congratsMenu.SetActive(false);
        }
    }

    public void CongrReturnButton() => StartCoroutine(CongrReturnCoroutine());

    IEnumerator CongrReturnCoroutine()
    {
        Time.timeScale = 0;
        congratsMenu.SetActive(!congratsMenu.activeSelf);
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1;
    }

    public void EnceclopediaButton() => StartCoroutine(EnceclopediaCoroutine());

    IEnumerator EnceclopediaCoroutine()
    {
        Time.timeScale = 0;
        enceclopedia.SetActive(!enceclopedia.activeSelf);
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1;
    }
}