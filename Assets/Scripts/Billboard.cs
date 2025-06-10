using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // מוצא את המצלמה הראשית
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.forward);
        }
    }
}
