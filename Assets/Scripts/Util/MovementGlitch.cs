using UnityEngine;

public class MovementGlitch : MonoBehaviour
{
    [SerializeField] private float glitchInterval = 0.01f;
    [SerializeField] private float glitchIntensity = 0.1f;

    private float timer;
    private float prevGlitchOffsetX = 0;
    private float prevGlitchOffsetY = 0;

    private void Start()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= glitchInterval)
        {
            ApplyGlitchEffect();
            timer = 0f;
        }
    }

    private void ApplyGlitchEffect()
    {
        RevertGlitchEffect();

        float randomOffsetX = Random.Range(-glitchIntensity, glitchIntensity);
        float randomOffsetY = Random.Range(-glitchIntensity, glitchIntensity);
        transform.position += new Vector3(randomOffsetX, randomOffsetY, 0);
        prevGlitchOffsetX = randomOffsetX;
        prevGlitchOffsetY = randomOffsetY;
    }

    private void RevertGlitchEffect()
    {
        transform.position -= new Vector3(prevGlitchOffsetX, prevGlitchOffsetY, 0);
    }
}