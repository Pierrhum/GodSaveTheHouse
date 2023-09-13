using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ControllerValues
{
   /// <summary>
   /// Distance at which hand of user is
   /// </summary>
   public float HandPositionFromCaptor;
   /// <summary>
   /// Pressure applied by user
   /// goes from -0. something (uninteresting) to +5. psi max can start count at 0.1
   /// </summary>
   public float PressureButtonValue;
   /// <summary>
   /// User is pressing pressure button
   /// </summary>
   public bool PressureButtonIsPressed;
   /// <summary>
   /// Lake button is pressed
   /// </summary>
   public bool LakeButtonIsPressed;
   public ControllerValues(string msg)
   {
      string[] message = msg.Split(";");
      HandPositionFromCaptor = float.Parse(message[0],CultureInfo.InvariantCulture.NumberFormat);
      PressureButtonValue = float.Parse(message[1],CultureInfo.InvariantCulture.NumberFormat);
      PressureButtonIsPressed = int.Parse(message[2]) != 0;
      LakeButtonIsPressed = int.Parse(message[3]) != 0;
   }
}
