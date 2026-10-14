using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    int health;

    void Start()
    {
        health = maxHealth;
        GameEvents.HealthChanged(health);
    }

    void OnEnable()  => GameEvents.OnItemCaught += HandleCaught;
    void OnDisable() => GameEvents.OnItemCaught -= HandleCaught;

    void HandleCaught(ItemType type)
    {
        if (type != ItemType.Bomb) return;
        health--;
        GameEvents.HealthChanged(health);
        if (health <= 0) GameEvents.GameOver();
    }
}