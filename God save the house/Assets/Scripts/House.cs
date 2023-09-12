using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HouseState { Normal, Burnt, Overflowed, Saved }
public class House : MonoBehaviour
{
    public List<Room> Rooms;
    [System.NonSerialized] public float lifeTimer = 0f;
    [System.NonSerialized] public HouseState State = HouseState.Normal;
    
    private int CurrentRoom = -1;
    private float RoomBurningTime;

    private void Start()
    {
        RoomBurningTime = GameManager.Instance.BurningTime / Rooms.Count;
        StartCoroutine(HouseCoroutine(2));
    }

    public bool isSavedRange()
    {
        return lifeTimer < 0 && lifeTimer >= -GameManager.Instance.OverflowLimit;
    }

    private IEnumerator HouseCoroutine(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        // Burning
        while (State == HouseState.Normal)
        {
            // Starts overflow
            if (lifeTimer <= -GameManager.Instance.OverflowLimit)
                State = HouseState.Overflowed;
            // Still burning
                lifeTimer += Time.deltaTime;
                if (lifeTimer > (CurrentRoom+1) * RoomBurningTime)
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

        // Overflowed : Game Over
        while (State == HouseState.Overflowed && lifeTimer < -GameManager.Instance.OverflowLimit)
        {
            lifeTimer -= Time.deltaTime / 5f;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Sponge.TargetHouse = this;
    }

    private void OnTriggerStay(Collider other)
    {
        lifeTimer -= 4 * Time.deltaTime;
        if (lifeTimer < 0)
            Debug.Log(lifeTimer.ToString());
    }

    private void OnTriggerExit(Collider other)
    {
        if (lifeTimer <= 0)
            if(lifeTimer < -GameManager.Instance.OverflowLimit) State = HouseState.Overflowed; 
            else State = HouseState.Saved;
        Debug.Log(((int)State).ToString());
        Sponge.TargetHouse = null;
    }
}
