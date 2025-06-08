using UnityEngine;

public class EnemyPatrolSmoothTurn : MonoBehaviour
{
    public float speed = 2f;              // מהירות ההליכה
    public float walkTime = 2f;           // זמן הליכה לפני סיבוב
    public float turnDuration = 1f;       // כמה זמן לוקח הסיבוב

    private float walkTimer = 0f;
    private bool isTurning = false;

    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float turnTimer = 0f;

    void Update()
    {
        if (!isTurning)
        {
            // תנועה קדימה
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            walkTimer += Time.deltaTime;

            if (walkTimer >= walkTime)
            {
                // התחלת סיבוב
                isTurning = true;
                walkTimer = 0f;
                turnTimer = 0f;

                startRotation = transform.rotation;
                targetRotation = startRotation * Quaternion.Euler(0f, 180f, 0f);
            }
        }
        else
        {
            // סיבוב הדרגתי
            turnTimer += Time.deltaTime;
            float t = Mathf.Clamp01(turnTimer / turnDuration);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            if (t >= 1f)
            {
                isTurning = false;
            }
        }
    }
}
