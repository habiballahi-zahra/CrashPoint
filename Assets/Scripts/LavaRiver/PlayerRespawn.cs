using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    // آخرین چک‌پوینت محلی (برای مرگ داخل Scene)
    private Vector3 lastCheckpoint;

    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // اگر از قبل چک‌پوینت ذخیره شده (مثلاً برگشت از Scene2)
        if (CheckpointManager.Instance != null &&
            CheckpointManager.Instance.hasCheckpoint)
        {
            lastCheckpoint = CheckpointManager.Instance.lastCheckpoint;

            // انتقال امن پلیر
            controller.enabled = false;
            transform.position = lastCheckpoint;
            controller.enabled = true;
        }
        else
        {
            // اولین ورود به بازی
            lastCheckpoint = transform.position;
        }
    }

    /// <summary>
    /// ثبت چک‌پوینت جدید
    /// </summary>
    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;

        // ذخیره Global برای Sceneهای بعدی
        CheckpointManager.Instance.lastCheckpoint = newCheckpoint;
        CheckpointManager.Instance.hasCheckpoint = true;

        Debug.Log("Checkpoint Set: " + newCheckpoint);
    }

    /// <summary>
    /// ری‌اسپاون (مثلاً افتادن توی گدازه)
    /// </summary>
    public void Respawn()
    {
        Debug.Log("Respawn!");

        controller.enabled = false;
        transform.position = lastCheckpoint;
        controller.enabled = true;
    }
}
