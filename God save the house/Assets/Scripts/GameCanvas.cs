using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private GameObject Victory;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject MainMenu;

    private void Start()
    {
        Victory.SetActive(false);
        GameOver.SetActive(false);
    }
    
    public void HideMainMenu()
    {
        MainMenu.SetActive(false);
        GameManager.Instance.MainMenuVisible = false;
        GameManager.Instance.Houses.ForEach(house => house.StartBurning());
    }

    public void VictoryScreen()
    {
        Victory.SetActive(true);
    }

    public void GameOverScreen()
    {
        GameOver.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
