using UnityEngine;
using TMPro;

public class EnemyTrigger : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    public TextMeshProUGUI swordCountText;
    public GameObject gameOverCanvas;
    private CameraManager cameraManager;

    public AudioClip enemyKilledSound;
    public AudioClip gameOverSound;
    public float volume = 1f;

    public ParticleSystem explosionParticle;


    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCollector collector = other.GetComponent<PlayerCollector>();

            if (collector != null)
            {
                if (collector.swordsCollected >= enemyHealth.swordsToKill)
                {
                    Debug.Log("Player had enough swords. Enemy destroyed.");

                    // הורדת חרבות ועדכון תצוגה
                    collector.swordsCollected -= enemyHealth.swordsToKill;
                    UpdateUI(collector.swordsCollected);

                    // הריגת האויב
                    enemyHealth.KillEnemy();

                    if (enemyKilledSound != null)
                    {
                        AudioSource.PlayClipAtPoint(enemyKilledSound, transform.position, volume);
                    }

                    if (explosionParticle != null)
                    {
                        ParticleSystem particle = Instantiate(explosionParticle, transform.position, Quaternion.identity);
                        particle.Play();
                        Destroy(particle.gameObject, particle.main.duration + particle.main.startLifetime.constant);
                    }
                }
                else
                {
                    GameOver();
                }
            }
        }
    }

    void GameOver()
    {
        Time.timeScale = 0;

        if (cameraManager != null)
        {
            cameraManager.isCameraLocked = true;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

       if (gameOverSound != null)
        {
            AudioSource.PlayClipAtPoint(gameOverSound, transform.position, volume);
        }
    }

    private void UpdateUI(int swordsCollected)
    {
        if (swordCountText != null)
        {
            swordCountText.text = swordsCollected.ToString();
        }
    }
}
