using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject ShopMenu, inventory, FishingSceneHolder, baitInv;
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

    [SerializeField] Sprite[] buttons;
    [SerializeField] Sprite soldout;
    [SerializeField] Animator angler;

    private bool isUsed1 = false, isUsed2 = false, isUsed3 = false;
    public static bool isHook = false;

    private void Start()
    {
        // Calls the starting music from VolumeManager
        VolumeManager.Instance.StartFishingMusic();
    }

    public void OnMouseDown()
    {
        World world = Object.FindAnyObjectByType<World>();

        if (world.CanGo == true && FishingSceneHolder.activeSelf)
        {
            if (gameObject.GetComponent<BoxCollider2D>() != null)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }

            // Triggers the pitch-bending warp effect into the shop
            VolumeManager.Instance.EnterShopMusic();

            world.inGame = true;
            StartCoroutine(Transition(true, world));
        }
        else if (world.CanGo == true && !FishingSceneHolder.activeSelf)
        {
            if (gameObject.GetComponent<BoxCollider2D>() != null)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }

            // Triggers the pitch-bending warp effect back to fishing
            VolumeManager.Instance.LeaveShopMusic();

            world.inGame = true;
            StartCoroutine(Transition(false, world));
        }
    }

    public IEnumerator Transition(bool InOrOut, World world)
    {
        InOutAnim.SetTrigger("Out");

        // Wait for the animation (and the music warp effect) to progress
        yield return new WaitForSeconds(1.2f);

        ShopMenu.SetActive(!ShopMenu.activeSelf);
        FishingSceneHolder.SetActive(!FishingSceneHolder.activeSelf);

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
            angler.SetTrigger("Yay");
            inv.balance -= 20;
            inv.balanceText.text = inv.balance + "$";
            BaitManager baitInventory = Object.FindAnyObjectByType<BaitManager>();
            int randIndex = Random.Range(0, 4);//0 - worm, 1 - special worm, 2-leech,3- srimp
            baitInventory.AddItem(randomBait[randIndex]);
        }
        else
        {
            angler.SetTrigger("No");

            errorMessage.text = "You don't have such money!";
            erorrAnim.SetTrigger("Yes");
        }
    }
    //public void Buy
    public void LineThingButton()
    {
        if(inv.balance >= 150 && isUsed1 == false)
        {
            angler.SetTrigger("Yay");
            isUsed1 = true;
            World world = Object.FindAnyObjectByType<World>();
            world.upgradeNum++;
            inv.balance -= 150;
            inv.balanceText.text = inv.balance + "$";
            errorMessage.text = "+ fish size appears!";
            erorrAnim.SetTrigger("Yes");
        }
        else
        {
            angler.SetTrigger("No");
            errorMessage.text = "Not enough $/Not in stock";
            erorrAnim.SetTrigger("Yes");
        }
    }
    public void bobberBuy()
    {
        if (inv.balance >= 150 && isUsed2 == false)
        {
            angler.SetTrigger("Yay");
            isUsed2 = true;
            Bobber world = Object.FindAnyObjectByType<Bobber>();
            world.bobberHelp += 10;
            inv.balance -= 150;
            inv.balanceText.text = inv.balance + "$";
            errorMessage.text = "+ less time to wait bobber!";
            erorrAnim.SetTrigger("Yes");
        }
        else
        {
            angler.SetTrigger("No");
            errorMessage.text = "Not enough $/Not in stock";
            erorrAnim.SetTrigger("Yes");
        }
    }
    public void HookBuy()
    {
        if (inv.balance >= 200 && isUsed2 == false)
        {
            angler.SetTrigger("Yay");
            isUsed2 = true;
            isHook = true;
            inv.balance -= 200;
            inv.balanceText.text = inv.balance + "$";
            errorMessage.text = "+ Fish is slower!";
            erorrAnim.SetTrigger("Yes");
        }
        else
        {
            angler.SetTrigger("No");
            errorMessage.text = "Not enough $/Not in stock";
            erorrAnim.SetTrigger("Yes");
        }
    }

}
