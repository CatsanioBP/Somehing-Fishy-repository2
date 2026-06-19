#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    public float balance = 0;
    public List<FishSCRobject> inventory;
    //public FishSCRobject fih;
    //public Sprite icon;
    public float sizeOfInventory = 25,icons = 10;
    //bool isItemInfo = false;
    [SerializeField] Transform[] inventoryPanel;
    [SerializeField] GameObject inventorySlotPrefab;
    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] public TMP_Text fishNameText, balanceText;
    [SerializeField] BaitSCRobject[] meats;
    [SerializeField] TMP_Text errorMessage,sizeText;
    [SerializeField] Animator errorMes;

    //private int slotIndex = 0;
    private InventorySlot selectedSlot;

    private void Start()
    {
        sizeText.text = $"{sizeOfInventory:F1}" + "kg" + " space left";
        balanceText.text = balance + "$";
    }

    public void AddItem(FishSCRobject currentFih)
    {
        if (sizeOfInventory < currentFih.minFishSize)
        {
            errorMessage.text = "No space left!";
            errorMes.SetTrigger("Yes");
        }
        else
        {
            //inventorySlotPrefab.AddComponent<FishSCRobject>().fishName = currentFih.fishName;
            inventory.Add(currentFih);
            CreateInventoryIcon(currentFih);
            sizeOfInventory -= currentFih.minFishSize;
            icons -= 1;
            sizeText.text = $"{sizeOfInventory:F1}" + "kg" + " space left";
            Debug.Log(sizeOfInventory);
        }
    }
    public void ShowItemInfo(InventorySlot slot)
    {

        selectedSlot = slot;
        itemInfoPanel.SetActive(true);
        fishNameText.text = "<b>"+slot.fish.fishName + "</b>" + "\n" + slot.fish.fishPrice + "$\n" + slot.fish.minFishSize + "kg\n"
            + slot.fish.fishRarity + "\n" + slot.fish.fishDesc;
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
            icons += 1;
            sizeOfInventory += selectedSlot.fish.minFishSize;
            sizeText.text = $"{sizeOfInventory:F1}" + "kg" + " space left";
            Destroy(selectedSlot.gameObject);
            itemInfoPanel.SetActive(false);
            selectedSlot = null;
        }
    }
    public void LetItGoSelectedItem()
    {

        if (selectedSlot == null)
            return;
            inventory.Remove(selectedSlot.fish);
            icons += 1;
            sizeOfInventory += selectedSlot.fish.minFishSize;
        sizeText.text = $"{sizeOfInventory:F1}" + "kg" + " space left";
        Destroy(selectedSlot.gameObject);
            itemInfoPanel.SetActive(false);
            selectedSlot = null;
        
    }
    public void BaitSelectedItem()
    {

        if (selectedSlot == null)
            return;
        BaitManager bait = Object.FindAnyObjectByType<BaitManager>();
        if (selectedSlot.fish.fishRarity == "Common" && bait.sizeOfInventory > 0)
        {
            bait.AddItem(meats[0]);
        }
        else if (selectedSlot.fish.fishRarity == "Uncommon" && bait.sizeOfInventory > 0)
        {
            bait.AddItem(meats[1]);
        }
        else if (selectedSlot.fish.fishRarity == "Rare" && bait.sizeOfInventory > 0)
        {
            bait.AddItem(meats[2]);
        }
        else if (selectedSlot.fish.fishRarity == "Special" && bait.sizeOfInventory > 0)
        {
            bait.AddItem(meats[3]);
        }
        else
        {
            errorMessage.text = "You don't have place!";
            errorMes.SetTrigger("Yes");
            return;
        }
        inventory.Remove(selectedSlot.fish);
        icons += 1;
        sizeOfInventory += selectedSlot.fish.minFishSize;
        sizeText.text =$"{sizeOfInventory:F1}" + "kg" + " space left";
        Destroy(selectedSlot.gameObject);
        itemInfoPanel.SetActive(false);
        selectedSlot = null;

    }
    //public void BaitSelectedItem()
    //{
    //    if (selectedSlot == null)
    //        return;
    //    ShopManager sellButton = Object.FindAnyObjectByType<ShopManager>();
    //    if (sellButton.canSell == true)
    //    {
    //        balance += selectedSlot.fish.fishPrice;
    //        balanceText.text = balance + "$";
    //        inventory.Remove(selectedSlot.fish);
    //        sizeOfInventory += selectedSlot.fish.minFishSize;
    //        Destroy(selectedSlot.gameObject);
    //        itemInfoPanel.SetActive(false);
    //        selectedSlot = null;
    //    }
    //}
}
