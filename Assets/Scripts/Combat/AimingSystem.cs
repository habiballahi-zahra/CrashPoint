using UnityEngine;

public class AimingSystem : MonoBehaviour
{
    [Header("Aiming Settings")]
    public float aimSensitivity = 2f;
    public float maxVerticalAngle = 80f;
    public bool invertY = false;
    
    [Header("References")]
    public Transform weaponPivot;
    public Transform playerBody;
    public Camera playerCamera;
    
    [Header("Zoom")]
    public bool isAiming = false;
    public float normalFOV = 60f;
    public float zoomedFOV = 40f;
    
    private float xRotation = 0f;
    private float yRotation = 0f;
    
    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
            
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        HandleMouseLook();
        isAiming = Input.GetMouseButton(1); // کلیک راست برای هدف‌گیری
        HandleZoom();
    }
    
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * aimSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * aimSensitivity * (invertY ? -1 : 1);
        
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);
        
        // چرخش بدنه
        if (playerBody != null)
            playerBody.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        
        // چرخش دوربین
        if (playerCamera != null)
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    void HandleZoom()
    {
        if (playerCamera == null) return;
        
        float targetFOV = isAiming ? zoomedFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * 10f);
    }
    
    public Vector3 GetAimDirection()
    {
        if (playerCamera != null)
            return playerCamera.transform.forward;
        return transform.forward;
    }
}