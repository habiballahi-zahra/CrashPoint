using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject helpPanel;

    void Start()
    {
    }

    // ======================
    //  نمایش پنل‌ها
    // ======================



    public void ShowSettings()
    {

        settingsPanel.SetActive(true);
        helpPanel.SetActive(false);
    }

    public void ShowHelp()
    {

        settingsPanel.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void ClosePannel()
    {

        settingsPanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    // ======================
    // 🎮 دکمه‌ها
    // ======================

    public void StartGame()
    {
        SceneManager.LoadScene("Level1Scene"); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
    
}
