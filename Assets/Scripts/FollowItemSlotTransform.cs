using UnityEngine;

public class FollowItemSlotTransform : MonoBehaviour
{
    public float smoothTimeBase = 0.05f;
    public float smoothTimeInterval = 0.1f;
    private Vector3 velocity;
    private HeldItem heldItem;

    void Awake()
    {
        heldItem = GetComponent<HeldItem>();
    }

    void Update()
    {
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
}