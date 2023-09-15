using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Sponge sponge;
    public static bool CanPlay = false;
    private Vector2 inputMovement;
    private float lerp;
    public const float maxDistance = 50f;
    public const float minDistance = 15f;
    private int buttonSelected = -1;

    public void OpenFirstLevel(InputAction.CallbackContext context)
    {
        if (context.started)
            SceneManager.LoadScene("Level_1");
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>() * GameManager.Instance.PlayerSpeed * 0.01f;
    }

    public void SetPlayerPosition(float LerpValue)
    {
        sponge.Move(GameManager.Instance.GetSpongePosition(LerpValue));
        AudioManager.Instance.SetGlobalParameter("CloudPosition", LerpValue);
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
        if (context.started)
            Press(true);
        else if (context.canceled)
            Press(false);
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

    public void SelectMenuButton(ControllerValues values)
    {
        if (GameManager.Instance.UI.GameOver.activeSelf)
        {
            if (values.HandPositionFromCaptor < ((maxDistance -minDistance) / 2f + minDistance) && buttonSelected != 1)
            {
                GameManager.Instance.UI.SetHoverRetryButtonGO();
                buttonSelected = 1;
            }
            else if (values.HandPositionFromCaptor > ((maxDistance -minDistance) / 2f + minDistance) && buttonSelected!= 2)
            {
                GameManager.Instance.UI.SetHoverQuitButtonGO();
                buttonSelected = 2;
            }
        }
        else
        {
            if (values.HandPositionFromCaptor < ((maxDistance -minDistance) / 3f + minDistance) && buttonSelected != 0)
            {
                GameManager.Instance.UI.SetHoverLevelButtonV();
                buttonSelected = 0;
            }
            else if (values.HandPositionFromCaptor is > (((maxDistance -minDistance) / 3f) + minDistance) and < (((maxDistance -minDistance) / 3f)*2f + minDistance) && buttonSelected!= 1)
            {
                GameManager.Instance.UI.SetHoverRetryButtonV();
                buttonSelected = 1;
            }
            else if (values.HandPositionFromCaptor > (((maxDistance -minDistance) / 3f)*2f + minDistance) && buttonSelected!= 2)
            {
                GameManager.Instance.UI.SetHoverQuitButtonV();
                buttonSelected = 2;
            }
        }
    
        if (values.PressureButtonIsPressed)
        {
            switch (buttonSelected)
            {
                case 0:
                    GameManager.Instance.UI.NextLevel();
                    break;
                case 1:
                    GameManager.Instance.UI.Retry();
                    break;
                case 2:
                    GameManager.Instance.UI.Quit();
                    break;
                default:
                    Debug.Log("should not happen");
                    break;
            }
            buttonSelected = -1;
        }
      
    }
}
