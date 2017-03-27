using UnityEngine;
using System.Collections.Generic;

public class Inventory
{
    class ItemSlot
    {
        public Transform itemTransform;
        public HeldItem heldItem;
    }

    private List<ItemSlot> itemSlots;
    public int headIndex;

    public int AvailableSlots
    {
        get { return itemSlots.Count - headIndex; }
    }

    public bool HasAvailableSpace
    {
        get { return AvailableSlots > 0; }
    }

    public int ItemCount
    {
        get { return headIndex; }
    }

    public bool HasItems
    {
        get { return ItemCount > 0; }
    }

    public Inventory(Transform[] itemTransforms)
    {
        itemSlots = new List<ItemSlot>();

        for (int i = 0; i < itemTransforms.Length; ++i) {
            itemSlots.Add(new ItemSlot() { itemTransform = itemTransforms[i], heldItem = null });
        }

        headIndex = 0;
    }

    public void AddItem(HeldItem heldItem)
    {
        if (headIndex >= itemSlots.Count) {
            return;
        }

        ItemSlot slot = itemSlots[headIndex];
        heldItem.OnAddedToInventory(this, headIndex, slot.itemTransform);
        ++headIndex;

        slot.heldItem = heldItem;
    }

    public void RemoveItem(int index)
    {
        itemSlots[index].heldItem = null;

        for (int i = index; i < itemSlots.Count - 1; ++i) {
            if (itemSlots[i + 1].heldItem != null) {
                itemSlots[i].heldItem = itemSlots[i + 1].heldItem;
                itemSlots[i + 1].heldItem = null;
                itemSlots[i].heldItem.OnAddedToInventory(this, i, itemSlots[i].itemTransform, false);
            }
        }

        --headIndex;
    }

    public HeldItem PopItem()
    {
        if (headIndex == 0) {
            return null;
        }

        HeldItem result = itemSlots[headIndex - 1].heldItem;
        RemoveItem(headIndex - 1);
        return result;
    }
}