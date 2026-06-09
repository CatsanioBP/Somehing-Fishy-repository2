using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<FishSCRobject> inventory;
    public FishSCRobject fih;
    //public Sprite icon;
    int sizeOfInventory = 3;
    [SerializeField] Transform[] inventoryPanel;
    [SerializeField] GameObject inventorySlotPrefab;


    private int slotIndex = 0;

    void CreateInventoryIcon(FishSCRobject fish)
    {
        //Debug.Log("Creating icon for " + fish.name);
        GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel[slotIndex]);
        //Debug.Log(slot.transform.parent.name);
        RectTransform rect = slot.GetComponent<RectTransform>();
        rect.SetParent(inventoryPanel[slotIndex], false);
        rect.anchoredPosition = new Vector2(slotIndex, 0);

        Image image = slot.GetComponent<Image>();
        image.sprite = fish.fishSprite;

        slotIndex++;
    }
    void Update()
    {
        
    }
    public void AddItem(FishSCRobject currentFih)
    {
        if (sizeOfInventory < currentFih.fishSize)
        {
            Debug.Log("NO SPACE!");
        }
        else
        {                              
            inventory.Add(currentFih);
            CreateInventoryIcon(currentFih);
            sizeOfInventory -= currentFih.fishSize;
        }
    }
}
