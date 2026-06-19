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

    // Start is called before the first frame update
    public void OnMouseDown()
    {
        World world = Object.FindAnyObjectByType<World>();
        if (world.CanGo == true && FishingSceneHolder.activeSelf)
        {
            if(gameObject.GetComponent<BoxCollider2D>() != null)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            //Debug.Log("pop2");
            //for(int i = 4;i<blockSighs.Length;i++)
            //{
            //    blockSighs[i].SetActive(true);
            //}
            world.inGame = true;
            StartCoroutine(Transition(true,world));
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
            world.inGame = true;
            StartCoroutine(Transition(false,world));
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
