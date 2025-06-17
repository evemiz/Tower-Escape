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

            currentHealth--;
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                
                if (this.CompareTag("AI"))
                {
                    EnemyCounterUI.Instance.UpdateEnemyCount();
                }

                if (playerManager != null)
                {
                    playerManager.GameOver();
                }
            }
            else
            {
                healthbar.UpdateHealthBar(maxHealth, currentHealth);
                Debug.Log("Hit");
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Heart") && this.CompareTag("Player"))
        {
            Debug.Log("other: " + other + ", this: " + this);
            currentHealth++;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
                Transform canvas = other.transform.Find("Canvas");
                if (canvas != null)
                {
                    canvas.gameObject.SetActive(true);
                    StartCoroutine(HideFloatingTextAfterDelay(canvas.gameObject, 2f));
                }
            }
            else
            {
                healthbar.UpdateHealthBar(maxHealth, currentHealth);
                Destroy(other.gameObject);
            }
        }

    }

    private IEnumerator HideFloatingTextAfterDelay(GameObject textObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (textObject != null)
            textObject.SetActive(false);
    }

}
