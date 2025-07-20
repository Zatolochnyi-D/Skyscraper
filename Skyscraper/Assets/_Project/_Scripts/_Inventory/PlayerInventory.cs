using System;
using System.Collections.Generic;
using System.Linq;
using ThreeDent.DevelopmentTools;
using UnityEngine;

[Serializable]
public struct ItemAmountPair
{
    public InventoryItem item;
    public int amount;
}

public class PlayerInventory : Singleton<PlayerInventory>
{
    public event Action<InventoryItem> OnItemDepleted;
    public event Action<int> OnItemsCountUpdated;

    [SerializeField] private ItemAmountPair[] items;

    public int ItemsCount => items.Length;
    public IEnumerable<InventoryItem> Items => items.Select(x => x.item);

    public InventoryItem GetItem(int index)
    {
        return items[index].item;
    }

    public int GetItemAmount(int index)
    {
        return items[index].amount;
    }

    public GameObject GetPrefab(int index)
    {
        var prefab = items[index].item.blockPrefab;
        items[index].amount--;
        OnItemsCountUpdated?.Invoke(index);
        return prefab;
    }
}