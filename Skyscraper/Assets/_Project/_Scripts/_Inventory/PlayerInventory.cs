using System;
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
    [SerializeField] private ItemAmountPair[] items;
}