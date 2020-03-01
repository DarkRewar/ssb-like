using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class DeviceDetection : MonoBehaviour
{
    public List<InputDevice> CurrentDevices = new List<InputDevice>();

    public List<PlayerBehaviour> NonBindedPlayers;

    // Use this for initialization
    void Start()
    {
        //var myAction = new InputAction(binding: "/*/<button>");
        //myAction.performed += OnAnyButtonPressed;
        //myAction.Enable();
    }

    private void OnAnyButtonPressed(InputAction.CallbackContext obj)
    {
        //Debug.Log(obj);
    }

    // Update is called once per frame
    void Update()
    {
        if (NonBindedPlayers.Count == 0) return;

        foreach(var g in Gamepad.all)
        {
            if (g.wasUpdatedThisFrame && !CurrentDevices.Contains(g.device))
            {
                CurrentDevices.Add(g.device);
                NonBindedPlayers[0].BindDevice(g.device);
                NonBindedPlayers.RemoveAt(0);
            }
        }

        if (Keyboard.current.anyKey.isPressed && !CurrentDevices.Contains(Keyboard.current.device))
        {
            CurrentDevices.Add(Keyboard.current.device);
            NonBindedPlayers[0].BindDevice(Keyboard.current.device);
            NonBindedPlayers.RemoveAt(0);
        }
    }
}
