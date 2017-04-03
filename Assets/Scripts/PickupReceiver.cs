using UnityEngine;
using System.Collections.Generic;

public class PickupReceiver : MonoBehaviour
{
    private List<PickupController> linkedPickups = new List<PickupController>();
    private PickupController activeGrabPickup;
    private float grabTimer;

    public bool IsGrabbing { get { return activeGrabPickup != null; } }
    public PickupController GrabbedPickup { get { return activeGrabPickup; } }

    public delegate void PickupHandler(PickupController pickup);
    public delegate bool CanPickupCallback(PickupController pickup);

    public event PickupHandler onPickupReceived;
    public event CanPickupCallback canPickupCallback;

    public bool Receive(PickupController pickup)
    {
        if (pickup != null) {
            if (canPickupCallback != null) {
                if (!canPickupCallback(pickup)) {
                    return false;
                }
            }

            activeGrabPickup = null;
            Unlink(pickup);
            pickup.OnReceived(this);
            if (onPickupReceived != null) {
                onPickupReceived(pickup);
            }
            return true;
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

    public void StartGrab()
    {
        if (activeGrabPickup != null) {
            return;
        }

        PickupController pickup = GetAvailablePickup();
        if (pickup == null) {
            return;
        }

        if (pickup.grabTime > 0f) {
            activeGrabPickup = pickup;
            grabTimer = pickup.grabTime;
        }
        else {
            Receive(pickup);
        }
    }

    public void EndGrab()
    {
        activeGrabPickup = null;
    }

    public PickupController GetAvailablePickup()
    {
        if (linkedPickups.Count > 0) {
            return linkedPickups[linkedPickups.Count - 1];
        }
        return null;
    }

    void Update()
    {
        if (IsGrabbing) {
            grabTimer -= Time.deltaTime;
            if (grabTimer <= 0f) {
                if (Receive(activeGrabPickup)) {
                    activeGrabPickup = null;
                }
            }
        }
    }
}