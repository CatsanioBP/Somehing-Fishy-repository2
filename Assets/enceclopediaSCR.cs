using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class enceclopediaSCR : MonoBehaviour
{
    public GameObject[] fishes,pages;
    public int pageCount = 0;
    [SerializeField] TMP_Text pageText;
    
    public void AddFish(int fishIndex)
    {
        fishes[fishIndex].gameObject.SetActive(true);
    }
    public void eRB()
    {
        pages[pageCount].SetActive(false);
        if(pageCount == pages.Length - 1)
        {
            pageCount = 1;
        }
        else
            pageCount++;
        int a = pages.Length - 1;
        pageText.text = pageCount + "/" + a;
        pages[pageCount].SetActive(true);
    }
    public void eLB()
    {
        pages[pageCount].SetActive(false);
        if (pageCount == 1)
        {
            pageCount = pages.Length-1;
        }
        else
            pageCount--;
        int a = pages.Length - 1;
        pageText.text = pageCount + "/" + a;
        pages[pageCount].SetActive(true);
    }
}
