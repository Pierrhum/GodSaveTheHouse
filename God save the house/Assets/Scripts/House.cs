using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HouseState { Normal, Burnt, Overflowed, Saved }
public class House : MonoBehaviour
{
    [System.NonSerialized] public float lifeState = 0f;
    private MeshRenderer Mesh;
    private HouseState State = HouseState.Normal;
    
    private void Awake()
    {
        Mesh = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        StartCoroutine(HouseCoroutine(1, 10));
    }

    private IEnumerator HouseCoroutine(float startDelay, float burningTime)
    {
        yield return new WaitForSeconds(startDelay);

        // Burning
        while (State == HouseState.Normal)
        {
            // Starts overflow
            if (lifeState <= -.2f)
                State = HouseState.Overflowed;
            // Still burning
            else if (lifeState < 1)
            {
                lifeState += Time.deltaTime / burningTime;
                Mesh.material.color = Color.Lerp(Color.grey, Color.red, lifeState);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            // Burnt
            else
                State = HouseState.Burnt;
        }

        // Overflowed
        while (State == HouseState.Overflowed && lifeState < -0.2)
        {
            Debug.Log("Overflowed");
            lifeState -= Time.deltaTime / 5f;
            Mesh.material.color = Color.Lerp(Color.grey, Color.blue, (lifeState / -1));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        if(State == HouseState.Burnt) Mesh.material.color = Color.red;
        if(State == HouseState.Saved) Mesh.material.color = Color.grey;
        if(State == HouseState.Overflowed) Mesh.material.color = Color.blue;
    }
    
    private void OnTriggerStay(Collider other)
    {
        lifeState -= 4 * Time.deltaTime / 10;
    }

    private void OnTriggerExit(Collider other)
    {
        if (lifeState <= 0)
            if(lifeState < -0.2) State = HouseState.Overflowed; 
            else State = HouseState.Saved;
        Debug.Log(((int)State).ToString());
    }
}
