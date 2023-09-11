using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponge : MonoBehaviour
{
    private MeshRenderer Mesh;
    private bool isRaining = false;

    private void Awake()
    {
        Mesh = GetComponent<MeshRenderer>();
    }

    public void Move(Vector3 DeltaPos)
    {
        transform.position += DeltaPos;
    }
    public void ToggleRain()
    {
        isRaining = !isRaining;
        if (isRaining)
            Mesh.material.color = Color.red;
        else
            Mesh.material.color = Color.white;
    }
}
