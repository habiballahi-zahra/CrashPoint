using JetBrains.Annotations;
using UnityEngine;

 [DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{   [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;

    [Header("BaseMovement")]
    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;

    public float drag = 0.1f;

    [Header("CameraSettings")]

    public float LookSenseH = 0.1f;
    public float LookSenseV = 0.1f;
    public float LookLimitV = 89f;



    private PlayerLocomotions playerLocomotions;
    private Vector2 cameraRotation = Vector2.zero;
    private Vector2 playerTargetRotation = Vector2.zero;

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

    private void LateUpdate()
    {

        cameraRotation.x += LookSenseH * playerLocomotions.LookInput.x;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y - LookSenseV * playerLocomotions.LookInput.y, -LookLimitV, LookLimitV);


        playerTargetRotation.x += transform.eulerAngles.x +LookSenseH * playerLocomotions.LookInput.x;
        transform.rotation = Quaternion.Euler(0f, playerTargetRotation.x, 0f);

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0f);

    }

}
