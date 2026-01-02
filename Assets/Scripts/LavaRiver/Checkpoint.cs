using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;

    public void Activate(PlayerRespawn respawn)
    {
        if (activated) return;

        activated = true;
        respawn.SetCheckpoint(transform.position);

        Debug.Log("Checkpoint Activated: " + name);
    }
}
