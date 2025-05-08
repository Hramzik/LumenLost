using UnityEngine;

public class WallOfDeath : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float resetDistance;

    private Vector3 startPosition;
    private float timer;

    private void Start()
    {
        timer = 0f;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (Vector3.Distance(startPosition, transform.position) > resetDistance) 
            transform.position = startPosition;

        timer += Time.deltaTime;
        if (timer >= 0.01f)
        {
            ChangeSpriteOffset();
            timer = 0f;
        }
    }

    private void ChangeSpriteOffset()
    {
        float randomOffsetX = Random.Range(-0.1f, 0.1f);
        float randomOffsetY = Random.Range(-0.1f, 0.1f);
        
        transform.Translate (Vector3.right * randomOffsetX);
        transform.Translate (Vector3.up    * randomOffsetY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var renderer = collision.gameObject.GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = false;
        
        var collider = collision.gameObject.GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
    }
}