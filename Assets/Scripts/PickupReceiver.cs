using UnityEngine;
using System.Collections.Generic;

public class PickupReceiver : MonoBehaviour
{
    private List<PickupController> linkedPickups = new List<PickupController>();

    public delegate void PickupHandler(PickupPayload payload);
    private event PickupHandler onPickupReceived;

    public void Subscribe(PickupHandler handler)
    {
        onPickupReceived += handler;
    }

    public void Unsubscribe(PickupHandler handler)
    {
        onPickupReceived -= handler;
    }

    public void Receive(PickupController pickup)
    {
        if (pickup != null) {
            pickup.OnReceived(this);
            if (onPickupReceived != null) {
                onPickupReceived(pickup.payload);
            }
        }
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