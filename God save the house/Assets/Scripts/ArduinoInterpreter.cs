using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoInterpreter : MonoBehaviour
{
   [SerializeField] private PlayerController playerController;
   [SerializeField] private ArduinoListener listener;

   private void Start()
   {
      listener.ArduinoDataReceived += ArduinoToPlayer;
   }

   private void ArduinoToPlayer(ControllerValues values)
   {
      playerController.RefillWater(values.LakeButtonIsPressed);
      playerController.Press(values.PressureButtonIsPressed);
      playerController.Move(values.HandPositionFromCaptor);
   }
}
