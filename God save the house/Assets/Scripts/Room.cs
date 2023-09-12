using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject BurnSprite;
    public Transform FireSpawnPoint;
    
    private ParticleSystem Smoke;
    private GameObject Fire;

    private void Start()
    {
        GameObject Fire_go = Instantiate(GameManager.Instance.GetRandomFire(), FireSpawnPoint);
        GameObject Smoke_go = Instantiate(GameManager.Instance.SmokeParticle, FireSpawnPoint);
        
        Fire = Fire_go;
        Fire.SetActive(false);
        Smoke = Smoke_go.GetComponentInChildren<ParticleSystem>();
        BurnSprite.SetActive(false);
    }

    public void EndBurning(bool Saved)
    {
        Fire.SetActive(false);
        Smoke.Stop();
        if(!Saved)
            BurnSprite.SetActive(true);
    }

    public void StartBurning()
    {
        Instantiate(GameManager.Instance.ExplosionParticle, FireSpawnPoint);
        Fire.SetActive(true);
        Smoke.Play();
    }
}
