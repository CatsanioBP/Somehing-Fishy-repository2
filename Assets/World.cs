using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class World : MonoBehaviour
{
    public bool inGame, canFish = true, reeling; // time doesnt pass when in game and stuff cant be interacted with, maybe the waves can still go tho idk
    public bool CanGo = true; // time doesnt pass when in game and stuff cant be interacted with, maybe the waves can still go tho idk
    public GameObject[] fish; // 0 is polderfish
    public Transform[] props; // 0 is bobber, 1 is fish bucket, 2 is the boys rod point
    public Transform[] rodpoints; // 0 is regular, 1 is reeling, 2 is tgrowing
    public FishSCRobject[] fishesPool;
    FishSCRobject currentFihStats;
    public Inventory inventoryHolder;
    Transform cam;
    Vector3 defaultCamPos;
    LineRenderer lr;
    public GameObject[] miniGames; // 0 is spinny, 1 is pully
    [SerializeField] Animator[] anims; // 0 is boy, 1 is bucket, 2 is sun, 3 is FadeOut/FadeIn

    void Start()
    {
        //canFish = CanGo = reeling = true;
        //Debug.Log(" START values are inGame=" + inGame + " canFish=" + canFish + " reeling= " + reeling + " CanGo="+CanGo);
        //inGame = true;
        //Transition(true);
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
        }

        if (Input.GetButtonUp("Fire1") && canFish && !inGame) // throwing the line
        {
            if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > props[2].position.x + 1){
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
                int randFih = Random.Range(0, 4);
                currentFihStats = fishesPool[randFih];
                StartCoroutine(startGame());
                
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

            GameObject fih = Instantiate(fish[fishType], props[0].position, Quaternion.identity);
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
            inventoryHolder.AddItem(currentFihStats);
            // maybe score it and make the sun emote, words coming from the bucket saying its rarity and a ding sound, should display fish rarity too
            anims[2].SetTrigger("SunSurprised");
            CanGo = true;
        }
        else
        {
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
        inGame = true;
        int x = Random.Range(0, 2); //x is a num that chooses random minigame,for now we have to so it'd from 0 to 2(1-SV,0-Spinny)
        Instantiate(miniGames[x], new Vector3(cam.position.x, 0, 0), Quaternion.identity);
        yield return null;
    }
    public IEnumerator castIt()
    {
        CanGo = false;
        anims[0].SetTrigger("go");
        props[2].position = rodpoints[2].position;
        anims[0].SetInteger("state", 0);
        yield return new WaitForSeconds(1);
        props[2].position = rodpoints[0].position;
    }
}