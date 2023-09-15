using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public enum HouseState { Burning, Overflowing, Saved, Burnt, Drown }
public enum HousePosition { L, C, R }
public class House : MonoBehaviour
{
    [Header("Parameters")] 
    public bool ShouldBurn = true;
    public HousePosition Position;
    public float DelayBeforeBurning = 1f;
    
    [Header("Components")] 
    public List<Room> Rooms;
    
    [Header("Debug")] 
    public float burnTimer = 0f;
    public float waterTimer = 0f;
    
    [System.NonSerialized] public HouseState State = HouseState.Burning;
    [System.NonSerialized] public  EventInstance RainOnSmth;
    private Material FloodMaterial;
    private EventInstance BurningSFX;
    private EventInstance FireLevelSFX;
    private int CurrentRoom = -1;

    private bool doOnce=true;
    [System.NonSerialized] public bool isRainSFXPlaying=false;

    private void Start()
    {
        if (ShouldBurn)
        {
            FloodMaterial = GetComponent<SpriteRenderer>().material;
            Rooms.ForEach(R => R.Position = Position == HousePosition.L ? "L" : Position == HousePosition.R ? "R" : "C");
            GameManager.Instance.Houses.Add(this);
        }
    }

    public void StartBurning()
    {
        if(ShouldBurn)
            StartCoroutine(BurnCoroutine(DelayBeforeBurning));
    }

    public bool isOverflowedRange()
    {
        return waterTimer > GameManager.Instance.SaveTime + GameManager.Instance.OverflowLimit;
    }
    
    public bool isSavedRange()
    {
        return waterTimer >= GameManager.Instance.SaveTime && !isOverflowedRange();
    }

    private IEnumerator BurnCoroutine(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        BurningSFX = AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.HouseBurning[(int)Position]);
        FireLevelSFX = AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.FireLevel[(int)Position]);

        // Burning
        while (State == HouseState.Burning)
        {
            // Starts overflow
            if (isOverflowedRange())
                State = HouseState.Overflowing;
            // Still burning
            else
            {
                burnTimer += Time.deltaTime;
                BurningSFX.setParameterByName("FireLevel", 2 * burnTimer / (GameManager.Instance.BurningTime * 7));
                FireLevelSFX.setParameterByName("FireLevel", 2 * burnTimer / (GameManager.Instance.BurningTime * 7));
                if (burnTimer >= (CurrentRoom+1) * GameManager.Instance.BurningTime)
                {
                    // First room
                    if(CurrentRoom != -1) Rooms[CurrentRoom].EndBurning(false);
                    CurrentRoom++;
                    // Burnt
                    if(CurrentRoom >= Rooms.Count) State = HouseState.Burnt;
                    else Rooms[CurrentRoom].StartBurning();
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        AudioManager.Instance.StopEvent(BurningSFX);
        AudioManager.Instance.StopEvent(FireLevelSFX);
        
        if (State == HouseState.Saved) GameManager.Instance.HouseEnd(true);
        if(State == HouseState.Burnt) GameManager.Instance.HouseEnd(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (State == HouseState.Burnt || State == HouseState.Drown) return;
        
        Sponge.TargetHouse = this;
        AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.HouseSelected[(int)Position]);
    }

    private void OnTriggerStay(Collider other)
    {
        if (State == HouseState.Burnt || State == HouseState.Drown) return;
        
        if (Sponge.isRaining && CurrentRoom >= 0)
        {
            if(!isRainSFXPlaying)
            {
                if(State == HouseState.Burning)
                    RainOnSmth = AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.RainOnFire[(int)Position]);
                else
                    RainOnSmth = AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.RainOnWater[(int)Position]);
                isRainSFXPlaying = true;
            }
            waterTimer += Time.deltaTime;
            if (waterTimer >= GameManager.Instance.SaveTime)
            {
                if(doOnce)
                {
                    doOnce = false;
                    State = HouseState.Overflowing;
                    isRainSFXPlaying = false;
                    AudioManager.Instance.StopEvent(RainOnSmth);
                }
                Rooms[CurrentRoom].EndBurning(true);
                Flooding((waterTimer - GameManager.Instance.SaveTime) / GameManager.Instance.OverflowLimit);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Sponge.isRaining && CurrentRoom >= 0)
        {
            if (waterTimer <= 0)
                if (isOverflowedRange()) State = HouseState.Overflowing;
                else State = HouseState.Saved;
            Sponge.TargetHouse = null;
            AudioManager.Instance.StopEvent(RainOnSmth);
            isRainSFXPlaying = false;
        }
    }
    
    public void Flooding(float LerpValue)
    {
        if(LerpValue >= 1f) 
        {
            State = HouseState.Drown;
            GameManager.Instance.HouseEnd(false);
            Rooms.ForEach(R => R.Flood(1f));
            FloodMaterial.SetFloat("_MaskIntensity", 1f);
        }
        else
        {
            Rooms.ForEach(R => R.Flood(LerpValue));
            FloodMaterial.SetFloat("_MaskIntensity", LerpValue);
        }
    }
}
