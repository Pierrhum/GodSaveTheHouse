using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject BurnSprite;
    public Transform FireSpawnPoint;

    [System.NonSerialized] public string Position;
    private ParticleSystem Smoke;
    private GameObject Fire;

    private bool doOnceHouseSafe = true;

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
        else if (doOnceHouseSafe)
        {
            doOnceHouseSafe = false;
            AudioManager.Instance.PlayOnShotEvent(AudioManager.fmodEvents.HouseSafe);
        }
    }

    public void StartBurning()
    {
        Instantiate(GameManager.Instance.ExplosionParticle, FireSpawnPoint);
        Fire.SetActive(true);
        Smoke.Play();
        EventInstance sfx = AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.Explosion);
        sfx.setParameterByNameWithLabel("HousePosition", Position);
    }
}
