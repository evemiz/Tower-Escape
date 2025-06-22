using UnityEngine;
using TMPro;

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
            GameObject newEnemies = Instantiate(enemiesParent2Prefab);
            currentenemiesParent = newEnemies.transform;

            GameObject newHearts = Instantiate(heartsParent2Prefab);
            currentheartsParent = newHearts.transform;


            AssignPlayerToAllEnemies(GameObject.FindWithTag("Player").transform);
            Count();
        }
        else if (level == 3)
        {
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

}
