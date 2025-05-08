using UnityEngine;
using System.Collections;

public class ExplosionCube : CubeController
{
    [SerializeField] private float explosionRadius;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) StartCoroutine(ExplodeAndReloadRoutine());
    }

    private IEnumerator ExplodeAndReloadRoutine()
    {
        Explode();
        yield return new WaitForSeconds(0f);
        //LevelManager.Instance.ReloadCurrentLevel();
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position, 
            explosionRadius, 
            ~0
        );

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out CubeController cube) && cube != this) 
            {
                var renderer = cube.GetComponent<Renderer>();
                if (renderer != null) renderer.enabled = false;
                
                var collider = cube.GetComponent<Collider2D>();
                if (collider != null) collider.enabled = false;
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}