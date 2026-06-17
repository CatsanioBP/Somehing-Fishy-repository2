using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class BaitManager : MonoBehaviour
{
    [SerializeField] Image usedBaitImage;
    [SerializeField] Sprite blankUse;
    public List<BaitSCRobject> inventory;
    //public FishSCRobject fih;
    //public Sprite icon;
    float sizeOfInventory = 6;
    //bool isItemInfo = false;
    [SerializeField] Transform[] baitInventoryPanel;
    [SerializeField] GameObject inventorySlotPrefab;
    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private TMP_Text baitNameText;
    [SerializeField] BaitSCRobject exampleBait;

    //private int slotIndex = 0;
    private BaitSlot selectedSlot;

    private void Start()
    {
        AddItem(exampleBait);
        AddItem(exampleBait);
    }
    private void Update()
    {
        World world = Object.FindAnyObjectByType<World>();
        if(world.isBaitUsed == true)
        {
            usedBaitImage.sprite = blankUse;
        }
    }
    public void AddItem(BaitSCRobject currentBait)
    {
        if (sizeOfInventory <= 0)
        {
            Debug.Log("NO SPACE!");
        }
        else
        {
            //inventorySlotPrefab.AddComponent<FishSCRobject>().fishName = currentFih.fishName;
            inventory.Add(currentBait);
            CreateInventoryIcon(currentBait);
            sizeOfInventory -= 1;
        }
    }
    public void ShowItemInfo(BaitSlot slot)
    {

        selectedSlot = slot;
        itemInfoPanel.SetActive(true);
        baitNameText.text = "<b>" + slot.bait.baitName + "</b>" + "\n" + slot.bait.baitDesc;
        //isItemInfo = true;
    }
    public void CloseItemInfo()
    {
        itemInfoPanel.SetActive(false);
        //isItemInfo = false;
    }
    void CreateInventoryIcon(BaitSCRobject bait)
    {
        for (int i = 0; i < baitInventoryPanel.Length; i++)
        {
            if (baitInventoryPanel[i].childCount == 0)
            {
                //Debug.Log("Creating icon for " + fish.name);
                GameObject slot = Instantiate(inventorySlotPrefab, baitInventoryPanel[i]);
                //Debug.Log(slot.transform.parent.name);
                RectTransform rect = slot.GetComponent<RectTransform>();
                rect.SetParent(baitInventoryPanel[i], false);
                rect.anchoredPosition = new Vector2(i, 0);
                BaitSlot inventorySlot = slot.GetComponent<BaitSlot>();
                inventorySlot.Setup(bait, this);

                Button button = slot.GetComponent<Button>();
                button.onClick.AddListener(inventorySlot.OnClick);

                Image image = slot.GetComponent<Image>();
                image.sprite = bait.baitSprite;
                //slotIndex++;
                break;
            }
        }
    }
    public void ActiveSelectedItem()
    {
        if (selectedSlot == null)
            return;
        //BaitManager sellButton = Object.FindAnyObjectByType<ShopManager>();
        //if (sellButton.canSell)
        //{
        World world = Object.FindAnyObjectByType<World>();
        world.isBaitUsed = false;
        world.chanceFromBait += selectedSlot.bait.fishPoolNum;
        usedBaitImage.sprite = selectedSlot.bait.baitSprite;
        inventory.Remove(selectedSlot.bait);            
        Destroy(selectedSlot.gameObject);
        itemInfoPanel.SetActive(false);
        selectedSlot = null;
        sizeOfInventory += 1;
        //}
    }
}
