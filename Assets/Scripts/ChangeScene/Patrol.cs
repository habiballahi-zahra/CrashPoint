using UnityEngine;

public class Portal : MonoBehaviour
{
    public string targetScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneTransition.Instance.LoadScene(targetScene);
        }
    }
}
