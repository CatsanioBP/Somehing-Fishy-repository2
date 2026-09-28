using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel, pausePanel;
    [SerializeField] private bool isPaused = false;

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
            {
                // Unpause Game
                Time.timeScale = 1;
                pausePanel.SetActive(false);
                optionsPanel.SetActive(false);
                isPaused = false;
            }
            else
            {
                // Pause Game
                Time.timeScale = 0;
                pausePanel.SetActive(true);
                optionsPanel.SetActive(false);
                isPaused = true;
            }
        }
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false); // Ensures options close if open
        isPaused = false; // CRITICAL: Tells the script the game is no longer paused
    }

    public void OptionsButton()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    public void SaveAndQuit()
    {
        //leaves to main menu
        SceneManager.LoadScene(0);
    }
}

