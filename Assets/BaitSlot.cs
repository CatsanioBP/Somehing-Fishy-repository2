using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitSlot : MonoBehaviour
{
    public BaitSCRobject bait;
    private BaitManager inventory;

    public void Setup(BaitSCRobject baitData, BaitManager inv)
    {
        bait = baitData;
        inventory = inv;
    }

    public void OnClick()
    {
        inventory.ShowItemInfo(this);
    }
}
