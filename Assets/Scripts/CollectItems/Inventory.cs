using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<Item> items = new List<Item>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Inventory Instance set");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        Debug.Log("Inventory ADD: " + item.itemName);
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }

    public void RemoveItem(Item item)
{
    items.Remove(item);
}

}

