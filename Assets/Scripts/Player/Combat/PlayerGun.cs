using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [Header("References")]
    public Transform gunPoint;
    public GameObject bulletPrefab;

    [Header("Gun Settings")]
    public float fireRate = 0.25f;
    public float bulletSpeed = 40f;
    
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
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }
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
        
        // ۱. ایجاد گلوله
        if (bulletPrefab != null && gunPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
            
            // اعمال سرعت
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = gunPoint.forward * bulletSpeed;
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
        
        // ۴. Raycast برای تشخیص برخورد (همان قبلی)
        Ray ray = new Ray(gunPoint.position, gunPoint.forward);
        RaycastHit hit;
        Debug.DrawRay(gunPoint.position, gunPoint.forward * 100f, Color.red, 1f);

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
    }
}