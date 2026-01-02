using UnityEngine;

public class PlayerTriggerReceiver : MonoBehaviour
{
    private PlayerRespawn respawn;

    private void Awake()
    {
        respawn = GetComponent<PlayerRespawn>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // گدازه
        if (other.CompareTag("Lava"))
        {
            respawn.Respawn();
            return;
        }

        // چک‌پوینت
        if (other.CompareTag("Checkpoint"))
        {
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            if (checkpoint != null)
                checkpoint.Activate(respawn);
        }
    }
}
