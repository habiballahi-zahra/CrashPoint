using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SimplePlayerPickup : MonoBehaviour
{
    // ──────────────────────────────
    // بخش ۱: تنظیمات و متغیرهای عمومی
    // ──────────────────────────────
    
    [Header("Settings")]
    public float pickupRadius = 2f;       // شعاع جستجوی اشیاء اطراف پلیر (به متر)
    public KeyCode pickupKey = KeyCode.E; // کلید برای برداشتن آیتم
    public float forwardDistance = 3f;    // فاصله جلوی پلیر برای نمایش مخروط دید
    
    [Header("UI")]
    public TextMeshProUGUI interactionText;          // متن UI برای نمایش پیام تعامل
    public ToolboxUI toolboxUI;           // اسکریپت مدیریت جعبه ابزار
    
    [Header("ToolBox")]
    public List<Item> inventory = new List<Item>(); // لیست آیتم‌های جمع‌آوری شده
    
    // شیء قابل تعامل فعلی که پلیر می‌تواند بردارد
    private Interactable currentInteractable;
    
    // ──────────────────────────────
    // بخش ۲: توابع اصلی Unity
    // ──────────────────────────────
    
    // تابع Start هنگام شروع بازی اجرا می‌شود
    void Start()
    {
        // در شروع بازی، متن تعامل را مخفی می‌کنیم
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }
    
    // تابع Update در هر فریم اجرا می‌شود
    void Update()
    {
        // در هر فریم، نزدیک‌ترین شیء قابل تعامل را پیدا کن
        FindClosestInteractable();
        
        // اگر کلید E زده شد و شیء قابل تعامل وجود دارد
        if (Input.GetKeyDown(pickupKey) && currentInteractable != null)
        {
            // آیتم فعلی را بردار
            PickupCurrentItem();
        }
    }
    
    // ──────────────────────────────
    // بخش ۳: منطق پیدا کردن اشیاء
    // ──────────────────────────────
    
    // این تابع نزدیک‌ترین شیء Interactable را در اطراف پلیر پیدا می‌کند
    void FindClosestInteractable()
    {
        // ۱. تمام Colliderها در شعاع مشخص شده اطراف پلیر را پیدا کن
        // OverlapSphere یک کره مجازی در اطراف پلیر ایجاد می‌کند
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, pickupRadius);
        
        // ۲. متغیرهایی برای ذخیره نزدیک‌ترین شیء و فاصله آن
        Interactable closest = null;
        float closestDistance = Mathf.Infinity; // ابتدا فاصله را بی‌نهایت قرار می‌دهیم
        
        // ۳. بین تمام Colliderهای پیدا شده، به دنبال Interactable بگرد
        foreach (Collider collider in nearbyColliders)
        {
            // ۳-۱. فقط Colliderهایی که تگ "Interactable" دارند را بررسی کن
            // (تگ Interactable را باید به اشیاء قابل برداشت داده باشی)
            if (!collider.CompareTag("Interactable")) continue;
            
            // ۳-۲. کامپوننت Interactable را از Collider بگیر
            Interactable interactable = collider.GetComponent<Interactable>();
            
            // ۳-۳. اگر Interactable وجود ندارد یا آیتم آن خالی است، ادامه بده
            if (interactable == null || interactable.item == null) continue;
            
            // ۳-۴. فاصله بین پلیر و این شیء را محاسبه کن
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            
            // ۳-۵. جهت از پلیر به شیء را محاسبه کن (وکتور نرمالایز شده)
            Vector3 toObject = (collider.transform.position - transform.position).normalized;
            
            // ۳-۶. ضرب نقطه‌ای (Dot Product) جهت پلیر و جهت به شیء
            // این مقدار بین ۱- تا ۱ است:
            //  ۱ = دقیقاً در جلوی پلیر
            //  ۰ = در راستای عمود بر پلیر
            // -۱ = دقیقاً پشت پلیر
            float dot = Vector3.Dot(transform.forward, toObject);
            
            // ۳-۷. اگر شیء در جلو یا اطراف پلیر است (نه پشت سر)
            // مقدار ۰.۳- یعنی حدود ۱۱۰ درجه دید (۵۵ درجه از هر طرف)
            if (dot > -0.3f)
            {
                // ۳-۸. اگر این شیء از قبلی نزدیک‌تر است
                if (distance < closestDistance)
                {
                    // این شیء جدید را به عنوان نزدیک‌ترین ذخیره کن
                    closestDistance = distance;
                    closest = interactable;
                }
            }
        }
        
        // ۴. اگر شیء جدیدی پیدا شد که با قبلی متفاوت است
        if (closest != null && closest != currentInteractable)
        {
            // شیء جدید را به عنوان currentInteractable تنظیم کن
            currentInteractable = closest;
            
            // پیام UI را نمایش بده
            ShowPrompt($"E : برداشتن {closest.item.itemName}");
            
            // برای دیباگ در کنسول لاگ بزن
            Debug.Log($"پیدا شد: {closest.item.itemName} در فاصله {closestDistance:F1} متر");
        }
        // ۵. اگر هیچ شیء‌ای پیدا نشد ولی قبلاً شیء‌ای بود
        else if (closest == null && currentInteractable != null)
        {
            // شیء قبلی را پاک کن و UI را مخفی کن
            HidePrompt();
            currentInteractable = null;
        }
    }
    
    // ──────────────────────────────
    // بخش ۴: منطق برداشتن آیتم
    // ──────────────────────────────
    
    // این تابع آیتم فعلی را برمی‌دارد
    void PickupCurrentItem()
    {
        // اگر شیء قابل تعامل وجود ندارد، کاری نکن
        if (currentInteractable == null) return;
        
        // پیام دیباگ
        Debug.Log($"در حال برداشتن: {currentInteractable.item.itemName}");
        
        // ۱. انیمیشن برداشت را اجرا کن
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("Pickup"); // تریگر "Pickup" را در انیماتور فعال کن
        
        // ۲. آیتم را به جعبه ابزار اضافه کن
        if (toolboxUI != null)
        {
            // اگر سیستم UI جعبه ابزار داریم، از آن استفاده کن
            toolboxUI.AddItem(currentInteractable.item);
        }
        else
        {
            // در غیر این صورت، به لیست ساده اضافه کن
            inventory.Add(currentInteractable.item);
        }
        
        // ۳. شیء را از صحنه مخفی کن (اما کاملاً پاک نکن)
        // SetActive(false) شیء را غیرفعال می‌کند اما در حافظه باقی می‌ماند
        currentInteractable.gameObject.SetActive(false);
        
        // ۴. متن UI را مخفی کن
        HidePrompt();
        
        // ۵. مرجع شیء را پاک کن (چون برداشته شد)
        currentInteractable = null;
    }
    
    // ──────────────────────────────
    // بخش ۵: مدیریت UI
    // ──────────────────────────────
    
    // نمایش پیام تعامل در UI
    void ShowPrompt(string message)
    {
        // اگر کامپوننت Text وجود دارد
        if (interactionText != null)
        {
            // متن را تنظیم و نمایش بده
            interactionText.text = message;
            interactionText.gameObject.SetActive(true);
        }
    }
    
    // مخفی کردن پیام تعامل
    void HidePrompt()
    {
        // اگر کامپوننت Text وجود دارد، آن را مخفی کن
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }
    
    // ──────────────────────────────
    // بخش ۶: ابزارهای دیباگ
    // ─────────────────────────────ـ
    
    // این تابع فقط در Unity Editor و هنگام انتخاب شیء در Scene View اجرا می‌شود
    // برای نمایش محدوده‌های تشخیص به صورت گرافیکی
    void OnDrawGizmosSelected()
    {
        // ۱. کره سبز رنگ برای نشان دادن شعاع جستجو
        // رنگ: سبز با شفافیت ۳۰٪ (آلفا ۰.۳)
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, pickupRadius);
        
        // ۲. خط زرد رنگ برای نشان دادن جهت نگاه پلیر
        Gizmos.color = Color.yellow;
        Vector3 forwardEnd = transform.position + transform.forward * forwardDistance;
        Gizmos.DrawLine(transform.position, forwardEnd);
        
        // ۳. خطوط کناری برای نشان دادن مخروط دید
        float angle = 110f; // زاویه کل مخروط (۱۱۰ درجه)
        
        // چرخش جهت نگاه به چپ (نصف زاویه)
        Vector3 leftDir = Quaternion.Euler(0, -angle/2, 0) * transform.forward;
        // چرخش جهت نگاه به راست (نصف زاویه)
        Vector3 rightDir = Quaternion.Euler(0, angle/2, 0) * transform.forward;
        
        // کشیدن خط چپ
        Gizmos.DrawLine(transform.position, transform.position + leftDir * forwardDistance);
        // کشیدن خط راست
        Gizmos.DrawLine(transform.position, transform.position + rightDir * forwardDistance);
    }
    
    // ──────────────────────────────
    // بخش ۷: توابع کمکی (اختیاری)
    // ─────────────────────────────ـ
    
    // بررسی می‌کند که آیا آیتم خاصی در جعبه ابزار وجود دارد
    public bool HasItem(Item itemToCheck)
    {
        return inventory.Contains(itemToCheck);
    }
    
    // بررسی می‌کند که آیا آیتمی با نام خاص در جعبه ابزار وجود دارد
    public bool HasItemByName(string itemName)
    {
        foreach (Item item in inventory)
        {
            if (item.itemName == itemName)
                return true;
        }
        return false;
    }
    
    // تعداد آیتم‌های موجود در جعبه ابزار
    public int GetItemCount()
    {
        return inventory.Count;
    }
    
    // خالی کردن جعبه ابزار
    public void ClearInventory()
    {
        inventory.Clear();
        Debug.Log("جعبه ابزار خالی شد");
    }
}