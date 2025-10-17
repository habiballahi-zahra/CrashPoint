using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotions : MonoBehaviour,PlayerControls.IPlayerLocomotionMapActions
{
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MovementControls{get; private set;}


    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.PlayerLocomotionMap.Enable();
        //  Debug.Log("enabaled");
        PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        Debug.Log("PlayerControls enabled");

    }


    private void OnDisable()
        {
        PlayerControls.PlayerLocomotionMap.Disable();
        PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }


    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementControls = context.ReadValue<Vector2>();
        Debug.Log(MovementControls);
        print(MovementControls);
    }
}