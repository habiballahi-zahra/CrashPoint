using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [Header("References")]
    public Transform gunPoint;
    public GameObject bulletPrefab;
    public AimingSystem aimingSystem; // اضافه شده

    [Header("Gun Settings")]
    public float fireRate = 0.25f;
    public float bulletSpeed = 40f;
    
    [Header("Recoil Settings")] // اضافه شده
    public float recoilAmount = 1f;
    public float recoilRecoverySpeed = 5f;
    private Vector3 currentRecoil;
    
    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;
    
    private float lastFireTime;
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource اضافه کن
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // پیدا کردن سیستم هدف‌گیری اگر ست نشده
        if (aimingSystem == null)
        {
            aimingSystem = GetComponentInParent<AimingSystem>();
        }
    }

    void Update()
    {
        // بازیابی ریکویل
        currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, Time.deltaTime * recoilRecoverySpeed);
        
        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }
        
        // اعمال ریکویل به اسلحه
        ApplyRecoil();
    }

    void TryShoot()
    {
        if (Time.time - lastFireTime < fireRate)
            return;

        lastFireTime = Time.time;
        Shoot();
    }

    void Shoot()
    {
        Debug.Log("SHOOT!");
        
        // ۱. ایجاد گلوله با جهت هدف‌گیری
        if (bulletPrefab != null && gunPoint != null)
        {
            Vector3 shootDirection = GetShootDirection();
            
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(shootDirection));
            
            // اعمال سرعت
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = shootDirection * bulletSpeed;
            }
        }
        
        // ۲. افکت دهانه تفنگ
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        
        // ۳. پخش صدا
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        
        // ۴. Raycast برای تشخیص برخورد با در نظر گرفتن سیستم هدف‌گیری
        Vector3 rayDirection = GetShootDirection();
        Ray ray = new Ray(gunPoint.position, rayDirection);
        RaycastHit hit;
        Debug.DrawRay(gunPoint.position, rayDirection * 100f, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log("HIT: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                Health hp = hit.collider.GetComponentInParent<Health>();
                if (hp != null)
                {
                    hp.TakeDamage(10);
                    Debug.Log("ENEMY DAMAGED");
                }
            }
        }
        
        // ۵. اعمال ریکویل
        ApplyRecoilForce();
    }
    
    Vector3 GetShootDirection()
    {
        Vector3 direction;
        
        if (aimingSystem != null)
        {
            // استفاده از جهت سیستم هدف‌گیری
            direction = aimingSystem.GetAimDirection();
        }
        else
        {
            // روش قدیمی
            direction = gunPoint.forward;
        }
        
        // اضافه کردن ریکویل
        direction += currentRecoil;
        direction.Normalize();
        
        return direction;
    }
    
    void ApplyRecoilForce()
    {
        // اضافه کردن ریکویل تصادفی
        currentRecoil += new Vector3(
            Random.Range(-recoilAmount, recoilAmount),
            Random.Range(0, recoilAmount),
            0
        );
    }
    
    void ApplyRecoil()
    {
        if (currentRecoil.magnitude > 0.01f && aimingSystem != null && aimingSystem.weaponPivot != null)
        {
            // اعمال ریکویل به اسلحه
            aimingSystem.weaponPivot.localRotation *= Quaternion.Euler(currentRecoil * 10f);
        }
    }
}