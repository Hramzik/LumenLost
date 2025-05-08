using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float voidYLevel = -5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isGrounded;
    private float groundCheckDistance = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ResolveVoidCollision();

        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if      (moveInput < 0) sr.flipX = true;
        else if (moveInput > 0) sr.flipX = false;

        TryJumpIfPressed();
    }

    private void TryJumpIfPressed()
    {
        Vector2 boxSize = new Vector2(sr.bounds.size.x, 0.1f);
        Vector2 boxCenter = new Vector2(transform.position.x, transform.position.y - sr.bounds.extents.y - 0.1f);

        isGrounded = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.down, groundCheckDistance, ~0).collider != null;
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded) rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeathWall")) LevelManager.Instance.ReloadCurrentLevel();
    }

    private void ResolveVoidCollision()
    {
        if (transform.position.y < voidYLevel) LevelManager.Instance.ReloadCurrentLevel();
    }
}