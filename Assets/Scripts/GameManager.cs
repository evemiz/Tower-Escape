using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject enemiesParent1Prefab;
    public GameObject enemiesParent2Prefab;
    public GameObject enemiesParent3Prefab;
    public GameObject playerPrefab;
    public GameObject heartsParent1Prefab;
    public GameObject heartsParent2Prefab;
    public GameObject heartsParent3Prefab;

    private Transform currentenemiesParent;
    private Transform currentheartsParent;

    public int level;
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI levelText;

    public GameObject levelTransitionPanel;
    public TextMeshProUGUI levelTransitionText;

    public CanvasGroup levelTransitionGroup;


    int count;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        level = 1;
        levelText.text = "Level: 1";
        NextLevel();
        Count();
    }

    public void UpdateEnemyCount()
    {
        count -= 1;
        enemyCountText.text = "Enemies Left: " + count;

        if (count == 0)
        {
            if (level == 1)
            {
                level = 2;
                levelText.text = "Level: 2";
                NextLevel();
            }
            else if (level == 2)
            {
                level = 3;
                levelText.text = "Level: 3";
                NextLevel();
            }
            else
            {
                Debug.Log("You Win! --> from script EnemyCounterUI");
            }
        }
    }

    public void Count()
    {
        count = 0;
        foreach (Transform child in currentenemiesParent)
        {
            if (child != null && child.gameObject.activeInHierarchy)
            {
                count++;
            }
        }

        enemyCountText.text = "Enemies Left: " + count;
    }

    public void NextLevel()
    {

        if (level == 1)
        {
            StartCoroutine(ShowLevelTransition("Get Ready! Level 1 is starting..."));
            GameObject newPlayer = Instantiate(playerPrefab);
            Transform playerTransform = newPlayer.transform;
            InputManager inputManager = newPlayer.GetComponent<InputManager>();

            CameraManager camManager = FindObjectOfType<CameraManager>();
            if (camManager != null)
            {
                camManager.SetTarget(playerTransform, inputManager);
            }

            GameObject newEnemies = Instantiate(enemiesParent1Prefab);
            currentenemiesParent = newEnemies.transform;

            GameObject newHearts = Instantiate(heartsParent1Prefab);
            currentheartsParent = newHearts.transform;

            AssignPlayerToAllEnemies(playerTransform);
            Count();
        }
        if (level == 2)
        {
            StartCoroutine(ShowLevelTransition("Great job! Level 2 is starting..."));
            GameObject newEnemies = Instantiate(enemiesParent2Prefab);
            currentenemiesParent = newEnemies.transform;

            GameObject newHearts = Instantiate(heartsParent2Prefab);
            currentheartsParent = newHearts.transform;


            AssignPlayerToAllEnemies(GameObject.FindWithTag("Player").transform);
            Count();
        }
        else if (level == 3)
        {
            StartCoroutine(ShowLevelTransition("Final Stage! Level 3 begins now. Give it your all!"));
            GameObject newEnemies = Instantiate(enemiesParent3Prefab);
            currentenemiesParent = newEnemies.transform;

            GameObject newHearts = Instantiate(heartsParent3Prefab);
            currentheartsParent = newHearts.transform;

            AssignPlayerToAllEnemies(GameObject.FindWithTag("Player").transform);
            Count();
        }
    }

    public void AssignPlayerToAllEnemies(Transform playerTransform)
    {
        if (currentenemiesParent == null) return;

        AI[] aiEnemies = currentenemiesParent.GetComponentsInChildren<AI>();
        foreach (AI ai in aiEnemies)
        {
            ai.SetPlayer(playerTransform);
        }
    }
    private IEnumerator ShowLevelTransition(string message)
    {
        if (levelTransitionPanel != null && levelTransitionText != null)
        {
            Time.timeScale = 0f;

            levelTransitionText.text = message;
            levelTransitionPanel.SetActive(true);

            RectTransform panelRect = levelTransitionPanel.GetComponent<RectTransform>();

            if (level == 1)
            {
                panelRect.anchoredPosition = new Vector2(-Screen.width, 0);
                yield return new WaitForSecondsRealtime(1f);
            }

            panelRect.anchoredPosition = new Vector2(-Screen.width, 0);

            yield return StartCoroutine(SlideInAndOut(panelRect, 0.5f, 2f));

            levelTransitionPanel.SetActive(false);

            yield return new WaitForSecondsRealtime(0.5f);
            Time.timeScale = 1f;
        }
    }

    private IEnumerator SlideInAndOut(RectTransform panel, float duration, float waitTime)
    {
        Vector2 startPos = new Vector2(-Screen.width, 0); 
        Vector2 centerPos = Vector2.zero;
        Vector2 endPos = new Vector2(Screen.width, 0); 

        panel.anchoredPosition = startPos;
        yield return null; 

        float elapsed = 0f;

        while (elapsed < duration)
        {
            panel.anchoredPosition = Vector2.Lerp(startPos, centerPos, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        panel.anchoredPosition = centerPos;

        yield return new WaitForSecondsRealtime(waitTime);

        elapsed = 0f;
        while (elapsed < duration)
        {
            panel.anchoredPosition = Vector2.Lerp(centerPos, endPos, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        panel.anchoredPosition = endPos;
    }

}
