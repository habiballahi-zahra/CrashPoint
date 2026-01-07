using UnityEngine;

public class ItemSocket : MonoBehaviour
{
    [Header("Socket Settings")]
    public Item requiredItem;          // کریستالی که لازم دارد
    public Transform placePoint;       // محل قرار گرفتن کریستال
    public GameObject placedVisual;    // مدل کریستال روی در

    [Header("State")]
    public bool isFilled = false;

    public void PlaceItem()
    {
        if (isFilled)
            return;

        isFilled = true;

        if (placedVisual != null)
            placedVisual.SetActive(true);

        Debug.Log("Crystal placed in socket");

        // اینجا بعداً در غار باز میشه
        SendMessage("OnSocketFilled", SendMessageOptions.DontRequireReceiver);
    }
}
