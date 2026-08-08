using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<int, int> OnHealthChanged;
    public event Action<Vector2> OnDamaged;
    public event Action<Vector2> OnDeath;

    public int health;
    public int maxHealth;

    [Header("Popup")]
    public GameObject healthPopup;

    public void SetHealth(int amount)
    {
        health = amount;
        OnHealthChanged?.Invoke(health, maxHealth);
    }

    public void ChangeHealth(int amount, Vector2 sourcePosition, bool showPopup = true)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        OnHealthChanged?.Invoke(health, maxHealth);

        if (healthPopup != null && showPopup)
        {
            var popup = Instantiate(healthPopup, transform.position, Quaternion.identity);
            popup.GetComponent<HealthPopup>().Setup(amount);
        }

        if (health <= 0)
        {
            OnDeath?.Invoke(sourcePosition);
        }

        else if (amount < 0)
        {
            OnDamaged?.Invoke(sourcePosition);
        }
    }

    public void ChangeMaxHealth(int newMaxHealth)
    {
        int difference = newMaxHealth - maxHealth;
        maxHealth += difference;

        ChangeHealth(difference, Vector2.zero, false);
    }
}
