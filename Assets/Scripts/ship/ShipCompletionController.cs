using UnityEngine;

public class ShipCompletionController : MonoBehaviour
{
    [Header("Sockets")]
    // لیست تمام سوکت‌های سفینه
    public ItemSocket[] shipSockets;

    [Header("Win UI")]
    // پنل برد
    public GameObject winPanel;

    private bool isCompleted = false;

    void Start()
    {
        // مطمئن شو پنل برد اول مخفی است
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    // ─────────────────────────────
    // این تابع بعد از هر جاگذاری صدا زده می‌شود
    // ─────────────────────────────
    public void CheckCompletion()
    {
        // اگر قبلاً کامل شده، دوباره چک نکن
        if (isCompleted) return;

        // بررسی تمام سوکت‌ها
        foreach (ItemSocket socket in shipSockets)
        {
            // اگر حتی یکی خالی بود
            if (!socket.isFilled)
                return;
        }

        // اگر به اینجا رسیدیم یعنی همه پر هستند
        CompleteShip();
    }

    // ─────────────────────────────
    // وقتی سفینه کامل شد
    // ─────────────────────────────
    void CompleteShip()
    {
        isCompleted = true;

        Debug.Log("Ship completed!");

        if (winPanel != null)
            winPanel.SetActive(true);

        // اینجا می‌تونی:
        // - انیمیشن بزنی
        // - موزیک برد پخش کنی
        // - تایم رو متوقف کنی
    }
}
