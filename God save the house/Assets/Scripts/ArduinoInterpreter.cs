using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoInterpreter : MonoBehaviour
{
   [SerializeField] private PlayerController playerController;
   [SerializeField] private ArduinoListener listener;

   private bool prevValueLake = false;
   private bool prevValuePressure = false;
   
   private void Start()
   {
      listener.ArduinoDataReceived += ArduinoToPlayer;
   }

   private void ArduinoToPlayer(ControllerValues values)
   {
      if (!values.LakeButtonIsPressed || !values.PressureButtonIsPressed)
      {
         if (prevValueLake != values.LakeButtonIsPressed)
         {
            playerController.RefillWater(values.LakeButtonIsPressed);
            prevValueLake = values.LakeButtonIsPressed;
         }

         if (prevValuePressure != values.PressureButtonIsPressed)
         {
            playerController.Press(values.PressureButtonIsPressed);
            prevValuePressure = values.PressureButtonIsPressed;
         }
      }
      else
      {
         //Pause or start
      }

      if (values.HandPositionFromCaptor < 95)
      {
         playerController.Move(values.HandPositionFromCaptor);
      }
   }
}
