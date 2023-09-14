using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Sponge sponge;
    public static bool CanPlay = false;

    private Vector2 inputMovement;
    private float lerp;
    private const float maxDistance = 50f;
    private const float minDistance = 15f;
    

    public void Move(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>() * GameManager.Instance.PlayerSpeed * 0.01f;
    }

    public void SetPlayerPosition(float LerpValue)
    {
        sponge.Move(GameManager.Instance.GetSpongePosition(LerpValue));
        AudioManager.Instance.SetGlobalParameter("CloudPosition", LerpValue);
        Debug.Log(LerpValue.ToString());
    }
    public void Move(float position)
    {
        if (position > maxDistance + 10)
        {
            return;
        }
        if (position > maxDistance)
        {
            position = maxDistance;
        }
        else if (position < minDistance)
        {
            position = minDistance;
        }
       

        float LerpValue = (position - minDistance) / (maxDistance - minDistance);
        SetPlayerPosition(LerpValue);
    }
    public void Press(InputAction.CallbackContext context)
    {
        if(context.started && Sponge.isRefilling)
            GameManager.Instance.Pause(CanPlay);
            
        if (GameManager.Instance.MainMenuVisible && context.started)
        {
            CanPlay = true;
            GameManager.Instance.UI.HideMainMenu();
        }
        else
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
    }
    
    public void Press(bool press)
    {
        if (press)
        {
            if (GameManager.Instance.MainMenuVisible)
            {
                CanPlay = true;
                GameManager.Instance.UI.HideMainMenu();
            }
            else if(Sponge.isRefilling)
                GameManager.Instance.Pause(CanPlay);
            else
            {
                sponge.SetRain(true);
                AudioManager.Instance.PlayOnShotEvent(AudioManager.fmodEvents.SpongePressed);
            }
        }
        else
        {
            sponge.SetRain(false);
        }
    }

    
    public void RefillWater(InputAction.CallbackContext context)
    {
        if (context.canceled || context.started)
        {
            sponge.SetRefill(context.started);
        }
    }
    public void RefillWater(bool refilling)
    {
        sponge.SetRefill(refilling);
    }
    private void Update()
    {
        //sponge.Move(inputMovement);
        if ((lerp >= 0 && inputMovement.x < 0) || (lerp < 1 && inputMovement.x > 0))
        {
            lerp += inputMovement.x;
            SetPlayerPosition(lerp);
        }

    }
}
