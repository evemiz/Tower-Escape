using UnityEngine;

public class TowerTransparencyController : MonoBehaviour
{
    public Renderer[] towerRenderers; // שדה חדש - מערך של רנדררים
    public Material transparentMaterial;
    public Material originalMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Renderer rend in towerRenderers)
            {
                rend.material = transparentMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Renderer rend in towerRenderers)
            {
                rend.material = originalMaterial;
            }
        }
    }
}
