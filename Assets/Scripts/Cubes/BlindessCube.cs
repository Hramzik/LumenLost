using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class BlindnessCube : CubeController
{
    [Header("Blindness Settings")]
    private float maxEffectDuration = 0f;
    private float fadeInTime = 1f;
    private float fadeOutTime = 10f;
    private float maxVignetteIntensity = 1f;

    static private Volume postProcessVolume;
    static private Vignette vignette;
    static public bool canStartCoroutine = true;
    static public Coroutine     effectCoroutine = null;
    static public BlindnessCube coroutineStarter = null; // Глупое юнити обязывает завершать корутину с ее начинателя
    static private float remainingEffectTime;

    private void Awake()
    {
        postProcessVolume = FindObjectsByType<Volume>(FindObjectsSortMode.None)[0];
        postProcessVolume.profile.TryGet(out vignette);
        vignette.intensity.value = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        remainingEffectTime = maxEffectDuration;
        StartEffect();
    }

    private void StartEffect()
    {
        if (!canStartCoroutine) return;
        if (effectCoroutine != null && coroutineStarter != null) coroutineStarter.StopEffect();
        effectCoroutine = StartCoroutine(BlindnessEffect());
        coroutineStarter = this;
    }

    private void StopEffect()
    {
        StopCoroutine(effectCoroutine);
    }

    private IEnumerator BlindnessEffect()
    {
        canStartCoroutine = false;

        float fadeTimer = 0f;
        float fadeStart = vignette.intensity.value;
        while (fadeTimer < fadeInTime && vignette.intensity.value < maxVignetteIntensity)
        {
            fadeTimer += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(fadeStart, maxVignetteIntensity, fadeTimer / fadeInTime);
            yield return null;
        }

        vignette.intensity.value = maxVignetteIntensity;

        while (remainingEffectTime > 0)
        {
            remainingEffectTime -= Time.deltaTime;
            yield return null;
        }

        canStartCoroutine = true;

        fadeTimer = 0f;
        while (fadeTimer < fadeOutTime)
        {
            fadeTimer += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(maxVignetteIntensity, 0f, fadeTimer / fadeOutTime);
            yield return null;
        }

        vignette.intensity.value = 0f;
    }
}