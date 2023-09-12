using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")] 
    public float PlayerSpeed = 1f;
    
    [Header("Houses")] 
    public float BurningTime = 18f;
    public float OverflowLimit = 1f;

    [System.NonSerialized] public Sponge Sponge;
    // Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("GameManager is null");
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null) _instance = this;
    }
}
