using UnityEngine;

public enum CarrotItemState
{
    Held,
    Dropped
}

public class CarrotItemController : MonoBehaviour
{
    public float smoothTimeBase = 0.05f;
    public float smoothTimeInterval = 0.1f;
    private Vector3 velocity;
    private HeldItem heldItem;
    private CarrotItemState state;

    void Awake()
    {
        heldItem = GetComponent<HeldItem>();
        state = CarrotItemState.Held;

        heldItem.onRemoved += onRemovedFromInventory;

        GetComponent<HealthProperty>().onDeath += onDeath;
    }

    private void onDeath(GameObject sender)
    {
        heldItem.Inventory.RemoveAllItems();
    }

    private void onRemovedFromInventory(HeldItem item)
    {
        state = CarrotItemState.Dropped;
        velocity.x = GameManagerLocator.GameManager.Random.NextFloat(-5f, 5f);
        velocity.y = 0f;
    }

    void Update()
    {
        if (state == CarrotItemState.Held) {
            Transform targetTransform = null;
            float smoothTime = 0.5f;

            if (heldItem != null) {
                targetTransform = heldItem.Transform;
                smoothTime = heldItem.Index * smoothTimeInterval + smoothTimeBase;
            }

            if (targetTransform == null) {
                return;
            }

            Vector3 position = transform.position;
            position = Vector3.SmoothDamp(position, targetTransform.position, ref velocity, smoothTime);
            transform.position = position;
        }
        else if (state == CarrotItemState.Dropped) {
            velocity.y -= GameManagerLocator.GameManager.gameConfig.gravity * Time.deltaTime;
            Vector3 position = transform.position;
            position += velocity * Time.deltaTime;
            transform.position = position;

            if (position.y < -8f) {
                Destroy(gameObject);
            }
        }
    }
}