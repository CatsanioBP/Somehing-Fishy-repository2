using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject ShopMenu,inventory;
    [SerializeField] Animator fadeInOut;
    [SerializeField] bool isItem;
    public bool canSell = false;
    [SerializeField] Text messageText;
    
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        World world = Object.FindAnyObjectByType<World>();
        if (world.inGame == false && isItem == false)
        {

            ShopMenu.SetActive(!ShopMenu.activeSelf);
        }
        else if(world.inGame == true && isItem == false)
        {
           
        }
    }

    public void SellFunction()
    {
        inventory.SetActive(true);
        messageText.text = "Choose fish to sell";
        canSell = true;
    }

// Update is called once per frame
void Update()
    {
        
    }
}
