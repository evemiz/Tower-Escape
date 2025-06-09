using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocomotion playerLocomotion;
    public GameObject gameOverCanvas;

    private bool isLoadingNextLevel = false;

    public bool isInteracting;

    public AudioClip levelCompleteSound;
    public float levelCompleteVolume = 1f;

    public AudioClip gameOverSound;
    public float volume = 1f;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ocean"))
        {
            GameOver();
        }
        if (other.CompareTag("NextLevel"))
        {
            LoadNextLevel();
        }
    }

    public void GameOver()
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


    public void LoadNextLevel()
    {
        if (isLoadingNextLevel) return;
        isLoadingNextLevel = true;

        StartCoroutine(LoadNextLevelSequence());
    }

    IEnumerator LoadNextLevelSequence()
    {
        Time.timeScale = 1f;

        if (levelCompleteSound != null)
        {
            AudioSource.PlayClipAtPoint(levelCompleteSound, Camera.main.transform.position, levelCompleteVolume);
            yield return new WaitForSecondsRealtime(levelCompleteSound.length);
        }

        FadeManager.Instance.FadeToScene("SecondLevel");
    }
}

