using System.Collections.Generic;
using UnityEngine;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance;

    // آیتم‌هایی که برداشته شده‌اند
    private HashSet<string> pickedItems = new HashSet<string>();

    // سوکت‌هایی که پر شده‌اند
    private HashSet<string> filledSockets = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ─────────────── Items ───────────────
    public void MarkItemPicked(string itemID)
    {
        pickedItems.Add(itemID);
    }

    public bool IsItemPicked(string itemID)
    {
        return pickedItems.Contains(itemID);
    }

    // ─────────────── Sockets ─────────────
    public void MarkSocketFilled(string socketID)
    {
        filledSockets.Add(socketID);
    }

    public bool IsSocketFilled(string socketID)
    {
        return filledSockets.Contains(socketID);
    }
}
