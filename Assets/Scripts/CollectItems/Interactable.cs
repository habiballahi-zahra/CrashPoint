// Interactable.cs
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Items")]
    public Item item;                    // اطلاعات آیتم
    public float rotationSpeed = 50f;    // سرعت چرخش (برای جلوه بصری)
    public bool isMove=true;
    [Header("Effects")]
    public GameObject pickupEffect;      // افکت هنگام برداشت
    public AudioClip pickupSound;        // صدای برداشت
    
    private Vector3 initialPosition;
    private bool isPickedUp = false;
    
    void Start()
    {
        initialPosition = transform.position;
        
    }
    
    void Update()
    {
        // اگر هنوز برداشته نشده، بچرخ (برای جلوه بصری)
        if (!isPickedUp)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            if (isMove)
            {
                    // حرکت بالا-پایین (اختیاری)
                float floatHeight = Mathf.Sin(Time.time * 2f) * 0.1f;
                transform.position = initialPosition + Vector3.up * floatHeight;
            }
           
        }
    }
    
    // وقتی بازیکن آیتم را برمی‌دارد
    public void Pickup()
    {
        if (isPickedUp) return;
        
        isPickedUp = true;
        
        // پخش افکت
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }
        
        // پخش صدا
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
        
        // غیرفعال کردن رندرر و کالایدر (اما شیء فعال بماند)
        SetVisible(false);
        
        // پس از ۲ ثانیه کاملاً از بین برود
        Destroy(gameObject, 2f);
        
        Debug.Log("Interact Item  " + item.itemName);
    }
    
    // تنظیم وضعیت نمایش شیء
    void SetVisible(bool visible)
    {
        // غیرفعال کردن رندررها
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
        
        // غیرفعال کردن کالایدرها
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = visible;
        }
    }
    
    // وقتی بازیکن وارد محدوده می‌شود (روش جایگزین به جای Raycast)
    void OnTriggerEnter(Collider other)
    {
        if (isPickedUp) return;
        
        if (other.CompareTag("Player"))
        {
            InteractionController player = other.GetComponent<InteractionController>();
            if (player != null)
            {
                player.ShowInteractionPrompt(item.itemName);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionController player = other.GetComponent<InteractionController>();
            if (player != null)
            {
                player.HideInteractionPrompt();
            }
        }
    }
}