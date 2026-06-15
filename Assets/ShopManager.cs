using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject ShopMenu,inventory,FishingSceneHolder;
    [SerializeField] bool isItem;
    [SerializeField] Animator InOutAnim;
    bool isInventoryActive = false;
    public bool canSell = false;
    [SerializeField] TMP_Text messageText;

    // Start is called before the first frame update
    public void OnMouseDown()
    {
        World world = Object.FindAnyObjectByType<World>();
        if (world == null)
        {
            Debug.LogError("World not found!");
            return;
        }
        if (world.CanGo == true && FishingSceneHolder.activeSelf)
        {
            //Debug.Log("pop2");
            world.inGame = true;
            StartCoroutine(Transition(true,world));
            //ShopMenu.SetActive(!ShopMenu.activeSelf);
            //FishingSceneHolder.SetActive(!FishingSceneHolder.activeSelf);
        }
        else if (world.CanGo == true && !FishingSceneHolder.activeSelf)
        {
            //Debug.Log("pop1");
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
        Debug.Log(world.inGame);
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
}
