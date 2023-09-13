using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Sponge")] 
    public float WaterCapacity = 3f;
    public float WaterRefill = 1f;

    [Header("Houses")] 
    [SerializeField] private House LeftHouse;
    [SerializeField] private House RightHouse;
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
}
