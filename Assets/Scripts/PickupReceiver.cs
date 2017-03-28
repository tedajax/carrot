using UnityEngine;
using System.Collections.Generic;

public class PickupReceiver : MonoBehaviour
{
    private List<PickupController> linkedPickups = new List<PickupController>();

    public delegate void PickupHandler(PickupPayload payload);
    public delegate bool CanPickupCallback(PickupPayload payload);

    public event PickupHandler onPickupReceived;
    public event CanPickupCallback canPickupCallback;

    public bool Receive(PickupController pickup)
    {
        if (pickup != null) {
            if (canPickupCallback != null) {
                if (!canPickupCallback(pickup.payload)) {
                    return false;
                }
            }

            pickup.OnReceived(this);
            if (onPickupReceived != null) {
                onPickupReceived(pickup.payload);
                return true;
            }
        }
        return false;
    }

    public void Link(PickupController pickup)
    {
        if (!linkedPickups.Contains(pickup)) {
            linkedPickups.Add(pickup);
        }
    }

    public void Unlink(PickupController pickup)
    {
        if (linkedPickups.Contains(pickup)) {
            linkedPickups.Remove(pickup);
        }
    }

    public void AttemptPickup()
    {
        if (linkedPickups.Count > 0) {
            Receive(linkedPickups[linkedPickups.Count - 1]);
        }
    }
}