using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public float balance = 0;
    public List<FishSCRobject> inventory;
    //public FishSCRobject fih;
    //public Sprite icon;
    int sizeOfInventory = 4;
    [SerializeField] Transform[] inventoryPanel;
    [SerializeField] GameObject inventorySlotPrefab;
    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private TMP_Text fishNameText,balanceText;

    private int slotIndex = 0;
    private InventorySlot selectedSlot;

    public void AddItem(FishSCRobject currentFih)
    {
        if (sizeOfInventory < currentFih.fishSize)
        {
            Debug.Log("NO SPACE!");
        }
        else
        {
            //inventorySlotPrefab.AddComponent<FishSCRobject>().fishName = currentFih.fishName;
            inventory.Add(currentFih);
            CreateInventoryIcon(currentFih);
            sizeOfInventory -= currentFih.fishSize;
        }
    }
    public void ShowItemInfo(InventorySlot slot)
    {

        selectedSlot = slot;

        itemInfoPanel.SetActive(true);
        fishNameText.text = "Name:"+slot.fish.fishName + "\nPrice:" + slot.fish.fishPrice + "\nSize:" +slot.fish.fishSize;

        //deleteButton.onClick.RemoveAllListeners();
        //deleteButton.onClick.AddListener(DeleteSelectedItem);
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
        if (sellButton.canSell == true)
        {
            balance += selectedSlot.fish.fishPrice;
            balanceText.text = balance + "$";
            inventory.Remove(selectedSlot.fish);
            sizeOfInventory += selectedSlot.fish.fishSize;
            Destroy(selectedSlot.gameObject);
            itemInfoPanel.SetActive(false);
            selectedSlot = null;
        }
    }
}
