using System;
using System.Collections.Generic;
using System.Linq;
using ThreeDent.DevelopmentTools;
using ThreeDent.EventBroker;
using UnityEngine;

[Serializable]
public struct ItemAmountPair
{
    public InventoryItem item;
    public int amount;
}

public class PlayerInventory : Singleton<PlayerInventory>
{
    public event Action<int> OnItemDepleted;
    public event Action<int> OnItemsCountUpdated;

    [SerializeField] private ItemAmountPair[] items;

    public int ItemsCount => items.Length;
    public IEnumerable<InventoryItem> Items => items.Select(x => x.item);

    private bool CheckIfInventoryIsEmpty()
    {
        return items.Select(x => x.amount).Sum() == 0;
    }

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
        if (CheckIfInventoryIsEmpty())
            EventBroker.Invoke<InventoryEmptyEvent>();
        if (items[index].amount == 0)
            OnItemDepleted?.Invoke(index);
        return prefab;
    }

    public bool IsDepleted(int index)
    {
        return items[index].amount == 0;
    }

    public void AddItem(int index)
    {
        if (CheckIfInventoryIsEmpty())
            EventBroker.Invoke<InventoryResuppliedEvent>();
        items[index].amount++;
        OnItemsCountUpdated?.Invoke(index);
    }
}

public class InventoryEmptyEvent : IEvent { }
public class InventoryResuppliedEvent : IEvent { }