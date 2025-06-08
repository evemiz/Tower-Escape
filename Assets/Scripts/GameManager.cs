using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Light towerGlowLight;
    public GameObject towerIndicator;

    public TextMeshProUGUI enemiesLeftText;

    private int totalEnemies;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        UpdateEnemiesLeftUI();

        if (towerGlowLight != null)
            towerGlowLight.enabled = false;

        if (towerIndicator != null)
            towerIndicator.SetActive(false);
    }

    public void EnemyKilled()
    {
        totalEnemies--;
        UpdateEnemiesLeftUI();

        if (totalEnemies <= 0)
        {
            Debug.Log("All enemies defeated! You win!");
            if (towerGlowLight != null)
            {
                towerGlowLight.enabled = true;
            }

            if (towerIndicator != null)
            {
                towerIndicator.SetActive(true);
            }

        }
    }

    private void UpdateEnemiesLeftUI()
    {
        if (enemiesLeftText != null)
        {
            enemiesLeftText.text = "Enemies Left: " + totalEnemies;
        }
    }
}
