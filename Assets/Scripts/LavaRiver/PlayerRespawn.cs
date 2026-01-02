using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 lastCheckpoint;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // اولین چک‌پوینت = محل شروع بازی
        lastCheckpoint = transform.position;
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
        Debug.Log("Checkpoint Set: " + newCheckpoint);
    }

    

    public void Respawn()
    {
        Debug.Log("Respawn!");

        // خاموش کردن موقت CharacterController
        controller.enabled = false;

        // انتقال پلیر
        transform.position = lastCheckpoint;

        // روشن کردن دوباره
        controller.enabled = true;
    }
}
