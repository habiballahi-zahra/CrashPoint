using TMPro;
using UnityEngine;

public class SimplePlayerPickup : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI")]
    public TextMeshProUGUI interactionText;   // متن UI تعامل

    public ToolboxUI toolboxUI;           // اسکریپت مدیریت جعبه ابزار


    private Interactable currentInteractable;
    private ItemSocket currentSocket;

    void Start()
    {
        HideUI();
    }

    void Update()
    {
        FindInteractable();

        if (Input.GetKeyDown(interactKey))
        {
            if (currentInteractable != null)
            {
                PickupItem();
            }
            else if (currentSocket != null)
            {
                PlaceItemInSocket();
            }
        }
    }

    // =========================
    // 🔍 پیدا کردن آیتم یا سوکت
    // =========================
    void FindInteractable()
    {
        currentInteractable = null;
        currentSocket = null;
        HideUI();

        Interactable[] interactables = FindObjectsOfType<Interactable>();
        float closestDistance = interactDistance;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance > closestDistance)
                continue;

            // 🟢 آیتم قابل برداشتن
            if (interactable.item != null)
            {
                currentInteractable = interactable;
                currentSocket = null;
                closestDistance = distance;

                ShowUI($"E : Collect {interactable.item.itemName}");
            }
            // 🔵 بررسی سوکت
            else
            {
                ItemSocket socket = interactable.GetComponent<ItemSocket>();
                if (socket == null || socket.isFilled)
                    continue;

                currentSocket = socket;
                currentInteractable = null;
                closestDistance = distance;

                if (Inventory.Instance != null &&
                    Inventory.Instance.HasItem(socket.requiredItem))
                {
                    ShowUI($"E : Place {socket.requiredItem.itemName}");
                }
                else
                {
                    ShowUI($"Need {socket.requiredItem.itemName}");
                }
            }
        }
    }

    // =========================
    // 🎒 برداشتن آیتم
    // =========================
    void PickupItem()
{
    Item item = currentInteractable.item;

    // اگر دارو است → مستقیم استفاده شود
    if (item.itemType == Item.ItemType.Health)
    {
        item.Use(gameObject); // پلیر
    }
    else
    {
        Inventory.Instance.AddItem(item);

        if (toolboxUI != null)
            toolboxUI.AddItem(item);
    }

    Destroy(currentInteractable.gameObject);
    currentInteractable = null;
    HideUI();
}


    // =========================
    // 🔌 جاگذاری در سوکت
    // =========================
    void PlaceItemInSocket()
    {
        Item requiredItem = currentSocket.requiredItem;

        if (!Inventory.Instance.HasItem(requiredItem))
            return;

        Inventory.Instance.RemoveItem(requiredItem);

           // آپدیت UI
        if (toolboxUI != null)
        {
            toolboxUI.RemoveItem(requiredItem);
        }
        currentSocket.PlaceItem();

        currentSocket = null;
        HideUI();

       
    }

    // =========================
    // 🖥 UI Helpers
    // =========================
    void ShowUI(string message)
    {
        if (interactionText == null) return;

        interactionText.gameObject.SetActive(true);
        interactionText.text = message;
    }

    void HideUI()
    {
        if (interactionText == null) return;

        interactionText.gameObject.SetActive(false);
    }
}
