using UnityEngine;
using TMPro;

public class EnemyCounterUI : MonoBehaviour
{
    public static EnemyCounterUI Instance;

    public Transform enemiesParent;
    public TextMeshProUGUI enemyCountText;

    int count;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Count();
    }

    public void UpdateEnemyCount()
    {
        count--;
        enemyCountText.text = "Enemies Left: " + count;
    }

    public void Count()
    {
        count = 0;
        foreach (Transform child in enemiesParent)
        {
            if (child != null && child.gameObject.activeInHierarchy)
            {
                count++;
            }
        }

        enemyCountText.text = "Enemies Left: " + count;
    }


}
