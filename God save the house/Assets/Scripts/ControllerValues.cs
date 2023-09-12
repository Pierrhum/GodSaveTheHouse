using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ControllerValues
{
   /// <summary>
   /// Distance at which hand of user is
   /// </summary>
   public static float HandPositionFromCaptor;
   /// <summary>
   /// Pressure applied by user
   /// Currently, the higher this number is, the lower the pressure
   /// </summary>
   public static float PressureButtonValue;
   /// <summary>
   /// User is pressing pressure button
   /// </summary>
   public static int PressureButtonIsPressed;
   /// <summary>
   /// Lake button is pressed
   /// </summary>
   public static int LakeButtonIsPressed;
   public ControllerValues(string msg)
   {
      string[] message = msg.Split(";");
      HandPositionFromCaptor = float.Parse(message[0],CultureInfo.InvariantCulture.NumberFormat);
      PressureButtonValue = float.Parse(message[1],CultureInfo.InvariantCulture.NumberFormat);
      PressureButtonIsPressed = int.Parse(message[2]);
      LakeButtonIsPressed = int.Parse(message[3]);
   }
}
