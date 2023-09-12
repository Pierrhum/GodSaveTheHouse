using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponge : MonoBehaviour
{
    public static House TargetHouse;
    public Collider RainCollider;
    [System.NonSerialized] public float WaterCapacity;
    
    private MeshRenderer Mesh;
    private Coroutine ConsumeCoroutine;
    private bool isRaining = false;

    private void Awake()
    {
        Mesh = GetComponent<MeshRenderer>();
        RainCollider.enabled = false;
        WaterCapacity = GameManager.Instance.WaterCapacity;
    }

    public void Move(Vector3 DeltaPos)
    {
        transform.position += DeltaPos;
    }
    public void ToggleRain()
    {
        isRaining = !isRaining;
        RainCollider.enabled = isRaining && WaterCapacity > 0;
        
        if (WaterCapacity > 0)
        {
            if (isRaining)
            {
                ConsumeCoroutine = StartCoroutine(ConsumeWater());
            }
            else
            {
                StopCoroutine(ConsumeCoroutine);
                Mesh.material.color = Color.grey; // Rain VFX
                if (TargetHouse && TargetHouse.isSavedRange())
                    TargetHouse.State = HouseState.Saved;
            }
        }
        
    }

    private IEnumerator ConsumeWater()
    {
        while (WaterCapacity > 0)
        {
            Mesh.material.color = Color.Lerp(Color.grey, Color.blue, WaterCapacity / GameManager.Instance.WaterCapacity); // Rain VFX
            WaterCapacity -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        ToggleRain();
    }
}
