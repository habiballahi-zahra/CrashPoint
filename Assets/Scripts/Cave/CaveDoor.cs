using UnityEngine;

public class CaveDoor : MonoBehaviour
{
    // ─────────────────────────────
    // تنظیمات حرکت در
    // ─────────────────────────────
    [Header("Door Movement")]

    // مقدار جابه‌جایی در هنگام باز شدن
    // مثال: به سمت چپ 3 متر
    public Vector3 openOffset = new Vector3(-3f, 0, 0);

    // سرعت باز شدن در
    public float openSpeed = 2f;

    // موقعیت اولیه (در بسته)
    private Vector3 closedPosition;

    // موقعیت نهایی (در باز)
    private Vector3 openPosition;

    // آیا در در حال باز شدن است؟
    private bool isOpening = false;

    // ─────────────────────────────
    // هنگام شروع بازی
    // ─────────────────────────────
    void Start()
    {
        // ذخیره موقعیت بسته
        closedPosition = transform.position;

        // محاسبه موقعیت باز
        openPosition = closedPosition + openOffset;
    }

    // ─────────────────────────────
    // هر فریم بررسی می‌کند
    // آیا باید در حرکت کند یا نه
    // ─────────────────────────────
    void Update()
    {
        if (isOpening)
        {
            // حرکت نرم در به سمت موقعیت باز
            transform.position = Vector3.MoveTowards(
                transform.position,
                openPosition,
                openSpeed * Time.deltaTime
            );
        }
    }

    // ─────────────────────────────
    // این تابع از بیرون صدا زده می‌شود
    // (مثلاً توسط Event سوکت)
    // ─────────────────────────────
    public void OpenDoor()
    {
        isOpening = true;
    }
}
