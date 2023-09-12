using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ControllerValues
{
   public static float HandPositionFromCaptor;
   public static float PressureButtonValue;
   public static int PressureButtonIsPressed;
   public ControllerValues(string msg)
   {
      string[] message = msg.Split(";");
      HandPositionFromCaptor = float.Parse(message[0],CultureInfo.InvariantCulture.NumberFormat);
      PressureButtonValue = float.Parse(message[1],CultureInfo.InvariantCulture.NumberFormat);
      PressureButtonIsPressed = int.Parse(message[2]);
   }
}
