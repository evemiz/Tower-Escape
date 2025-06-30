using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public Sprite soundOnIcon; 
    public Sprite soundOffIcon; 
    public Image buttonImage;   

    private bool isMuted = false;

    void Start()
    {
        isMuted = AudioListener.volume == 0f;
        UpdateButtonIcon();
    }

    public void ToggleSound()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? soundOffIcon : soundOnIcon;
        }
    }
}
