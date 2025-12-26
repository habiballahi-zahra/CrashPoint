// InteractionController.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InteractionController : MonoBehaviour
{
    [Header("Settings")]
    public float interactionRange = 3f;           // فاصله تعامل
    public KeyCode interactionKey = KeyCode.E;    // کلید تعامل
    
    [Header("UI")]
    public GameObject interactionPrompt;          // متن "Press E to interact"
    public ToolboxUI toolboxUI;                   // رابط جعبه ابزار
    
    [Header("Collect System")]
    public List<Item> collectedItems = new List<Item>(); // لیست آیتم‌های جمع‌آوری شده
    public int maxInventorySize = 20;             // حداکثر تعداد آیتم‌ها
    
    // شیء قابل تعامل فعلی
    private Interactable currentInteractable;
    private Animator playerAnimator;
    
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        
        // مخفی کردن prompt در شروع
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
    
    void Update()
    {
              // خط اول Update این کد رو اضافه کن:
    Debug.Log($"Update: E key down? {Input.GetKeyDown(KeyCode.E)}, Has interactable? {currentInteractable != null}");


        // هر فریم، به دنبال اشیاء قابل تعامل بگرد
        CheckForInteractables();
        
        // اگر کلید E زده شد و شیء قابل تعامل وجود دارد
        if (Input.GetKeyDown(interactionKey) && currentInteractable != null)
        {
            InteractWithObject();
        }
    }
    
    // بررسی وجود اشیاء قابل تعامل در اطراف
   // InteractionController.cs - در تابع CheckForInteractables
void CheckForInteractables()
{
   // همیشه از Camera استفاده کن
    if (Camera.main == null)
    {
        Debug.LogError("No main camera!");
        return;
    }
    
    // Ray از مرکز صفحه به زمین
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    RaycastHit hit;
    
    Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 0.1f);
    
    if (Physics.Raycast(ray, out hit, interactionRange))
    {
        Debug.Log($"Looking at: {hit.collider.name}");
        
        Interactable interactable = hit.collider.GetComponent<Interactable>();
        if (interactable != null && interactable.item != null)
        {
            currentInteractable = interactable;
            ShowInteractionPrompt(interactable.item.itemName);
            return;
        }
    }
    
    // اگر چیزی پیدا نشد
    if (currentInteractable != null)
    {
        currentInteractable = null;
        HideInteractionPrompt();
    }
}
    
    // تعامل با شیء
    void InteractWithObject()
    {
        if (currentInteractable == null) return;
        
        // اگر انبار پر است
        if (collectedItems.Count >= maxInventorySize)
        {
            Debug.Log("جعبه ابزار پر است!");
            return;
        }
        
        // پخش انیمیشن برداشت
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Pickup");
        }
        
        // اضافه کردن آیتم به لیست
        AddItemToInventory(currentInteractable.item);
        
        // از بین بردن شیء از صحنه
        currentInteractable.Pickup();
        
        // پاک کردن مرجع
        currentInteractable = null;
        HideInteractionPrompt();
    }
    
    // اضافه کردن آیتم به جعبه ابزار
    public void AddItemToInventory(Item newItem)
    {
        if (newItem == null) return;
        
        // اضافه به لیست
        collectedItems.Add(newItem);
        Debug.Log("آیتم اضافه شد: " + newItem.itemName);
        
        // به‌روزرسانی UI
        if (toolboxUI != null)
        {
            toolboxUI.AddItem(newItem);
        }
        
        // می‌توانید اینجا صدا یا افکت پخش کنید
        // AudioManager.Instance.PlaySound("Pickup");
    }
    
    // حذف آیتم از جعبه ابزار
    public void RemoveItemFromInventory(Item itemToRemove)
    {
        if (collectedItems.Contains(itemToRemove))
        {
            collectedItems.Remove(itemToRemove);
            
            // به‌روزرسانی UI
            if (toolboxUI != null)
            {
                toolboxUI.RemoveItem(itemToRemove);
            }
        }
    }
    
    // بررسی وجود یک آیتم خاص
    public bool HasItem(Item itemToCheck)
    {
        return collectedItems.Contains(itemToCheck);
    }
    
    // بررسی وجود آیتم بر اساس نام
    public bool HasItemByName(string itemName)
    {
        foreach (Item item in collectedItems)
        {
            if (item.itemName == itemName)
                return true;
        }
        return false;
    }
    
    // نمایش prompt تعامل
    public void ShowInteractionPrompt(string itemName)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
            
            // می‌توانید متن prompt را تغییر دهید
            Text promptText = interactionPrompt.GetComponent<Text>();
            if (promptText != null)
            {
                promptText.text = $"E : برداشتن {itemName}";
            }
        }
    }
    
    // مخفی کردن prompt
    public void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
    
    // پاک کردن مرجع شیء قابل تعامل
    void ClearCurrentInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable = null;
            HideInteractionPrompt();
        }
    }
    
    // برای Debug: نمایش Ray در Scene View
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
        Gizmos.DrawRay(rayOrigin, transform.forward * interactionRange);
    }
}

