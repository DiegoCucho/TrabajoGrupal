using UnityEngine;

/// <summary>
/// Enemigo que persigue al buzo y le quita vida al tocarlo.
/// Se crea desde EnemySpawner.
/// </summary>
public class EnemyChaser : MonoBehaviour
{
    public float speed = 1.8f;
    public float damage = 10f;
    public int health = 2;
    public float knockbackTime = 0.35f;

    private float stunTimer;
    private Vector2 pushDir;
    private SpriteRenderer sr;
    private float hitFlash;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (MermaidMovement.Instance == null) return;
        if (PlayerHealth.Instance != null && PlayerHealth.Instance.IsDead) return;

        Vector2 pos = transform.position;
        Vector2 target = MermaidMovement.Instance.transform.position;

        if (stunTimer > 0f)
        {
            // rebote despues de golpear / ser golpeado
            stunTimer -= Time.deltaTime;
            transform.position = pos + pushDir * (speed * 1.8f) * Time.deltaTime;
        }
        else
        {
            transform.position = Vector2.MoveTowards(pos, target, speed * Time.deltaTime);
        }

        // Mirar hacia el buzo (el sprite mira a la derecha)
        if (sr != null)
        {
            sr.flipX = target.x < pos.x;
            if (hitFlash > 0f)
            {
                hitFlash -= Time.deltaTime;
                if (hitFlash <= 0f) sr.color = Color.white;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Mermaid")) return;
        if (stunTimer > 0f) return;

        PlayerHealth ph = PlayerHealth.Instance;
        if (ph == null) return;

        if (ph.TakeDamage(damage))
        {
            // rebota lejos del buzo
            pushDir = ((Vector2)transform.position - (Vector2)other.transform.position).normalized;
            stunTimer = knockbackTime;
        }
    }

    /// <summary>Lo llama PlayerAttack.</summary>
    public void Hit(int amount, Vector2 from)
    {
        health -= amount;
        pushDir = ((Vector2)transform.position - from).normalized;
        stunTimer = knockbackTime;

        if (sr != null)
        {
            sr.color = new Color(1f, 0.7f, 0.7f, 1f);
            hitFlash = 0.1f;
        }

        if (health <= 0)
        {
            EnemySpawner.EnemiesKilled++;
            Destroy(gameObject);
        }
    }
}
