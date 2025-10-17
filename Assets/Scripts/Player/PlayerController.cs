using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;

    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;

    public float drag = 0.1f;

    private PlayerLocomotions playerLocomotions;

    private void Awake()
    {
        playerLocomotions = GetComponent<PlayerLocomotions>();

    }

    private void Update() 
    {
        Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightxz = new Vector3(playerCamera.transform.right.x, 0f, playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraRightxz * playerLocomotions.MovementControls.x + cameraForwardXZ * playerLocomotions.MovementControls.y;


        Vector3 movementDelta = movementDirection * runAcceleration * Time.deltaTime;
        Vector3 newVelocity = characterController.velocity + movementDelta;


        // Add dreg to player

        Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
        newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
        newVelocity = Vector3.ClampMagnitude(newVelocity, runSpeed);


        //Move Character
        characterController.Move(newVelocity * Time.deltaTime);
    }

}
