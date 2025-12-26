// ToolboxUI.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToolboxUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject toolboxPanel;          // پنل اصلی جعبه ابزار
    public Transform itemsContainer;         // والد آیتم‌های UI
    public GameObject itemUIPrefab;          // پرفب آیتم UI
    
    [Header("Show Setting")]
    public KeyCode toggleKey = KeyCode.Tab;  // کلید نمایش/مخفی کردن
    public int maxItemsPerRow = 5;           // حداکثر آیتم در هر ردیف
    
    private Dictionary<Item, GameObject> itemUIMap = new Dictionary<Item, GameObject>();
    private bool isToolboxVisible = false;
    
    void Start()
    {
        // مخفی کردن پنل در شروع
        if (toolboxPanel != null)
            toolboxPanel.SetActive(false);
    }
    
    void Update()
    {
        // نمایش/مخفی کردن با کلید Tab
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleToolbox();
        }
    }
    
    // اضافه کردن آیتم به UI
    public void AddItem(Item item)
    {
        if (item == null || itemUIMap.ContainsKey(item)) return;
        
        // ایجاد آیتم UI جدید
        GameObject newItemUI = Instantiate(itemUIPrefab, itemsContainer);
        
        // تنظیم آیکون و متن
        Image iconImage = newItemUI.GetComponentInChildren<Image>();
        if (iconImage != null && item.icon != null)
        {
            iconImage.sprite = item.icon;
        }
        
        Text nameText = newItemUI.GetComponentInChildren<Text>();
        if (nameText != null)
        {
            nameText.text = item.itemName;
        }
        
        // ذخیره در دیکشنری
        itemUIMap.Add(item, newItemUI);
        
        // مرتب‌سازی آیتم‌ها
        OrganizeItems();
    }
    
    // حذف آیتم از UI
    public void RemoveItem(Item item)
    {
        if (item == null || !itemUIMap.ContainsKey(item)) return;
        
        // از بین بردن GameObject مربوطه
        Destroy(itemUIMap[item]);
        
        // حذف از دیکشنری
        itemUIMap.Remove(item);
        
        // مرتب‌سازی مجدد
        OrganizeItems();
    }
    
    // مرتب‌سازی آیتم‌ها در UI
    void OrganizeItems()
    {
        int index = 0;
        foreach (var kvp in itemUIMap)
        {
            GameObject itemUI = kvp.Value;
            RectTransform rt = itemUI.GetComponent<RectTransform>();
            
            if (rt != null)
            {
                // محاسبه موقعیت بر اساس index
                int row = index / maxItemsPerRow;
                int col = index % maxItemsPerRow;
                
                float xPos = col * 100;  // فاصله افقی
                float yPos = -row * 100; // فاصله عمودی
                
                rt.anchoredPosition = new Vector2(xPos, yPos);
            }
            
            index++;
        }
    }
    
    // نمایش/مخفی کردن جعبه ابزار
    void ToggleToolbox()
    {
        isToolboxVisible = !isToolboxVisible;
        
        if (toolboxPanel != null)
            toolboxPanel.SetActive(isToolboxVisible);
        
        // اگر جعبه ابزار باز شد، بازی را متوقف کن (اختیاری)
        if (isToolboxVisible)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    // بستن جعبه ابزار
    public void CloseToolbox()
    {
        isToolboxVisible = false;
        
        if (toolboxPanel != null)
            toolboxPanel.SetActive(false);
        
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
