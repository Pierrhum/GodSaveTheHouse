using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponge : MonoBehaviour
{
    public Collider RainCollider;
    private MeshRenderer Mesh;
    private bool isRaining = false;

    private void Awake()
    {
        Mesh = GetComponent<MeshRenderer>();
        RainCollider.enabled = false;
    }

    public void Move(Vector3 DeltaPos)
    {
        transform.position += DeltaPos;
    }
    public void ToggleRain()
    {
        isRaining = !isRaining;
        RainCollider.enabled = isRaining;
        
        if (isRaining)
            Mesh.material.color = Color.blue;
        else
            Mesh.material.color = Color.grey;
    }
}
