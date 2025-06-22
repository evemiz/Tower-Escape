using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image healthbarSprite;
    public float reduceSpeed = 2;
    public float target = 1;
    private Camera cam;

    private void Awake()
    {
        if (cam == null)
        {
            GameObject camObj = GameObject.FindWithTag("MainCamera");
            if (camObj != null)
            {
                cam = camObj.GetComponent<Camera>();
                Debug.Log("✅ Healthbar: Found camera with tag 'MainCamera': " + cam.name);
            }
            else
            {
                Debug.LogWarning("⚠️ Healthbar: No camera found with tag 'MainCamera'");
            }
        }
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        healthbarSprite.fillAmount = Mathf.MoveTowards(healthbarSprite.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
}
