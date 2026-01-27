using UnityEngine;
using UnityEngine.Events;

public class ItemSocket : MonoBehaviour
{
    [Header("Socket Settings")]
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
