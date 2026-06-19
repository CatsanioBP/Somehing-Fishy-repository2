using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject ShopMenu,inventory,FishingSceneHolder,baitInv;
    [SerializeField] bool isItem;
    [SerializeField] Animator InOutAnim;
    bool isInventoryActive = false;
    public bool canSell = false;
    int bopperBuyCount = 0;
    [SerializeField] TMP_Text messageText;
    [SerializeField] BaitSCRobject[] randomBait;
    [SerializeField] Inventory inv;
    [SerializeField] Sprite bopperMain;
    [SerializeField] Sprite[] boppers;
    [SerializeField] GameObject[] blockSighs;
    [SerializeField] TMP_Text errorMessage;
    [SerializeField] Animator erorrAnim;
    [SerializeField] AudioSource fishingSource;
    [SerializeField] AudioSource shopMusicSource;
    [SerializeField] AudioSource shopAmbientSource;

    [SerializeField] AudioClip FishingStartMusic;
    [SerializeField] AudioClip FishingLoopMusic;
    [SerializeField] AudioClip ShopAmbient;
    [SerializeField] AudioClip ShopSoundtreck;

    [SerializeField] float musicFadeTime = 2f;

    Coroutine currentMusicTransition;

    private void Start()
    {
        StartCoroutine(StartFishingMusic());
    }
    IEnumerator StartFishingMusic()
    {
        fishingSource.Stop();

        fishingSource.clip = FishingStartMusic;
        fishingSource.loop = false;
        fishingSource.volume = 1f;
        fishingSource.pitch = 1f;

        fishingSource.Play();

        yield return new WaitForSeconds(FishingStartMusic.length);

        fishingSource.clip = FishingLoopMusic;
        fishingSource.loop = true;
        fishingSource.Play();
    }
    public void EnterShopMusic()
    {
        if (currentMusicTransition != null)
            StopCoroutine(currentMusicTransition);

        currentMusicTransition = StartCoroutine(FadeToShop());
    }
    IEnumerator FadeToShop()
    {
        shopMusicSource.clip = ShopSoundtreck;
        shopAmbientSource.clip = ShopAmbient;

        shopMusicSource.loop = true;
        shopAmbientSource.loop = true;

        shopMusicSource.volume = 0;
        shopAmbientSource.volume = 0;

        shopMusicSource.Play();
        shopAmbientSource.Play();

        float timer = 0;

        float fishStartVol = fishingSource.volume;
        float fishStartPitch = fishingSource.pitch;

        while (timer < musicFadeTime)
        {
            timer += Time.deltaTime;

            float t = timer / musicFadeTime;

            fishingSource.volume =
                Mathf.Lerp(fishStartVol, 0f, t);

            fishingSource.pitch =
                Mathf.Lerp(fishStartPitch, 0.5f, t);

            shopMusicSource.volume =
                Mathf.Lerp(0f, 1f, t);

            shopAmbientSource.volume =
                Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        fishingSource.Stop();

        fishingSource.volume = 1f;
        fishingSource.pitch = 1f;
    }
    public void LeaveShopMusic()
    {
        if (currentMusicTransition != null)
            StopCoroutine(currentMusicTransition);

        currentMusicTransition = StartCoroutine(FadeToFishing());
    }
    IEnumerator FadeToFishing()
    {
        fishingSource.clip = FishingLoopMusic;
        fishingSource.loop = true;

        fishingSource.volume = 0;
        fishingSource.pitch = 0.5f;

        fishingSource.Play();

        float timer = 0;

        while (timer < musicFadeTime)
        {
            timer += Time.deltaTime;

            float t = timer / musicFadeTime;

            fishingSource.volume =
                Mathf.Lerp(0f, 1f, t);

            fishingSource.pitch =
                Mathf.Lerp(0.5f, 1f, t);

            shopMusicSource.volume =
                Mathf.Lerp(1f, 0f, t);

            shopAmbientSource.volume =
                Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        shopMusicSource.Stop();
        shopAmbientSource.Stop();

        shopMusicSource.volume = 1f;
        shopAmbientSource.volume = 1f;
    }

    // Start is called before the first frame update
    public void OnMouseDown()
    {
        World world = Object.FindAnyObjectByType<World>();
        if (world.CanGo == true && FishingSceneHolder.activeSelf)
        {
            if (gameObject.GetComponent<BoxCollider2D>() != null)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }

            EnterShopMusic();
            world.inGame = true;
            StartCoroutine(Transition(true, world));
            //ShopMenu.SetActive(!ShopMenu.activeSelf);
            //FishingSceneHolder.SetActive(!FishingSceneHolder.activeSelf);
        }
        else if (world.CanGo == true && !FishingSceneHolder.activeSelf)
        {
            //Debug.Log("pop1");
            if (gameObject.GetComponent<BoxCollider2D>() != null)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
            LeaveShopMusic();

            world.inGame = true;
            StartCoroutine(Transition(false, world));

            //ShopMenu.SetActive(!ShopMenu.activeSelf);
            //FishingSceneHolder.SetActive(!FishingSceneHolder.activeSelf);
            //StartCoroutine(world.Transition(true));
        }
    }

    public IEnumerator Transition(bool InOrOut,World world)
    {
        
        InOutAnim.SetTrigger("Out");
        yield return new WaitForSeconds(1.2f);
        //yield return new WaitForSeconds(1f);
        ShopMenu.SetActive(!ShopMenu.activeSelf);
        FishingSceneHolder.SetActive(!FishingSceneHolder.activeSelf);
       // world.inGame = false;
        InOutAnim.SetTrigger("In");
        yield return new WaitForSeconds(1.2f);
        if (FishingSceneHolder.activeSelf)
        {
            world.inGame = false;
        }
        else
        {
            world.inGame = true;
        }
    }
    //public void LeaveShop()
    //{
    //    //Debug.Log("yes");
    //    OnMouseDown();
    //}

    public void SellFunction()
    {
        if (!isInventoryActive)
        {
            inventory.SetActive(true);
            isInventoryActive = true;
            messageText.text = "Choose fish to sell";
            canSell = true;
        }
        else
        {
            inventory.SetActive(false);
            isInventoryActive = false;
            messageText.text = "";
            canSell = false;
        }
    }
    public void ShowBaitInv()
    {
        if (isInventoryActive == false)
        {
            baitInv.SetActive(!baitInv.activeSelf);
        }
    }
    public void BaitBuyButton()
    {
        //Inventory inv = Object.FindAnyObjectByType<Inventory>();
        if (inv.balance >= 20)
        {
            inv.balance -= 20;
            inv.balanceText.text = inv.balance + "$";
            BaitManager baitInventory = Object.FindAnyObjectByType<BaitManager>();
            int randIndex = Random.Range(0, 4);//0 - worm, 1 - special worm, 2-leech,3- srimp
            baitInventory.AddItem(randomBait[randIndex]);
        }
        else
        {
            errorMessage.text = "You don't have such money!";
            erorrAnim.SetTrigger("Yes");
            return;
        }
    }
    //public void Buy
    public void LineThingButton()
    {
        if(inv.balance >= 50)
        {
            
        }
    }
}
