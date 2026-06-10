using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public FishSCRobject fish;
    private Inventory inventory;

    public void Setup(FishSCRobject fishData, Inventory inv)
    {
        fish = fishData;
        inventory = inv;
    }

    public void OnClick()
    {
        inventory.ShowItemInfo(this);
    }
}
