using UnityEngine;

[CreateAssetMenu(fileName = "FishSCRobject", menuName = "Scriptable Objects/FishSCRobject")]
public class FishSCRobject : ScriptableObject
{
    public Sprite fishSprite;
    public float fishPrice;
    public int fishSize;
    public string fishName;
    //public bait typeOfBait;
}
