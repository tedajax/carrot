using UnityEngine;

public class HeldItem : MonoBehaviour
{
    public Inventory Inventory { get; private set; }
    public int Index { get; private set; }
    public Transform Transform { get; private set; }

    public delegate void OnRemovedHandler(HeldItem item);
    public event OnRemovedHandler onRemoved;

    void Awake()
    {
        var health = GetComponent<HealthProperty>();
        if (health != null) {
            health.onDeath += (sender) => { RemoveFromInventory(); };
        }
    }

    public void OnAddedToInventory(Inventory itemHold, int slotIndex, Transform slotTransform, bool snapToSlot)
    {
        Inventory = itemHold;
        Index = slotIndex;
        Transform = slotTransform;

        if (snapToSlot) {
            // immediately set position of the held item to the transform of the item slot.
            transform.SetParent(Transform, false);
            transform.SetParent(null);
        }
    }

    public void RemoveFromInventory()
    {
        if (Inventory != null) {
            Inventory.RemoveItem(Index);
        }
    }

    public void Clear()
    {
        Inventory = null;
        Transform = null;
        Index = -1;
    }

    public void OnRemoved()
    {
        if (onRemoved != null) {
            onRemoved(this);
        }
    }
}