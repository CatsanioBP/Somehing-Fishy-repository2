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
    public int fishIndexNum;
    public FishSCRobject Clone()
    {
        FishSCRobject copy = CreateInstance<FishSCRobject>();

        copy.fishName = fishName;
        copy.fishRarity = fishRarity;
        copy.minFishSize = minFishSize;
        copy.fishPrice = fishPrice;
        copy.fishSprite = fishSprite;
        copy.fishIndexNum = fishIndexNum;

        return copy;
    }
    //public bait typeOfBait;
}
