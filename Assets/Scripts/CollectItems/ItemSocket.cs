using UnityEngine;
using UnityEngine.Events;

public class ItemSocket : MonoBehaviour
{
    [Header("Socket Settings")]
    public string socketID;
    public Item requiredItem;          // کریستالی که لازم دارد
    public Transform itemPlacePoint;       // محل قرار گرفتن کریستال
    public GameObject placedVisual;    // مدل کریستال روی در
    

    public bool isFilled = false;    // مشخص می‌کند آیا سوکت پر شده یا نه

    // ─────────────────────────────
    // Event عمومی
    // هر چیزی می‌تواند به این گوش دهد
    // (در، نور، دشمن، کات‌سین و...)
    // ─────────────────────────────
    public UnityEvent OnItemPlaced;

    // ─────────────────────────────
    // این تابع زمانی صدا زده می‌شود
    // که پلیر آیتم درست را جاگذاری کند
    // ─────────────────────────────
    
    private void Start() {
         if (SceneStateManager.Instance.IsSocketFilled(socketID))
{
    RestoreFilledState();
}
    }
   
 void RestoreFilledState()
    {
        isFilled = true;

        // آیتم داخل سوکت دیده بشه
        if (placedVisual != null)
            placedVisual.SetActive(true);

        // اگر Collider داری که تعامل رو قطع کنه
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }
    
    public void PlaceItem()
    {
        // اگر قبلاً پر شده کاری نکن
        if (isFilled) return;

        // سوکت را پر شده علامت بزن
        isFilled = true;

        Debug.Log("Item placed in socket");

        // اجرای Event
        // یعنی به همه Listener ها خبر بده
        OnItemPlaced?.Invoke();
    }
}
