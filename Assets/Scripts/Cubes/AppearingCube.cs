using UnityEngine;
using System.Collections;

public class AppearingCube : CubeController
{
    [Header("Appearing Settings")]
    public int triggerKey = 1;
    public float fadeDuration = 1f;
    public bool startVisible = true;

    private SpriteRenderer spriteRenderer;
    new private Collider2D collider;
    private bool isActive;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        CubeManager.Instance.RegisterCube(this);
        SetInitialState();
    }

    private void SetInitialState()
    {
        isActive = startVisible;
        UpdateState();
    }

    public void Toggle()
    {
        isActive = !isActive;
        UpdateState();
    }

    public void Activate()
    {
        isActive = true;
        UpdateState();
    }

    public void Deactivate()
    {
        isActive = false;
        UpdateState();
    }

    private void UpdateState()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateTransition());
    }

    private IEnumerator AnimateTransition()
    {
        float targetAlpha = isActive ? 1f : 0f;
        Color currentColor = spriteRenderer.color;

        collider.enabled = isActive;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(currentColor.a, targetAlpha, timer / fadeDuration);
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        CubeManager.Instance.UnregisterCube(this);
    }
}