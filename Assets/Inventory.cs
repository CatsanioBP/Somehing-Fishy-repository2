using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Object = UnityEngine.Object;

public class Inventory : MonoBehaviour
{
    public float balance = 0;
    public List<FishSCRobject> inventory;
    //public FishSCRobject fih;
    //public Sprite icon;
    float sizeOfInventory = 4;
    //bool isItemInfo = false;
    [SerializeField] Transform[] inventoryPanel;
    [SerializeField] GameObject inventorySlotPrefab;
    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private TMP_Text fishNameText,balanceText;

    //private int slotIndex = 0;
    private InventorySlot selectedSlot;

    private void Start()
    {
        balanceText.text = balance + "$";
    }
    public void AddItem(FishSCRobject currentFih)
    {
        if (sizeOfInventory < currentFih.minFishSize)
        {
            Debug.Log("NO SPACE!");
        }
        else
        {
            //inventorySlotPrefab.AddComponent<FishSCRobject>().fishName = currentFih.fishName;
            inventory.Add(currentFih);
            CreateInventoryIcon(currentFih);
            sizeOfInventory -= currentFih.minFishSize;
        }
    }
    public void ShowItemInfo(InventorySlot slot)
    {

        selectedSlot = slot;
        itemInfoPanel.SetActive(true);
        fishNameText.text = "Name:"+slot.fish.fishName + "\nPrice:" + slot.fish.fishPrice + "\nSize:" + slot.fish.minFishSize + "\nRarity:"
            + slot.fish.fishRarity + "\nDescription:" + slot.fish.fishDesc;
        //isItemInfo = true;
    }
    public void CloseItemInfo()
    {
        itemInfoPanel.SetActive(false);
        //isItemInfo = false;
    }
    void CreateInventoryIcon(FishSCRobject fish)
    {
        for (int i = 0; i < inventoryPanel.Length; i++)
        {
            if (inventoryPanel[i].childCount == 0)
            {
                //Debug.Log("Creating icon for " + fish.name);
                GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel[i]);
                //Debug.Log(slot.transform.parent.name);
                RectTransform rect = slot.GetComponent<RectTransform>();
                rect.SetParent(inventoryPanel[i], false);
                rect.anchoredPosition = new Vector2(i, 0);
                InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
                inventorySlot.Setup(fish, this);

                Button button = slot.GetComponent<Button>();
                button.onClick.AddListener(inventorySlot.OnClick);
                
                Image image = slot.GetComponent<Image>();
                image.sprite = fish.fishSprite;
                //slotIndex++;
                break;
            }
        }
    }
   public void SellSelectedItem()
    {

        if (selectedSlot == null)
            return;
        ShopManager sellButton = Object.FindAnyObjectByType<ShopManager>();
        if (sellButton.canSell)
        {
            balance += selectedSlot.fish.fishPrice;
            balanceText.text = balance + "$";
            inventory.Remove(selectedSlot.fish);
            sizeOfInventory += selectedSlot.fish.minFishSize;
            Destroy(selectedSlot.gameObject);
            itemInfoPanel.SetActive(false);
            selectedSlot = null;
        }
    }
    public void BaitSelectedItem()
    {
        if (selectedSlot == null)
            return;
        ShopManager sellButton = Object.FindAnyObjectByType<ShopManager>();
        if (sellButton.canSell == true)
        {
            balance += selectedSlot.fish.fishPrice;
            balanceText.text = balance + "$";
            inventory.Remove(selectedSlot.fish);
            sizeOfInventory += selectedSlot.fish.minFishSize;
            Destroy(selectedSlot.gameObject);
            itemInfoPanel.SetActive(false);
            selectedSlot = null;
        }
    }
}
