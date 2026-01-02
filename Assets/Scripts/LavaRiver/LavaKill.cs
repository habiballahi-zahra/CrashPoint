using UnityEngine;

public class LavaKill : MonoBehaviour
{
    private PlayerRespawn respawn;

    private void Start()
    {
        respawn = GetComponent<PlayerRespawn>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // اگر پلیر وارد گدازه شد
        if (other.CompareTag("Lava"))
        {
            Debug.Log("Entered Trigger: " + other.name);

            respawn.Respawn();
        }
    }
}
