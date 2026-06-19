using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] AudioSource mainMenuSource;
    [SerializeField] AudioClip mainMenuSong;
    private void Start()
    {
        mainMenuSource.clip = mainMenuSong;
        mainMenuSource.Play();
    }
    public GameObject Credits,mainMenu;
    public void StartButton()
    {
        
                SceneManager.LoadScene(1);
        //SceneManager.LoadScene(0);
    }
    public void CreditsManager()
    {
        Credits.SetActive(!Credits.activeSelf);
        mainMenu.SetActive(!mainMenu.activeSelf);

    }
}
