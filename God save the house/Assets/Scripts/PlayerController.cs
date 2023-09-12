using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float WaterCapacity;
    public Sponge sponge;

    private Vector2 inputMovement;

    public void Move(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>() * 0.01f;
    }
    
    public void Press(InputAction.CallbackContext context)
    {
        if(context.canceled || context.started)
            sponge.ToggleRain();
    }

    private void Update()
    {
        sponge.Move(inputMovement);
    }
}
