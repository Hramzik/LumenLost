using UnityEngine;

public class WallLimiterCube : CubeController
{
    private MovementGlitch movementGlitch;
    private MovementLimiter wallLimiter;

    [SerializeField] private float wallLimiterOffset;

    private void Start()
    {
        movementGlitch = GetComponent<MovementGlitch>();
        wallLimiter = GetComponent<MovementLimiter>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        movementGlitch.enabled = false;
        float maxX = transform.position.x + wallLimiterOffset;
        wallLimiter.SetLimit(maxX);
    }
}