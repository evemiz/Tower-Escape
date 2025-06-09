using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarPlayer : MonoBehaviour
{
    public float maxHealth = 3;
    public GameObject deathEffect, hitEffect;
    public float currentHealth;
    public Healthbar healthbar;
    private PlayerManager playerManager;

    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.UpdateHealthBar(maxHealth, currentHealth);
        playerManager = GetComponent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fireball"))
        {
            if (this.CompareTag("AI"))
            {
                GetComponent<AI>().TakeDamage();
            }
            
            currentHealth -= Random.Range(0.5f, 1.5f);
            if (currentHealth <= 0)
            {
                // Instantiate(deathEffect, transform.position, Quaternion.Euler(-90, 0, 0));
                Destroy(gameObject);
                if (playerManager != null)
                {
                    playerManager.GameOver();
                }
            }
            else
            {
                // Instantiate(hitEffect, transform.position, Quaternion.identity);
                healthbar.UpdateHealthBar(maxHealth, currentHealth);
                Debug.Log("Hit");
            }
            Destroy(other.gameObject);
        }
    }

}
