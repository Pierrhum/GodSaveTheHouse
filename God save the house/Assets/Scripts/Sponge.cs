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
    public static bool isRefilling = false;
    private EventInstance SpongeRefill;
    private EventInstance SpongeRaining;
    private Coroutine ConsumeCoroutine;
    private Coroutine RefillCoroutine;
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
    public void SetRain(bool activate)
    {
        RainCollider.enabled = activate && WaterCapacity > 0;
        
        if (activate && !isRaining)
        {
            if (WaterCapacity > 0)
            {
                SpongeRaining = AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.SpongeRaining);
                RainVFX.Play();
                ConsumeCoroutine = StartCoroutine(ConsumeWater());
            } else 
                AudioManager.Instance.PlayOnShotEvent(AudioManager.fmodEvents.SpongeEmpty);
        }
        else if (!activate && isRaining)
        {
            AudioManager.Instance.StopEvent(SpongeRaining);
            if(TargetHouse)
                AudioManager.Instance.StopEvent(TargetHouse.RainOnSmth);
            RainVFX.Stop();
            StopCoroutine(ConsumeCoroutine);
            if (TargetHouse && TargetHouse.isSavedRange())
                TargetHouse.State = HouseState.Saved;
        }
        isRaining = activate && WaterCapacity > 0;    
    }

    public void SetRefill(bool activate)
    {
        if (activate && !isRefilling)
        {
            SpongeRefill = AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.SpongeRefill);
                if(WaterCapacity < GameManager.Instance.WaterCapacity)
                    RefillCoroutine = StartCoroutine(RefillWater());
        }
        else if(!activate && isRefilling)
        {
            AudioManager.Instance.StopEvent(SpongeRefill);
            if(RefillCoroutine != null)
                StopCoroutine(RefillCoroutine);
        }
        isRefilling = activate;
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
