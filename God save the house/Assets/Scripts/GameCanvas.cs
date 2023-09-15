using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private GameObject Victory;
    [SerializeField] public GameObject GameOver;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private Button buttonRetryGO;
    [SerializeField] private Button buttonRetryV;
    [SerializeField] private Button buttonQuitGO;
    [SerializeField] private Button buttonQuitV;
    [SerializeField] private Button buttonLevelV;
    private void Start()
    {
        Victory.SetActive(false);
        GameOver.SetActive(false);
        GameManager.Instance.MainMenuVisible = true;
    }
    
    public void HideMainMenu()
    {
        MainMenu.SetActive(false);
        GameManager.Instance.MainMenuVisible = false;
        GameManager.Instance.Houses.ForEach(house => house.StartBurning());
    }

    public void VictoryScreen()
    {
        GameManager.Instance.EndMenuVisible = true;
        Victory.SetActive(true);
    }

    public void GameOverScreen()
    {
        GameManager.Instance.EndMenuVisible = true;
        GameOver.SetActive(true);
    }

    public void Retry()
    {
        GameManager.Instance.EndMenuVisible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void NextLevel()
    {
        AudioManager.Instance.StopEvents();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetHoverRetryButtonGO()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject (null);
        buttonRetryGO.Select();
        
       
    }

    public void SetHoverRetryButtonV()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject (null);
        buttonRetryV.Select();
    }

    public void SetHoverQuitButtonGO()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject (null);
        buttonQuitGO.Select();
    }
    public void SetHoverQuitButtonV()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject (null);
        buttonQuitV.Select();
    }
    public void SetHoverLevelButtonV()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject (null);
        buttonLevelV.Select();
    }
}
