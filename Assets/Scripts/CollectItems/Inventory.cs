using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Singleton Instance
    public static Inventory Instance;

    // لیست آیتم‌ها
    public List<Item> items = new List<Item>();

    void Awake()
    {
        // اگر اولین نمونه است
        if (Instance == null)
        {
            Instance = this;

            // ⭐ این خط مهم‌ترین بخشه
            // باعث میشه Inventory بین Scene ها باقی بمونه
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // اگر قبلاً Inventory وجود داشته
            // این یکی اضافیه → حذفش کن
            Destroy(gameObject);
        }
    }

    // ─────────────────────────────
    // اضافه کردن آیتم
    // ─────────────────────────────
    public void AddItem(Item item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            Debug.Log("Item added to Inventory: " + item.itemName);
        }
    }

    // ─────────────────────────────
    // حذف آیتم
    // ─────────────────────────────
    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log("Item removed from Inventory: " + item.itemName);
        }
    }

    // ─────────────────────────────
    // بررسی وجود آیتم
    // ─────────────────────────────
    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }
}
