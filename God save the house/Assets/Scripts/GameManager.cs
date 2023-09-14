using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("UI")] 
    public GameCanvas UI;
    public bool MainMenuVisible = false;

    [Header("Sponge")] 
    public float StartWaterCapacity = 3f;
    public float MaxWaterCapacity = 3f;
    public float WaterRefill = 1f;

    [Header("Houses")] 
    [SerializeField] private int MinHouseToSave = 1;
    [SerializeField] private House LeftHouse;
    [SerializeField] private House RightHouse;
    public Material FloodMaterial;
    public float BurningTime = 3f;
    public float OverflowTime = 6f;
    public float OverflowLimit = 2f;
    public float SaveTime = 2f;
    public List<GameObject> FireParticles;
    public GameObject SmokeParticle;
    public GameObject ExplosionParticle;

    [Header("Debug")] 
    public float PlayerSpeed = 1f;
    
    [System.NonSerialized] public Sponge Sponge;
    [System.NonSerialized] public List<House> Houses = new List<House>();
    private int HousesDead = 0;
    private int HousesSaved = 0;
    private float HousesDistance;
    
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

    private void Start()
    {
        HousesDistance = Vector3.Distance(LeftHouse.transform.position, RightHouse.transform.position);
        if (!MainMenuVisible)
        {
            PlayerController.CanPlay = true;
            Houses.ForEach(house => house.StartBurning());
            UI.HideMainMenu();
        }
    }

    public GameObject GetRandomFire()
    {
        int random = Random.Range(0, FireParticles.Count);
        return FireParticles[random];
    }

    public float GetSpongePosition(float LerpValue)
    {
        return LeftHouse.transform.position.x + LerpValue * HousesDistance;
    }

    public void HouseEnd(bool Saved)
    {
        if (!Saved) HousesDead++;
        else HousesSaved++;

        if (HousesDead + HousesSaved == Houses.Count)
        {
            if(HousesSaved >= MinHouseToSave)  Victory();
            else GameOver();
        }
    }

    public void Pause(bool On)
    {

        if (On)
        {
            AudioManager.Instance.GamePaused();
            Time.timeScale = 0;
        }
        else
        {
            AudioManager.Instance.GameUnpaused();
            Time.timeScale = 1;
        }
        
        PlayerController.CanPlay = !On;
    }
    private void GameOver()
    {
        UI.GameOverScreen();
    }

    private void Victory()
    {
        UI.VictoryScreen();
    }
}
