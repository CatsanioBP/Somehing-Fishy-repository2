using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EncScr : MonoBehaviour
{
    [SerializeField] List<GameObject> pages;
    [SerializeField] int pageCount = 1;
    [SerializeField] TextMeshPro pageCountT;
    // Start is called before the first frame update
    public void LeftPage()
    {
        if (pageCount == 0)
        {
            Debug.Log(pageCount);
            pages[pageCount].SetActive(false);
            pageCount = pages.Count-1;
            pages[pageCount].SetActive(true);
            
        }
        else
        {
            pages[pageCount].SetActive(false);
            pageCount--;
            pages[pageCount].SetActive(true);
           
        }
        pageCountT.text = pageCount + "/10";
    }
    public void RightPage() 
    {
        if (pageCount == pages.Count-1)
        {
            Debug.Log(pageCount);
            pages[pageCount].SetActive(false);
            pageCount = 0;
            pages[pageCount].SetActive(true);
        }
        else
        {
            pages[pageCount].SetActive(false);
            pageCount++;
            pages[pageCount].SetActive(true);
        }
        pageCountT.text = pageCount + "/10";
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
