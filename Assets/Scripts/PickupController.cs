using UnityEngine;

[System.Serializable]
public class PickupPayload
{
    public string name;
}

public class PickupController : MonoBehaviour
{
    public bool pickupOnContact;
    public bool destroyOnPickup;
    public float grabTime = 0f;
    public PickupPayload payload;

    public delegate void OnPickupHandler();
    public event OnPickupHandler onPickup;

    public void OnReceived(PickupReceiver receiver)
    {
        if (onPickup != null) {
            onPickup();
        }

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

        if (pickupOnContact) {
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