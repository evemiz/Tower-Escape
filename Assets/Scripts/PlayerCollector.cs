using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCollector : MonoBehaviour
{
    public int swordsCollected = 0;
    public TextMeshProUGUI swordCountText;

    public void CollectSword()
    {
        swordsCollected++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (swordCountText != null)
        {
            swordCountText.text = swordsCollected.ToString();
        }
    }
}