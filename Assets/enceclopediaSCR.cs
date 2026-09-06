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
    // Start is called before the first frame update
    void Start()
    {
             
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void eRB()
    {
        pages[pageCount].SetActive(false);
        if(pageCount == pages.Length - 1)
        {
            pageCount = 0;
        }
        else
            pageCount++;
        pageText.text = pageCount + "/20";
        pages[pageCount].SetActive(true);
    }
    public void eLB()
    {
        pages[pageCount].SetActive(false);
        if (pageCount == 0)
        {
            pageCount = pages.Length-1;
        }
        else
            pageCount--;
        pageText.text = pageCount + "/20";
        pages[pageCount].SetActive(true);
    }

}
