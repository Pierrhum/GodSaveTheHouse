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
    public HousePosition Position;
    public List<Room> Rooms;
    //[System.NonSerialized] 
    public float burnTimer = 0f;
    //[System.NonSerialized] 
    public float waterTimer = 0f;
    [System.NonSerialized] public HouseState State = HouseState.Burning;
    
    private int CurrentRoom = -1;
    private EventInstance BurningSFX;
    private EventInstance FireLevelSFX;

    private void Start()
    {
        Rooms.ForEach(R => R.Position = Position == HousePosition.L ? "L" : Position == HousePosition.R ? "R" : "C");
        StartCoroutine(BurnCoroutine(2));
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
        if (State == HouseState.Overflowing) StartCoroutine(OverflowCoroutine());
        if (State == HouseState.Saved) Debug.Log(("Saved"));
    }

    private IEnumerator OverflowCoroutine()
    {
        while (State == HouseState.Overflowing)
        {
            if(waterTimer > GameManager.Instance.SaveTime + GameManager.Instance.OverflowTime)
                State = HouseState.Drown;
            else
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.blue, waterTimer / (GameManager.Instance.SaveTime + GameManager.Instance.OverflowTime));
                waterTimer += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Sponge.TargetHouse = this;
        AudioManager.Instance.PlayEvent(AudioManager.fmodEvents.HouseSelected[(int)Position]);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Sponge.isRaining && CurrentRoom >= 0)
        {
            waterTimer += Time.deltaTime;
            if(waterTimer >= GameManager.Instance.SaveTime)
                Rooms[CurrentRoom].EndBurning(true);
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
        }
    }
}
