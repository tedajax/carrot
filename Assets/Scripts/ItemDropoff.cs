using UnityEngine;

public interface IPickupHolder
{
    HeldItem PopItem();
}

[System.Serializable]
public struct ItemDropoffConfig
{
    public float initialDelay;
    public float dropoffInterval;
    public float baseScore;
    public float chainMultiplier;
}

public class ItemDropoff : MonoBehaviour
{
    public ItemDropoffConfig config;

    private IPickupHolder activeHolder;
    private float dropoffTimer;
    private int multiplier;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (activeHolder != null) {
            return;
        }

        var holder = collider.GetComponent<IPickupHolder>();
        if (holder != null) {
            onHolderArrive(holder);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        var holder = collider.GetComponent<IPickupHolder>();
        if (holder == activeHolder) {
            onHolderLeave();
        }
    }

    private void onHolderArrive(IPickupHolder holder)
    {
        activeHolder = holder;
        multiplier = 1;
        dropoffTimer = config.initialDelay;
    }

    private void onHolderLeave()
    {
        activeHolder = null;
    }

    void Update()
    {
        if (activeHolder == null) {
            return;
        }

        if (dropoffTimer <= 0f) {
            dropoffTimer += config.dropoffInterval;
            HeldItem item = activeHolder.PopItem();

            if (item != null) {
                int score = calculateScore(item);
                GameManagerLocator.GameManager.hudController.CreateScoreText(score, item.transform.position);
                ++multiplier;
                Destroy(item.gameObject);
            }
            else {
                onHolderLeave();
            }
        }

        dropoffTimer -= Time.deltaTime;
    }

    private int calculateScore(HeldItem item)
    {
        return (int)(config.baseScore * Mathf.Pow(config.chainMultiplier, multiplier));
    }
}