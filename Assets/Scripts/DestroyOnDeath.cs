using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    void Awake()
    {
        var health = GetComponent<HealthProperty>();
        if (health != null) {
            health.onDeath += onDeath;
        }
        else {
            Debug.LogError("DestroyOnDeath component requires a HealthProperty to work.");
        }
    }

    private void onDeath(GameObject sender)
    {
        if (gameObject != sender) {
            Debug.LogWarning("gameObject/sender mismatch in DestroyOnDeath.  Somehow we are trying to destroy something that we are not on.");
            return;
        }

        Destroy(gameObject);
    }
}