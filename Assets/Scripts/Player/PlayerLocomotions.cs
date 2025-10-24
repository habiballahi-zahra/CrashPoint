using UnityEngine;
using UnityEngine.InputSystem;

 [DefaultExecutionOrder(-2)]

public class PlayerLocomotions : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
{
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MovementControls { get; private set; }

    public Vector2 LookInput { get; private set; }


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

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }
}