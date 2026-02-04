using UnityEngine;

/// <summary>
/// این کلاس فقط وظیفه نگه‌داری آخرین چک‌پوینت رو بین Sceneها داره
/// هیچ کنترلی روی پلیر نداره
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    // آخرین چک‌پوینت ذخیره شده
    public Vector3 lastCheckpoint;

    // آیا تا الان چک‌پوینتی ثبت شده؟
    public bool hasCheckpoint = false;

    private void Awake()
    {
        // اگر قبلاً ساخته شده، این یکی رو نابود کن
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // این آبجکت بین Sceneها باقی می‌مونه
        DontDestroyOnLoad(gameObject);
    }
}
