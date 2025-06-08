using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            // הופך את האובייקט לפונה לכיוון המצלמה כל הזמן
            transform.forward = Camera.main.transform.forward;
        }
    }
}
