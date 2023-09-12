using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HouseState { Normal, Burnt, Overflowed, Saved }
public class House : MonoBehaviour
{
    [System.NonSerialized] public float lifeTimer = 0f;
    [System.NonSerialized] public HouseState State = HouseState.Normal;
    private MeshRenderer Mesh;
    
    private void Awake()
    {
        Mesh = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        StartCoroutine(HouseCoroutine(1));
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
            else if (lifeTimer < GameManager.Instance.BurningTime)
            {
                lifeTimer += Time.deltaTime;
                Mesh.material.color = Color.Lerp(Color.grey, Color.red, lifeTimer / GameManager.Instance.BurningTime);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            // Burnt
            else
                State = HouseState.Burnt;
        }

        // Overflowed : Game Over
        while (State == HouseState.Overflowed && lifeTimer < -GameManager.Instance.OverflowLimit)
        {
            lifeTimer -= Time.deltaTime / 5f;
            Mesh.material.color = Color.Lerp(Color.grey, Color.blue, (lifeTimer / -3));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        // Temporary
        if(State == HouseState.Burnt) Mesh.material.color = Color.red;
        if(State == HouseState.Saved) Mesh.material.color = Color.grey;
        if(State == HouseState.Overflowed) Mesh.material.color = Color.blue;
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
