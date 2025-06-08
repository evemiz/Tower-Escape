using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    public GameObject swordPrefab;     // Prefab של החרב (שגררי מה-Assets)
    public Transform throwPoint;       // נקודת הזריקה (למשל מול היד של השחקן)
    public float throwForce = 15f;     // עוצמת הזריקה

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowSword();
        }
    }

    void ThrowSword()
    {
        GameObject sword = Instantiate(swordPrefab, throwPoint.position, Quaternion.identity);
        Quaternion localRotation = swordPrefab.transform.localRotation;
        sword.transform.rotation = throwPoint.rotation * localRotation;
        Rigidbody rb = sword.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = throwPoint.forward * throwForce;
        }
    }
}
