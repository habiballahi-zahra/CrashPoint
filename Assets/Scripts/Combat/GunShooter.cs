using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public Transform gunPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 40f;
    public AimingSystem aimingSystem; // اضافه شده
    
    private bool hasShotThisFrame = false;
    
    void Start()
    {
        if (aimingSystem == null)
        {
            aimingSystem = GetComponentInParent<AimingSystem>();
        }
    }
    
    void Update()
    {
        // ریست فلگ هر فریم
        hasShotThisFrame = false;
        
        if (Input.GetMouseButtonDown(0) && !hasShotThisFrame)
        {
            hasShotThisFrame = true;
            ShootSingleBullet();
        }
    }
    
    public void ShootSingleBullet()
    {
        Debug.Log("تنها یک گلوله شلیک شد");
        
        if (bulletPrefab && gunPoint)
        {
            Vector3 shootDirection = GetShootDirection();
            
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(shootDirection));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            
            if (rb)
            {
                rb.velocity = shootDirection * bulletSpeed;
                rb.useGravity = false;
            }
            
            Destroy(bullet, 2f);
        }
    }
    
    Vector3 GetShootDirection()
    {
        if (aimingSystem != null)
        {
            return aimingSystem.GetAimDirection();
        }
        
        return gunPoint.forward;
    }
    
    // جلوگیری از شلیک چندگانه
    void LateUpdate()
    {
        hasShotThisFrame = false;
    }
}