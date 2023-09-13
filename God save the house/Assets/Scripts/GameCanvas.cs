using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private GameObject Victory;
    [SerializeField] private GameObject GameOver;

    private void Start()
    {
        Victory.SetActive(false);
        GameOver.SetActive(false);
    }

    public void VictoryScreen()
    {
        Victory.SetActive(true);
    }

    public void GameOverScreen()
    {
        GameOver.SetActive(true);
    }
}
