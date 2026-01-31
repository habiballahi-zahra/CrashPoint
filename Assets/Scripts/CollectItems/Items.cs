
using UnityEngine;
   
// این کلاس اطلاعات پایه هر آیتم را نگهداری می‌کند
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item";        // نام آیتم
    public Sprite icon = null;                  // آیکون در UI
    public GameObject prefab;                   // مدل ۳بعدی آیتم
    public ItemType itemType = ItemType.General; // نوع آیتم
    
     [Header("Health Item Settings")]
    public int healAmount = 0;   // فقط برای آیتم Health استفاده می‌شود
    public enum ItemType
    {
        General,     // آیتم عمومی
        Key,         // کلید
        Health,      // سلامت
        Ammo,        // مهمات
        Weapon       // سلاح
    }
    
    // این متد زمانی صدا زده می‌شود که آیتم استفاده شود
     public virtual void Use(GameObject user)
    {
        
            if (itemType != ItemType.Health) return;

            Health health = user.GetComponent<Health>();
            if (health == null) return;

            if (health.currentHealth >= health.maxHealth)
                return;

            health.Heal(healAmount);
    }
}

