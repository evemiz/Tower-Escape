using UnityEngine;

public class CollectibleSword : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    public AudioClip collectSound;
    public float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCollector collector = other.GetComponent<PlayerCollector>();
            if (collector != null)
            {
                collector.CollectSword();
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);
            }

            if (explosionParticle != null)
            {
                ParticleSystem particle = Instantiate(explosionParticle, transform.position, Quaternion.identity);
                particle.Play();
                Destroy(particle.gameObject, particle.main.duration + particle.main.startLifetime.constant);
            }

            Destroy(gameObject);
        }
    }
}
