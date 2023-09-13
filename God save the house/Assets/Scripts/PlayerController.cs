using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Sponge sponge;

    private Vector2 inputMovement;

    public void Move(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>() * GameManager.Instance.PlayerSpeed * 0.01f;
    }
    
    public void Press(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            sponge.SetRain(false);
        }
        else if (context.started)
        {
            sponge.SetRain(true);
            AudioManager.Instance.PlayOnShotEvent(AudioManager.fmodEvents.SpongePressed);
        }
    }
    
    public void RefillWater(InputAction.CallbackContext context)
    {
        if(context.canceled || context.started)
            sponge.SetRefill(context.started);
    }

    private void Update()
    {
        sponge.Move(inputMovement);
    }
}
