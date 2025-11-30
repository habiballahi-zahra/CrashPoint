using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour


#region Class variables


{   [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;

    [Header("BaseMovement")]
    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;
    public float sprintAcceleration=0.5f;
    public float sprintSpeed=0.7f;
    public float drag = 0.1f;
    
    public float gravity=25f;
    public float jumpSpeed=1.0f;
    public float movingThreshold=0.01f;

    [Header("CameraSettings")]

    public float LookSenseH = 0.1f;
    public float LookSenseV = 0.1f;
    public float LookLimitV = 89f;



    private PlayerLocomotions playerLocomotions;
    private PlayerState playerState;

    private Vector2 cameraRotation = Vector2.zero;
    private Vector2 playerTargetRotation = Vector2.zero;

    private float verticalVelocity=0f;

#endregion
  
  
#region  Statrup
    private void Awake()
    {
        playerLocomotions = GetComponent<PlayerLocomotions>();
        playerState=GetComponent<PlayerState>();
        

    }
#endregion
   
 #region Update Logic  
    private void Update()
    {
        UpdateMovementState();
        HandleVerticalMovement();
        HandleLateralMovement();
       
    }

    private void HandleVerticalMovement()
    {
        bool isGrounded= playerState.InGroundedState();
        if(isGrounded && verticalVelocity<0)
            verticalVelocity=0f;
        verticalVelocity-=gravity*Time.deltaTime;
        if(playerLocomotions.jumpPressed && isGrounded)
        {
            verticalVelocity +=Mathf.Sqrt(jumpSpeed*3*gravity);
        }
    }

     private void HandleLateralMovement()
    {
        //Create quick refrences for current state
        bool isSprinting=playerState.CurrentPlayerMovementState==PlayerMovementState.Sprinting;
        bool isGrounded=playerState.InGroundedState();

        //State dependent acceleration and speed
        float lateralAcceleration=isSprinting?sprintAcceleration:runAcceleration;
        float clampLateralMagnitude=isSprinting?sprintSpeed:runSpeed;
 
        Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightxz = new Vector3(playerCamera.transform.right.x, 0f, playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraRightxz * playerLocomotions.MovementControls.x + cameraForwardXZ * playerLocomotions.MovementControls.y;


        Vector3 movementDelta = movementDirection *lateralAcceleration;
        Vector3 newVelocity = characterController.velocity + movementDelta;


        // Add dreg to player

        Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
        newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
        newVelocity = Vector3.ClampMagnitude(newVelocity, clampLateralMagnitude);
        newVelocity.y += verticalVelocity;


        //Move Character
        characterController.Move(newVelocity * Time.deltaTime);

    }

    private void UpdateMovementState()
    {
        bool isMovementInput=playerLocomotions.MovementControls !=Vector2.zero;   //order
        bool isMovingLaterally=IsMovingLaterally();                               //matter
        bool isSprinting=playerLocomotions.SprintToggledOn && isMovingLaterally;  //order matter
        bool isGrounded=IsGrounded();


        PlayerMovementState lateralState= isSprinting?PlayerMovementState.Sprinting:
                                            isMovingLaterally || isMovementInput ?PlayerMovementState.Running :PlayerMovementState.Idling;
        playerState.SetPlayerMovementState(lateralState);
   
        //Control Airborn Status
        if(!isGrounded && characterController.velocity.y>=0f)
        {
            playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
        }
        else if(!isGrounded && characterController.velocity.y<0f )
        {
            playerState.SetPlayerMovementState(PlayerMovementState.Falling);
        }

   
    }
#endregion
 
#region  Late update 

    private void LateUpdate()
    {

        cameraRotation.x += LookSenseH * playerLocomotions.LookInput.x;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y - LookSenseV * playerLocomotions.LookInput.y, -LookLimitV, LookLimitV);


        playerTargetRotation.x += transform.eulerAngles.x +LookSenseH * playerLocomotions.LookInput.x;
        transform.rotation = Quaternion.Euler(0f, playerTargetRotation.x, 0f);

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0f);

    }
private bool IsGrounded()
    {
        return characterController.isGrounded;
    }
     
#endregion

#region  State Checks
    private bool IsMovingLaterally()
    {
       Vector3 lateralVelocity=new Vector3(characterController.velocity.x,0f,characterController.velocity.y);
       return lateralVelocity.magnitude>movingThreshold;
    }

#endregion
}
