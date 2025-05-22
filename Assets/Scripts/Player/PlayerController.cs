using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce;
    [SerializeField] private float voidYLevel;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool wantToJump = false;
    private float wantToJumpCheckDistance = 0.2f;

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

        TryJump();
    }

    private void TryJump()
    {
        if (wantToJump) return;
        if (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.W)) return;
        if (rb.linearVelocity.y > 0) return;

        Vector2 boxSize = new Vector2(sr.bounds.size.x, 0.01f);
        Vector2 boxCenter = new Vector2(transform.position.x, transform.position.y - sr.bounds.extents.y - 0.1f);

        wantToJump = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.down, wantToJumpCheckDistance, ~0).collider != null;
    }

    private void Jump()
    {
        if (!wantToJump) return;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        wantToJump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeathWall")) LevelManager.Instance.ReloadCurrentLevel();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent("CubeController")) Jump();
    }

    private void ResolveVoidCollision()
    {
        if (transform.position.y < voidYLevel) LevelManager.Instance.ReloadCurrentLevel();
    }
}