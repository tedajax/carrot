using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PickupPayload
{
    public string name;
}

public class PickupController : MonoBehaviour
{
    public bool pickupImmediately;
    public bool destroyOnPickup;
    public PickupPayload payload;

    public void OnReceived(PickupReceiver receiver)
    {
        if (destroyOnPickup) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        PickupReceiver receiver = collider.GetComponent<PickupReceiver>();

        if (receiver == null) {
            return;
        }

        if (pickupImmediately) {
            receiver.Receive(this);
        }
        else {
            receiver.Link(this);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        PickupReceiver receiver = collider.GetComponent<PickupReceiver>();

        if (receiver == null) {
            return;
        }

        receiver.Unlink(this);
    }
}