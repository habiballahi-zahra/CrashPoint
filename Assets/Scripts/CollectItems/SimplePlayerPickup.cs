// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections.Generic;
// using TMPro;

// public class SimplePlayerPickup : MonoBehaviour
// {
//     // ──────────────────────────────
//     // بخش ۱: تنظیمات و متغیرهای عمومی
//     // ──────────────────────────────
    
//     [Header("Settings")]
//     public float pickupRadius = 2f;       // شعاع جستجوی اشیاء اطراف پلیر (به متر)
//     public KeyCode pickupKey = KeyCode.E; // کلید برای برداشتن آیتم
//     public float forwardDistance = 3f;    // فاصله جلوی پلیر برای نمایش مخروط دید
    
//     [Header("UI")]
//     public TextMeshProUGUI interactionText;          // متن UI برای نمایش پیام تعامل
//     public ToolboxUI toolboxUI;           // اسکریپت مدیریت جعبه ابزار
    
//     [Header("ToolBox")]
//     public List<Item> inventory = new List<Item>(); // لیست آیتم‌های جمع‌آوری شده
    
//     // شیء قابل تعامل فعلی که پلیر می‌تواند بردارد
//     private Interactable currentInteractable;

//     private ItemSocket currentSocket;

    
//     // ──────────────────────────────
//     // بخش ۲: توابع اصلی Unity
//     // ──────────────────────────────
    
//     // تابع Start هنگام شروع بازی اجرا می‌شود
//     void Start()
//     {
//         // در شروع بازی، متن تعامل را مخفی می‌کنیم
//         if (interactionText != null)
//             interactionText.gameObject.SetActive(false);
//     }
    
//     // تابع Update در هر فریم اجرا می‌شود
//     void Update()
//     {
//         // در هر فریم، نزدیک‌ترین شیء قابل تعامل را پیدا کن
//         FindClosestInteractable();
        
//         // اگر کلید E زده شد و شیء قابل تعامل وجود دارد
//        if (Input.GetKeyDown(pickupKey))
// {
//     if (currentInteractable != null)
//         PickupCurrentItem();
//     else if (currentSocket != null)
//         TryPlaceItem();
// }



//     }
    
//     // ──────────────────────────────
//     // بخش ۳: منطق پیدا کردن اشیاء
//     // ──────────────────────────────
    
//     // این تابع نزدیک‌ترین شیء Interactable را در اطراف پلیر پیدا می‌کند
//    void FindClosestInteractable()
// {
//     Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, pickupRadius);

//     Interactable closest = null;
//     ItemSocket closestSocket = null;
//     float closestDistance = Mathf.Infinity;

//     foreach (Collider collider in nearbyColliders)
//     {
//         if (!collider.CompareTag("Interactable")) continue;

//         // 🔹 اینجا interactable تعریف میشه
//         Interactable interactable = collider.GetComponent<Interactable>();
//         if (interactable == null) continue;

//         float distance = Vector3.Distance(transform.position, collider.transform.position);
//         if (distance > closestDistance) continue;

//         Vector3 toObject = (collider.transform.position - transform.position).normalized;
//         float dot = Vector3.Dot(transform.forward, toObject);
//         if (dot < -0.3f) continue;

//         // ───────── تصمیم‌گیری ─────────

//         // 1️⃣ آیتم قابل برداشتن
//         if (interactable.item != null)
//         {
//             closest = interactable;
//             closestSocket = null;
//             closestDistance = distance;
//         }
//         // 2️⃣ احتمالاً سوکت
//         else
//         {
//             ItemSocket socket = collider.GetComponent<ItemSocket>();
//             if (socket != null)
//             {
//                 closestSocket = socket;
//                 closest = null;
//                 closestDistance = distance;
//             }
//         }
//     }

//     // ───────── نمایش UI ─────────

//     if (closest != null)
//     {
//         currentInteractable = closest;
//         currentSocket = null;
//         ShowPrompt($"E : Collect {closest.item.itemName}");
//     }
//     else if (closestSocket != null)
//     {
//         currentSocket = closestSocket;
//         currentInteractable = null;

//         if (HasItem(closestSocket.requiredItem))
//             ShowPrompt($"E : Place {closestSocket.requiredItem.itemName}");
//         else
//             ShowPrompt($"Need {closestSocket.requiredItem.itemName}");
//     }
//     else
//     {
//         currentInteractable = null;
//         currentSocket = null;
//         HidePrompt();
//     }
// }

    
//     // ──────────────────────────────
//     // بخش ۴: منطق برداشتن آیتم
//     // ──────────────────────────────
    
//     // این تابع آیتم فعلی را برمی‌دارد
//     void PickupCurrentItem()
//     {
//         // اگر شیء قابل تعامل وجود ندارد، کاری نکن
//         if (currentInteractable == null) return;
        
//         // پیام دیباگ
//         Debug.Log($"Pickupping: {currentInteractable.item.itemName}");
        
//         // ۱. انیمیشن برداشت را اجرا کن
//         Animator anim = GetComponent<Animator>();
//         if (anim != null)
//             anim.SetTrigger("Pickup"); // تریگر "Pickup" را در انیماتور فعال کن
        
//         // ۲. آیتم را به جعبه ابزار اضافه کن
//         // اضافه کردن آیتم به Inventory اصلی
//         if (Inventory.Instance == null)
//         {
//             Debug.LogError("Inventory.Instance is NULL");
//         }
//         else
//         {
//             Inventory.Instance.AddItem(currentInteractable.item);
//         }

        // // آپدیت UI
        // if (toolboxUI != null)
        // {
        //     toolboxUI.AddItem(currentInteractable.item);
        // }

        
//         // ۳. شیء را از صحنه مخفی کن (اما کاملاً پاک نکن)
//         // SetActive(false) شیء را غیرفعال می‌کند اما در حافظه باقی می‌ماند
//         currentInteractable.gameObject.SetActive(false);
        
//         // ۴. متن UI را مخفی کن
//         HidePrompt();
        
//         // ۵. مرجع شیء را پاک کن (چون برداشته شد)
//         currentInteractable = null;
//     }
    
//     // ──────────────────────────────
//     // بخش ۵: مدیریت UI
//     // ──────────────────────────────
    
//     // نمایش پیام تعامل در UI
//     void ShowPrompt(string message)
//     {
//         // اگر کامپوننت Text وجود دارد
//         if (interactionText != null)
//         {
//             // متن را تنظیم و نمایش بده
//             interactionText.text = message;
//             interactionText.gameObject.SetActive(true);
//         }
//     }
    
//     // مخفی کردن پیام تعامل
//     void HidePrompt()
//     {
//         // اگر کامپوننت Text وجود دارد، آن را مخفی کن
//         if (interactionText != null)
//             interactionText.gameObject.SetActive(false);
//     }
    
//     // ──────────────────────────────
//     // بخش ۶: ابزارهای دیباگ
//     // ─────────────────────────────ـ
    
//     // این تابع فقط در Unity Editor و هنگام انتخاب شیء در Scene View اجرا می‌شود
//     // برای نمایش محدوده‌های تشخیص به صورت گرافیکی
  
  
  
  
//   void TryPlaceItem()
// {
//     if (currentSocket == null) return;

//     // اگر آیتم مورد نیاز رو نداریم
//     if (!HasItem(currentSocket.requiredItem))
//     {
//         Debug.Log("Required item not in inventory");
//         return;
//     }

//     // پر کردن سوکت
//     currentSocket.PlaceItem();

//     // حذف آیتم از اینونتوری
//     inventory.Remove(currentSocket.requiredItem);

//     HidePrompt();
//     currentSocket = null;
// }

  
  
  
  
  
//     void OnDrawGizmosSelected()
//     {
//         // ۱. کره سبز رنگ برای نشان دادن شعاع جستجو
//         // رنگ: سبز با شفافیت ۳۰٪ (آلفا ۰.۳)
//         Gizmos.color = new Color(0, 1, 0, 0.3f);
//         Gizmos.DrawSphere(transform.position, pickupRadius);
        
//         // ۲. خط زرد رنگ برای نشان دادن جهت نگاه پلیر
//         Gizmos.color = Color.yellow;
//         Vector3 forwardEnd = transform.position + transform.forward * forwardDistance;
//         Gizmos.DrawLine(transform.position, forwardEnd);
        
//         // ۳. خطوط کناری برای نشان دادن مخروط دید
//         float angle = 110f; // زاویه کل مخروط (۱۱۰ درجه)
        
//         // چرخش جهت نگاه به چپ (نصف زاویه)
//         Vector3 leftDir = Quaternion.Euler(0, -angle/2, 0) * transform.forward;
//         // چرخش جهت نگاه به راست (نصف زاویه)
//         Vector3 rightDir = Quaternion.Euler(0, angle/2, 0) * transform.forward;
        
//         // کشیدن خط چپ
//         Gizmos.DrawLine(transform.position, transform.position + leftDir * forwardDistance);
//         // کشیدن خط راست
//         Gizmos.DrawLine(transform.position, transform.position + rightDir * forwardDistance);
//     }
    
//     // ──────────────────────────────
//     // بخش ۷: توابع کمکی (اختیاری)
//     // ─────────────────────────────ـ
    
//     // بررسی می‌کند که آیا آیتم خاصی در جعبه ابزار وجود دارد
//     public bool HasItem(Item itemToCheck)
//     {
//         return inventory.Contains(itemToCheck);
//     }
    
//     // بررسی می‌کند که آیا آیتمی با نام خاص در جعبه ابزار وجود دارد
//     public bool HasItemByName(string itemName)
//     {
//         foreach (Item item in inventory)
//         {
//             if (item.itemName == itemName)
//                 return true;
//         }
//         return false;
//     }
    
//     // تعداد آیتم‌های موجود در جعبه ابزار
//     public int GetItemCount()
//     {
//         return inventory.Count;
//     }
    
//     // خالی کردن جعبه ابزار
//     public void ClearInventory()
//     {
//         inventory.Clear();
//         Debug.Log("جعبه ابزار خالی شد");
//     }
// }


using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimplePlayerPickup : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI")]
    public TextMeshProUGUI interactionText;   // متن UI تعامل

    public ToolboxUI toolboxUI;           // اسکریپت مدیریت جعبه ابزار


    private Interactable currentInteractable;
    private ItemSocket currentSocket;

    void Start()
    {
        HideUI();
    }

    void Update()
    {
        FindInteractable();

        if (Input.GetKeyDown(interactKey))
        {
            if (currentInteractable != null)
            {
                PickupItem();
            }
            else if (currentSocket != null)
            {
                PlaceItemInSocket();
            }
        }
    }

    // =========================
    // 🔍 پیدا کردن آیتم یا سوکت
    // =========================
    void FindInteractable()
    {
        currentInteractable = null;
        currentSocket = null;
        HideUI();

        Interactable[] interactables = FindObjectsOfType<Interactable>();
        float closestDistance = interactDistance;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance > closestDistance)
                continue;

            // 🟢 آیتم قابل برداشتن
            if (interactable.item != null)
            {
                currentInteractable = interactable;
                currentSocket = null;
                closestDistance = distance;

                ShowUI($"E : Collect {interactable.item.itemName}");
            }
            // 🔵 بررسی سوکت
            else
            {
                ItemSocket socket = interactable.GetComponent<ItemSocket>();
                if (socket == null || socket.isFilled)
                    continue;

                currentSocket = socket;
                currentInteractable = null;
                closestDistance = distance;

                if (Inventory.Instance != null &&
                    Inventory.Instance.HasItem(socket.requiredItem))
                {
                    ShowUI($"E : Place {socket.requiredItem.itemName}");
                }
                else
                {
                    ShowUI($"Need {socket.requiredItem.itemName}");
                }
            }
        }
    }

    // =========================
    // 🎒 برداشتن آیتم
    // =========================
    void PickupItem()
    {
        Item item = currentInteractable.item;

        Inventory.Instance.AddItem(item);
          // آپدیت UI
        if (toolboxUI != null)
        {
            toolboxUI.AddItem(item);
        }

        Destroy(currentInteractable.gameObject);
        currentInteractable = null;

        HideUI();
    }

    // =========================
    // 🔌 جاگذاری در سوکت
    // =========================
    void PlaceItemInSocket()
    {
        Item requiredItem = currentSocket.requiredItem;

        if (!Inventory.Instance.HasItem(requiredItem))
            return;

        Inventory.Instance.RemoveItem(requiredItem);

           // آپدیت UI
        if (toolboxUI != null)
        {
            toolboxUI.RemoveItem(requiredItem);
        }
        currentSocket.PlaceItem();

        currentSocket = null;
        HideUI();

       
    }

    // =========================
    // 🖥 UI Helpers
    // =========================
    void ShowUI(string message)
    {
        if (interactionText == null) return;

        interactionText.gameObject.SetActive(true);
        interactionText.text = message;
    }

    void HideUI()
    {
        if (interactionText == null) return;

        interactionText.gameObject.SetActive(false);
    }
}
