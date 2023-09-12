using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Sponge")] 
    public float WaterCapacity = 6f;
    public float WaterRefill = 3.6f;
    
    [Header("Houses")] 
    public float BurningTime = 18f;
    public float OverflowLimit = 1f;
    public List<GameObject> FireParticles;
    public GameObject SmokeParticle;
    public GameObject ExplosionParticle;

    [Header("Debug")] 
    public float PlayerSpeed = 1f;
    
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

    public GameObject GetRandomFire()
    {
        int random = Random.Range(0, FireParticles.Count);
        return FireParticles[random];
    }
}
