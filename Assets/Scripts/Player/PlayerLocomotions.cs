using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

 [DefaultExecutionOrder(-2)]

public class PlayerLocomotions : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
{
#region  class variable

     [SerializeField] private bool holdToSprint=true;



    public bool jumpPressed{get; private set;}

    public bool SprintToggledOn{get; private set;}
    public PlayerControls PlayerControls { get; private set; }
    public Vector2 MovementControls { get; private set; }

    public Vector2 LookInput { get; private set; }

#endregion
   
 #region Startup  
   
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

#endregion

#region Late update
    private void LateUpdate()
        {
            jumpPressed=false;
        }

#endregion

#region Input CallBack
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

    public void OnNewaction(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnToggleSprint(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            SprintToggledOn=holdToSprint||!SprintToggledOn;

        }

        else if(context.canceled)
        {
            SprintToggledOn=!holdToSprint && SprintToggledOn;

        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed)
        {
            return;
        }
        jumpPressed=true;
    }
#endregion
}