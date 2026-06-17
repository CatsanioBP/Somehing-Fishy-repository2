using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaitSCRobject", menuName = "Scriptable Objects/BaitSCRobject")]
public class BaitSCRobject : ScriptableObject
{
    public Sprite baitSprite;
    public int fishPoolNum;
    public string baitName;
    public string baitDesc;
}
