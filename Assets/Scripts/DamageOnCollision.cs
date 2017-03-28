using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    public float damage = 1f;
    public bool damageSelf = true;

    void OnCollisionEnter2D(Collision2D collision)
    {
        var health = collision.gameObject.GetComponent<HealthProperty>();

        if (health != null) {
            health.Damage(damage);
        }

        if (damageSelf) {
            var rockHealth = GetComponent<HealthProperty>();
            if (rockHealth != null) {
                rockHealth.Damage(damage);
            }
        }
    }
}