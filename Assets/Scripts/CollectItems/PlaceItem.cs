using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItem : MonoBehaviour
{

    [Header("State")]
    // مشخص می‌کند آیا سوکت پر شده یا نه
      public bool isFilled = false;

    // [Header("Socket Settings")]
    // public Item requiredItem;          // کریستالی که لازم دارد
    // public Transform itemPlacePoint;       // محل قرار گرفتن کریستال
    // public GameObject placedVisual;    // مدل کریستال روی در


   
    public void PlaceItemInSoket(GameObject placedVisual)
    {
        if (isFilled)
            return;

        isFilled = true;

        if (placedVisual != null)
            placedVisual.SetActive(true);
    }
}
