using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private PlayerLocomotions playerLocomotionInput;
    private static int inputXHash = Animator.StringToHash("inputX");
    private static int inputYHash = Animator.StringToHash("inputY");

    private void Awake()
    {
        playerLocomotionInput = GetComponent<PlayerLocomotions>();

    }

    private void Update()
    {
        UpdateAnimationState();
        
    }

    private void UpdateAnimationState()
    {

        Vector2 inputTarget = playerLocomotionInput.MovementControls;
        animator.SetFloat(inputXHash, inputTarget.x);
        animator.SetFloat(inputYHash, inputTarget.y);
    }

}
