using UnityEngine;

[CreateAssetMenu(fileName = "FishSCRobject", menuName = "Scriptable Objects/FishSCRobject")]
public class FishSCRobject : ScriptableObject
{
    public Sprite fishSprite;
    public float fishPrice;
    public float minFishSize;
    public string fishName;
    public string fishRarity;
    public string fishDesc;
    //public bait typeOfBait;
}
