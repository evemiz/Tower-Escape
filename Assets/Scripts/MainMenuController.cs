using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject logoImage;
    public GameObject startButton;
    public GameObject howToPlayButton;
    public GameObject audioButton;
    public GameObject howToPlayImage;
    public GameObject player;
    public GameObject returnButton;



    public void ShowHowToPlay()
    {
        logoImage.SetActive(false);
        startButton.SetActive(false);
        audioButton.SetActive(false);
        howToPlayButton.SetActive(false);
        player.SetActive(false);

        howToPlayImage.SetActive(true);
        returnButton.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        logoImage.SetActive(true);
        startButton.SetActive(true);
        audioButton.SetActive(true);
        howToPlayButton.SetActive(true);
        player.SetActive(true);

        howToPlayImage.SetActive(false);
        returnButton.SetActive(false);
    }

}