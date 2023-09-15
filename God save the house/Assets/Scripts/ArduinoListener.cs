using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoListener : MonoBehaviour
{
    private bool _isDeviceConnected = false;
    public bool IsDeviceConnected => _isDeviceConnected;
    public Action<ControllerValues> ArduinoDataReceived = s => {} ;


    public void OnMessageArrived(string msg)
    {
        ControllerValues values = new ControllerValues(msg);
        ArduinoDataReceived.Invoke(values);
    }

    public void OnConnectionEvent(bool success)
    {
        //Debug.Log(success ? "Device connected" : "Device disconnected");
        _isDeviceConnected = success;
    }
}
