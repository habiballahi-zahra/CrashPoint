
using UnityEngine;
   
// این کلاس اطلاعات پایه هر آیتم را نگهداری می‌کند
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item";        // نام آیتم
    public Sprite icon = null;                  // آیکون در UI
    public GameObject prefab;                   // مدل ۳بعدی آیتم
    public ItemType itemType = ItemType.General; // نوع آیتم
    
    public enum ItemType
    {
        General,     // آیتم عمومی
        Key,         // کلید
        Health,      // سلامت
        Ammo,        // مهمات
        Weapon       // سلاح
    }
    
    // این متد زمانی صدا زده می‌شود که آیتم استفاده شود
    public virtual void Use()
    {
        Debug.Log("استفاده از آیتم: " + itemName);
        // اینجا می‌توانید منطق استفاده از آیتم را بنویسید
    }
}

