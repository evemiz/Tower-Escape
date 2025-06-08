using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int swordsToKill = 3; // מספר חרבות שצריך בשביל להרוג את האויב

    private TextMeshProUGUI swordCountText;

    void Start()
    {
        // חיפוש טקסט בתוך הילדים של האויב
        swordCountText = GetComponentInChildren<TextMeshProUGUI>();

        UpdateText();
    }

    // קריאה מהחרב כשפוגעת
    public void TakeHit()
    {
        swordsToKill--;

        if (swordsToKill <= 0)
        {
            KillEnemy();
        }
        else
        {
            UpdateText();
        }
    }

    // קריאה מהשחקן (אם יש לו מספיק חרבות)
    public void KillEnemy()
    {
        GameManager.Instance.EnemyKilled();
        Destroy(gameObject);
    }

    // עדכון הכיתוב מעל הראש
    private void UpdateText()
    {
        if (swordCountText != null)
        {
            swordCountText.text = swordsToKill.ToString();
        }
    }
}
