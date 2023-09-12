using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HouseState { Normal, Burning, Overflowing, Saved, Burnt, Drown }
public class House : MonoBehaviour
{
    public List<Room> Rooms;
    //[System.NonSerialized] 
    public float burnTimer = 0f;
    //[System.NonSerialized] 
    public float waterTimer = 0f;
    [System.NonSerialized] public HouseState State = HouseState.Normal;
    
    private int CurrentRoom = -1;

    private void Start()
    {
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

        // Burning
        while (State == HouseState.Normal)
        {
            // Starts overflow
            if (isOverflowedRange())
                State = HouseState.Overflowing;
            // Still burning
            else
            {
                burnTimer += Time.deltaTime;
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

    private void OnTriggerStay(Collider other)
    {
        if (CurrentRoom >= 0)
        {
            Sponge.TargetHouse = this;
            waterTimer += Time.deltaTime;
            if(waterTimer >= GameManager.Instance.SaveTime)
                Rooms[CurrentRoom].EndBurning(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CurrentRoom >= 0)
        {
            if (waterTimer <= 0)
                if (isOverflowedRange()) State = HouseState.Overflowing;
                else State = HouseState.Saved;
            Sponge.TargetHouse = null;
        }
    }
}
