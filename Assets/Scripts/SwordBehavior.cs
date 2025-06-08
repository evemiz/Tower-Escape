using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Sword hit an enemy!");
        }

        Debug.Log("Sword hit: ", collision.gameObject);

        Destroy(gameObject);
    }
}
