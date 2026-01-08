using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public int damage = 10;
    public float maxDistance = 100f;
    public GameObject hitEffect;
    
    private Vector3 startPosition;
    private bool hasHit = false;
    private TrailRenderer trail; // برای اثر دنباله گلوله

    void Start()
    {
      


        startPosition = transform.position;
        
        // اضافه کردن TrailRenderer برای جلوه بصری بهتر
        trail = GetComponent<TrailRenderer>();
        if (trail == null)
        {
            trail = gameObject.AddComponent<TrailRenderer>();
            trail.time = 0.1f;
            trail.widthMultiplier = 0.05f;
            trail.material = new Material(Shader.Find("Sprites/Default"));
            trail.startColor = Color.yellow;
            trail.endColor = Color.red;
        }
        
        // غیرفعال کردن collision با player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            if (playerCollider != null)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), playerCollider);
            }
        }
    }

    void Update()
    {   
        if (hasHit) return;
        
        // بررسی مسافت طی شده
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            DestroyBullet();
        }
    }


     private void OnTriggerEnter(Collider other) {
        
        // if (hasHit) return;
        
        // نادیده گرفتن برخورد با player
        if (other.gameObject.CompareTag("Player")) return;
        
        // hasHit = true;
        
        // آسیب به دشمن
        if (other.gameObject.CompareTag("Enemy"))
        {
            Health enemyHealth = other.gameObject.GetComponentInParent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"آسیب به دشمن: {damage}");
            }
        }
        
        // افکت برخورد
        // if (hitEffect != null)
        // {
        //     Instantiate(hitEffect, transform.position, Quaternion.identity);
        // }
        
        // DestroyBullet();
    }

    void DestroyBullet()
    {
        // غیرفعال کردن TrailRenderer قبل از نابودی
        if (trail != null)
        {
            trail.autodestruct = true;
        }
        
        Destroy(gameObject);
    }
}