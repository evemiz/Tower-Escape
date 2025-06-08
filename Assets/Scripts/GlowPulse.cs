using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    public Light glowLight;
    public float minIntensity = 1f;
    public float maxIntensity = 5f;
    public float pulseSpeed = 3f;

    void Update()
    {
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        glowLight.intensity = intensity;
    }
}