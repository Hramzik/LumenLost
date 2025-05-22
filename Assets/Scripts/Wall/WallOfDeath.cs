using UnityEngine;

public class WallOfDeath : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var renderer = collision.gameObject.GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = false;
        
        var collider = collision.gameObject.GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
    }
}