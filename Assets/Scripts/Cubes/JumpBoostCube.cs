using UnityEngine;

public class JumpBoostCube : CubeController
{
    [SerializeField] private float jumpMultiplier = 1.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.jumpForce *= jumpMultiplier;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.jumpForce /= jumpMultiplier;
        }
    }
}