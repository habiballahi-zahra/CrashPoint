using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [Header("References")]
    public Transform gunPoint;

    [Header("Gun Settings")]
    public float range = 30f;
    public int damage = 10;
    public float fireRate = 0.25f;

    private float lastFireTime;

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

        Ray ray = new Ray(gunPoint.position, gunPoint.forward);
        RaycastHit hit;

        Debug.DrawRay(gunPoint.position, gunPoint.forward * range, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("HIT: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                Health hp = hit.collider.GetComponentInParent<Health>();
                if (hp != null)
                {
                    hp.TakeDamage(damage);
                    Debug.Log("ENEMY DAMAGED");
                }
            }
        }
    }
}
