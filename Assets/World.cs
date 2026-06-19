using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class World : MonoBehaviour
{
    public bool inGame, canFish = true, reeling; // time doesnt pass when in game and stuff cant be interacted with, maybe the waves can still go tho idk
    private bool rewardClaimed,dialogueSkipped = false;
    public bool CanGo = true,isBaitUsed; // time doesnt pass when in game and stuff cant be interacted with, maybe the waves can still go tho idk
    public float chanceFromBait = 0;
    public float sizeHelper=0;
    public GameObject congratsMenu,congratsReturn;
    [SerializeField] TMP_Text congratsText;
    [SerializeField] Image congratsImage;
    public GameObject[] fish; // 0 is polderfish
    public Transform[] props; // 0 is bobber, 1 is fish bucket, 2 is the boys rod point
    public Transform[] rodpoints; // 0 is regular, 1 is reeling, 2 is tgrowing
    public FishSCRobject[] comFishesPool,uncFishesPool,rarFishesPool,specFishesPool; //pools of fishes, common,uncommon,rare,special
    FishSCRobject currentFihStats;
    public Inventory inventoryHolder;
    [HideInInspector] public bool cursorHover, throwing, menued;
    public static int dayOn = 1;
    [SerializeField] TextMeshProUGUI dayDisplayer;
    Transform cam;
    Vector3 defaultCamPos;
    LineRenderer lr;
    public GameObject[] miniGames; // 0 is spinny, 1 is pully
    [SerializeField] public GameObject dialogue;
    public string[] listOfAdvices;
    [SerializeField] Animator[] anims; // 0 is boy, 1 is bucket, 2 is sun, 3 is FadeOut/FadeIn
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip actionBucket,swing,pushSound,releaseFSound,fishFall;
    [SerializeField] TMP_Text errorMessage;
    [SerializeField] Animator erorrAnim;
    public int upgradeNum = 0;

    void Start()
    {
        //canFish = CanGo = reeling = true;
        //Debug.Log(" START values are inGame=" + inGame + " canFish=" + canFish + " reeling= " + reeling + " CanGo="+CanGo);
        //inGame = true;
        //Transition(true);
        rewardClaimed = true;
        isBaitUsed = true;
        lr = GetComponent<LineRenderer>();
        props[0].gameObject.SetActive(false);
        cam = Camera.main.transform;
        defaultCamPos = cam.transform.position;
    }
    

    void Update()
    {
        //Debug.Log(" UPDATE values are inGame=" + inGame + " canFish=" + canFish + " reeling= " + reeling + " CanGo=" + CanGo);
        if (Input.GetButton("Fire1") && canFish && !inGame)
        {
            anims[0].SetInteger("state", 1);
            throwing = true;
        }
        dayDisplayer.text = "Day " + dayOn;

        if (Input.GetButtonUp("Fire1") && canFish && !inGame && !menued && rewardClaimed) // throwing the line
        {
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > props[2].position.x + 1){
                CanGo = false;

                audioSource.PlayOneShot(swing);
                props[0].gameObject.SetActive(true);
                props[0].transform.position = props[2].transform.position;
                props[0].GetComponent<Rigidbody2D>().velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - props[2].position) * 1.25f; // maybe better fishing lines throw farther
                canFish = false;
                StartCoroutine(castIt());
            }
            else
            {
                anims[0].SetInteger("state", 0);
            }
        }

        if (!canFish)
        {
            lr.enabled = true;
            lr.SetPosition(0, props[0].position);
            lr.SetPosition(1, props[2].position);

            cam.position = Vector3.Lerp(cam.transform.position, new Vector3(Mathf.Clamp(props[0].position.x, 0, 80), defaultCamPos.y, -10), Time.deltaTime * 5);

            if (!inGame && Input.GetButtonDown("Fire1") && !inGame && !reeling)
            {
                CanGo = false;
                if (props[0].GetComponent<Bobber>().bob > 0)
                {

                    float distanceBoyBopper = Vector2.Distance(props[0].position, props[2].position);
                    float whichPool = Random.Range(0, 50);

                    whichPool += chanceFromBait + distanceBoyBopper;
                    chanceFromBait = 0;
                    //isBaitUsed = true;
                    if (whichPool <= 50)
                    {
                        int randFih = Random.Range(0, comFishesPool.Length);
                        currentFihStats = comFishesPool[randFih].Clone();
                        //comFishesPool[randFih].minFishSize = Random.Range(comFishesPool[randFih].minFishSize, comFishesPool[randFih].minFishSize * 5);
                        float currentFishSize = Random.Range(currentFihStats.minFishSize, currentFihStats.minFishSize + 2 + sizeHelper);
                        float roundedNum = Mathf.Round(currentFishSize * 10f) / 10f;

                        //float finalFishPrice
                        int finalPrice = Mathf.RoundToInt(currentFihStats.fishPrice * (roundedNum / currentFihStats.minFishSize));
                        //currentFihStats = comFishesPool[randFih];

                        currentFihStats.minFishSize = roundedNum;
                        currentFihStats.fishPrice = finalPrice;

                    }
                    else if (whichPool > 50 && whichPool <= 85)
                    {
                        int randFih = Random.Range(0, uncFishesPool.Length);
                        currentFihStats = uncFishesPool[randFih].Clone();
                        float currentFishSize = Random.Range(uncFishesPool[randFih].minFishSize, uncFishesPool[randFih].minFishSize + 2);
                        float roundedNum = Mathf.Round(currentFishSize * 10f) / 10f;

                        //float finalFishPrice
                        int finalPrice = Mathf.RoundToInt(currentFihStats.fishPrice * (roundedNum / currentFihStats.minFishSize));
                        //currentFihStats = uncFishesPool[randFih];

                        currentFihStats.minFishSize = roundedNum;
                        currentFihStats.fishPrice = finalPrice;
                        //currentFihStats = uncFishesPool[randFih];
                    }
                    else if (whichPool > 85)
                    {
                        int randFih = Random.Range(0, rarFishesPool.Length);
                        currentFihStats = rarFishesPool[randFih].Clone();
                        float currentFishSize = Random.Range(rarFishesPool[randFih].minFishSize, rarFishesPool[randFih].minFishSize + 2);
                        float roundedNum = Mathf.Round(currentFishSize * 10f) / 10f;

                        //float finalFishPrice
                        int finalPrice = Mathf.RoundToInt(currentFihStats.fishPrice * (roundedNum / currentFihStats.minFishSize));
                        //currentFihStats = rarFishesPool[randFih];

                        currentFihStats.minFishSize = roundedNum;
                        currentFihStats.fishPrice = finalPrice;
                    }
                    else if (whichPool < 0)
                    {
                        int randFih = Random.Range(0, 999);
                        if (randFih < 333)
                        {
                            randFih = 0;
                        }
                        if (randFih >= 333 && randFih < 666)
                        {
                            randFih = 1;
                        }
                        if (randFih >= 666 && randFih < 999)
                        {
                            randFih = 2;
                        }
                        if(randFih == 999)
                        {
                            randFih = 3;
                        }
                        currentFihStats = specFishesPool[randFih].Clone();
                        float currentFishSize = Random.Range(specFishesPool[randFih].minFishSize, specFishesPool[randFih].minFishSize + 2 + upgradeNum);
                        float roundedNum = Mathf.Round(currentFishSize * 10f) / 10f;

                        //float finalFishPrice
                        int finalPrice = Mathf.RoundToInt(currentFihStats.fishPrice * (roundedNum / currentFihStats.minFishSize));
                        //currentFihStats = rarFishesPool[randFih];

                        currentFihStats.minFishSize = roundedNum;
                        currentFihStats.fishPrice = finalPrice;
                    }

                    StartCoroutine(startGame());
                }
                else
                {
                    StartCoroutine(endGame(false));
                }
            }
        }
        else
        {
            lr.enabled = false;
            cam.position = Vector3.Lerp(cam.transform.position, defaultCamPos, Time.deltaTime * 5);
        }
    }

    public IEnumerator endGame(bool win, int fishType = 0)
    {
        reeling = true;
        float tmr = 0;
        inGame = false;

        props[2].position = rodpoints[1].position;
        anims[0].SetInteger("state", 1);

        if(win){
            audioSource.Stop();
            audioSource.PlayOneShot(fishFall);
            GameObject fih = Instantiate(fish[fishType], props[0].position, Quaternion.identity);
            fih.GetComponent<SpriteRenderer>().sprite = currentFihStats.fishSprite;
            Vector2 fihpos = fih.transform.position;

            while(tmr <= 1f){
                yield return null;
                tmr+=Time.deltaTime;
                fih.transform.position = Vector2.Lerp(fihpos, props[0].position + new Vector3(0, 20, 0), tmr / 1f);
                props[0].position = Vector2.Lerp(props[0].position, props[2].position, tmr / 2f);
                fih.transform.Rotate(0, 0, 360 * Time.deltaTime);

                if(Vector2.Distance(props[0].position, props[2].position) < 0.35f)
                {
                    props[0].gameObject.SetActive(false);
                }
            }
            
            tmr = 0;
            fih.transform.position = new Vector2(props[1].position.x, props[1].position.y + 20);
            fihpos = fih.transform.position;
            yield return new WaitForSeconds(0.5f);
            
            while(tmr <= 1f){
                yield return null;
                props[0].position = Vector2.Lerp(props[0].position, props[2].position, tmr / 2f);

                if(Vector2.Distance(props[0].position, props[2].position) < 0.35f)
                {
                    props[0].gameObject.SetActive(false);
                }

                tmr+=Time.deltaTime;
                fih.transform.Rotate(0, 0, 360 * Time.deltaTime);
                fih.transform.position = Vector2.Lerp(fihpos, props[1].position, tmr / 1f);
            }

            Destroy(fih.gameObject);
            anims[1].SetTrigger("catch");
            audioSource.PlayOneShot(actionBucket);
            dialogue.GetComponent<Referencer>().things[0].GetComponent<TextMeshPro>().text = listOfAdvices[Random.Range(0, listOfAdvices.Length)];
            dialogue.GetComponent<Referencer>().things[1].GetComponent<SpriteRenderer>().sprite = currentFihStats.fishSprite;
            dialogue.GetComponent<Referencer>().things[2].GetComponent<TextMeshPro>().text = currentFihStats.fishName;
            dialogueSkipped = false;
            StartCoroutine(slide(1));
            if (dialogueSkipped == true)
            {
                congratsImage.sprite = currentFihStats.fishSprite;
                congratsText.text = currentFihStats.fishName + "\n" + currentFihStats.fishRarity + "\n" + currentFihStats.minFishSize + "\n" + currentFihStats.fishPrice;
                congratsMenu.SetActive(true);
                anims[2].SetTrigger("SunSurprised");
                isBaitUsed = true;
                CanGo = true;
                rewardClaimed = false;
                inGame = false;
            }
        }
        else
        {
            CanGo = true;
            audioSource.Stop();
            // play a dissapointing "kwaow"
            anims[2].SetTrigger("SunSad");
            while(tmr <= 1f){
                yield return null;
                tmr+=Time.deltaTime;
                props[0].position = Vector2.Lerp(props[0].position, props[2].position, tmr / 1f);
            }

            props[0].gameObject.SetActive(false);
        }

        anims[0].SetInteger("state", 0);

        canFish = true;
        reeling = false;
    }
    public IEnumerator startGame()
    {
        audioSource.PlayOneShot(pushSound,0.2f);
        inGame = true;
        int x = Random.Range(0, 2); //x is a num that chooses random minigame,for now we have to so it'd from 0 to 2(1-SV,0-Spinny)
        Instantiate(miniGames[x], new Vector3(cam.position.x, 0, 0), Quaternion.identity);
        yield return null;
    }
    public IEnumerator castIt()
    {
       
        anims[0].SetTrigger("go");
        props[2].position = rodpoints[2].position;
        while (!props[0].GetComponent<Bobber>().onWater)
            yield return null;
        anims[0].SetInteger("state", 0);
        yield return new WaitForSeconds(1);
        props[2].position = rodpoints[0].position;
    }
    public void TrashFish()
    {
        
        congratsReturn.SetActive(false);
        audioSource.PlayOneShot(releaseFSound);
        congratsMenu.SetActive(false);
        CanGo = true;
        //inGame = false;
        rewardClaimed = true;
    }
    public void KeepFish()
    {
        if (inventoryHolder.sizeOfInventory >= currentFihStats.minFishSize)
        {
            congratsReturn.SetActive(false);
            inventoryHolder.AddItem(currentFihStats);
            congratsMenu.SetActive(false);
            CanGo = true;
            //canFish = true;
            rewardClaimed = true;
            //inGame = false;
        }
        else
        {
            errorMessage.text = "No place in Inventory!";
            erorrAnim.SetTrigger("Yes");
            congratsReturn.SetActive(true);
            congratsMenu.SetActive(false);
        }
    }
    public void congrReturnButton()
    {
        congratsMenu.SetActive(!congratsMenu.activeSelf);
    }

    IEnumerator slide(float slway)
    {
       
        if (slway == 1)
            dialogue.SetActive(true);

        float tmr = 0;
        Vector2 wherego = new Vector2(0, -5 + slway * 2);

        while (tmr < 1.5f)
        {
            yield return null;
            dialogue.transform.position = Vector2.Lerp(dialogue.transform.position, new Vector2(cam.transform.position.x + wherego.x, cam.transform.position.y + wherego.y), Time.deltaTime * 2);
            tmr += Time.deltaTime;
        }

        if (slway == -1)
        {
            dialogue.SetActive(false);
            congratsImage.sprite = currentFihStats.fishSprite;
            congratsText.text = currentFihStats.fishName + "\n" + currentFihStats.fishRarity + "\n" + currentFihStats.minFishSize + "\n" + currentFihStats.fishPrice;
            congratsMenu.SetActive(true);
            anims[2].SetTrigger("SunSurprised");
            isBaitUsed = true;
            CanGo = true;
            rewardClaimed = false;
            inGame = false;
        }

            bool o = false;

        if (slway == 1)
        {
            while (!o)
            {
                canFish = false;
                CanGo = false;
                if (Input.GetButtonDown("Fire1"))
                {
                    dialogueSkipped = true;
                    o = true;
                }
                   
                yield return null;
            }

            StartCoroutine(slide(-1));
        }

        canFish = true;
    }
}