using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float locomotionBlendSpeed=0.02f;

    private PlayerLocomotions playerLocomotionInput;
    private PlayerState playerState;


    private static int inputXHash = Animator.StringToHash("inputX");
    private static int inputYHash = Animator.StringToHash("inputY");

    private static int inputMagnitudeHash=Animator.StringToHash("inputMagnitude");

    private static int isGroundedHash=Animator.StringToHash("isGrounded");
    private static int isFallingHash=Animator.StringToHash("isFalling");
    private static int isJumpingHash=Animator.StringToHash("isJumping");

    private Vector3 currentBlendInput=Vector3.zero;

    private void Awake()
    {
        playerLocomotionInput = GetComponent<PlayerLocomotions>();
        playerState=GetComponent<PlayerState>();

    }

    private void Update()
    {
        UpdateAnimationState();
        
    }

    private void UpdateAnimationState()
    {
        bool isidling= playerState.CurrentPlayerMovementState ==PlayerMovementState. Idling;
        bool iskunning=playerState.CurrentPlayerMovementState ==PlayerMovementState.Running;
        bool isSprinting =playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
        bool isJumping= playerState.CurrentPlayerMovementState ==PlayerMovementState.Jumping;
        bool isFalling=playerState.CurrentPlayerMovementState== PlayerMovementState.Falling;
        bool isGrounded= playerState.InGroundedState();
        
      

        
        Vector2 inputTarget =isSprinting? playerLocomotionInput.MovementControls*1.5f: playerLocomotionInput.MovementControls;
        currentBlendInput=Vector3.Lerp(currentBlendInput,inputTarget,locomotionBlendSpeed*Time.deltaTime);


        animator.SetBool(isGroundedHash, isGrounded);
        animator.SetBool(isFallingHash, isFalling);
        animator.SetBool(isJumpingHash, isJumping);


        animator.SetFloat(inputXHash, inputTarget.x);
        animator.SetFloat(inputYHash, inputTarget.y);
        animator.SetFloat(inputMagnitudeHash, currentBlendInput.magnitude);
    }

}
