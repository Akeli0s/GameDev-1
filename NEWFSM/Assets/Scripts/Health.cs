using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider healthBar;
    public int maxHealth;
    public float currentHealth;

    public event Action<GameObject> OnDeath; // Event to notify death

    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if (currentHealth <= 0)
        {
            Die(); // Call Die method when health drops to 0
        }
    }

    void Die()
    {
        OnDeath?.Invoke(gameObject); // Invoke the death event
        Destroy(gameObject);
    }
}
