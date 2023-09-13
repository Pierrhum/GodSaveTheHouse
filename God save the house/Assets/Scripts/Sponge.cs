using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Sponge : MonoBehaviour
{
    [Header("Components")]
    public static House TargetHouse;
    public SpriteRenderer CloudSprite;
    public Collider RainCollider;
    public ParticleSystem RainVFX;

    [Header("Parameters")] 
    public List<Sprite> Sprites;
    
    [Header("Debug")]
    public float WaterCapacity;

    public static bool isRaining = false;
    private EventInstance SpongeRefill;
    private EventInstance SpongeRaining;
    private EventInstance RainOnSmth;
    private Coroutine ConsumeCoroutine;
    private Coroutine RefillCoroutine;
    private bool isRefilling = false;
    private int CurrentSprite = 0;

    public float GetLerpWaterCapacity()
    {
        return WaterCapacity / GameManager.Instance.WaterCapacity;
    }

    private void Start()
    {
        WaterCapacity = GameManager.Instance.WaterCapacity;
    }

    public void Move(Vector3 DeltaPos)
    {
        transform.position += DeltaPos;
    }
    public void Move(float PosX)
    {
        transform.position = new Vector2(PosX, transform.position.y);
    }
    public void SetRain(bool Activate)
    {
        isRaining = Activate && WaterCapacity > 0;
        
        if (isRaining)
        {
            if (WaterCapacity > 0)
            {
                SpongeRaining = AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.SpongeRaining);
                if(TargetHouse.State == HouseState.Burning)
                    RainOnSmth = AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.RainOnFire);
                else if(TargetHouse.State == HouseState.Overflowing)
                    RainOnSmth = AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.RainOnWater);
                RainVFX.Play();
                ConsumeCoroutine = StartCoroutine(ConsumeWater());
            } else 
                AudioManager.Instance.PlayOnShotEvent(AudioManager.fmodEvents.SpongeEmpty);
        }
        else
        {
            AudioManager.Instance.StopEvent(SpongeRaining);
            AudioManager.Instance.StopEvent(RainOnSmth);
            RainVFX.Stop();
            StopCoroutine(ConsumeCoroutine);
            if (TargetHouse && TargetHouse.isSavedRange())
                TargetHouse.State = HouseState.Saved;
        }
    }

    public void SetRefill(bool Activate)
    {
        isRefilling = Activate;

        if (isRefilling)
        {
            SpongeRefill = AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.SpongeRefill);
            if(WaterCapacity < GameManager.Instance.WaterCapacity)
                RefillCoroutine = StartCoroutine(RefillWater());
        }
        else
        {
            StopCoroutine(RefillCoroutine);
            AudioManager.Instance.StopEvent(SpongeRefill);
        }
    }

    private void SetSprite(int SpriteID)
    {
        if (SpriteID != CurrentSprite)
        {
            CurrentSprite = SpriteID;
            CloudSprite.sprite = Sprites[CurrentSprite];
        }
    }

    private IEnumerator ConsumeWater()
    {
        while (WaterCapacity > 0)
        {
            WaterCapacity -= Time.deltaTime;
            float Lerp = GetLerpWaterCapacity();
            
            if (Lerp > .6f) SetSprite(0);
            else if (Lerp > .3f) SetSprite(1);
            else SetSprite(2);
            AudioManager.Instance.SetGlobalParameter("RainIntensity", (1 - Lerp));
            AudioManager.Instance.SetGlobalParameter("CloudPosition", .5f);
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
        AudioManager.Instance.PlayOnShotEvent(AudioManager.fmodEvents.SpongeEmpty);
        SetRain(false);
    }
    
    private IEnumerator RefillWater()
    {
        while (WaterCapacity < GameManager.Instance.WaterCapacity)
        {
            WaterCapacity += Time.deltaTime;
            float Lerp = GetLerpWaterCapacity();
            
            if (Lerp > .6f) SetSprite(0);
            else if (Lerp > .3f) SetSprite(1);
            else SetSprite(2);
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
        AudioManager.Instance.PlayOnShotEvent(AudioManager.fmodEvents.SpongeFull);
        SetRefill(false);
    }
}
