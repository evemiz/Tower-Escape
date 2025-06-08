using UnityEngine;

public class PlayerVFXShooter : MonoBehaviour
{
    public GameObject laserVFXPrefab; // גררי לכאן את ה־Prefab של ה־VFX
    public Transform firePoint;       // הנקודה ממנה נורה הלייזר
    public float laserLifetime = 2f;

 void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootLaser();
        }
    }

    void ShootLaser()
    {
        GameObject laser = Instantiate(laserVFXPrefab, firePoint.position, firePoint.rotation);

        // נגן את ה-Particle System
        ParticleSystem ps = laser.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }

        Destroy(laser, laserLifetime); // מחיקה אחרי זמן
    }
}
