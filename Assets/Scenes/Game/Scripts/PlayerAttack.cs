using System.Collections;
using UnityEngine;

/// <summary>
/// Ataque del buzo: ESPACIO lanza un pulso de burbujas que golpea a los enemigos cercanos.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    public KeyCode attackKey = KeyCode.Space;
    public float radius = 2.2f;
    public float cooldown = 0.6f;
    public int damage = 1;

    private float nextAttack;

    private void Update()
    {
        if (PlayerHealth.Instance != null && PlayerHealth.Instance.IsDead) return;

        if (Input.GetKeyDown(attackKey) && Time.time >= nextAttack)
        {
            nextAttack = Time.time + cooldown;
            Attack();
        }
    }

    private void Attack()
    {
        Vector2 center = transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);
        foreach (Collider2D h in hits)
        {
            EnemyChaser enemy = h.GetComponent<EnemyChaser>();
            if (enemy != null) enemy.Hit(damage, center);
        }
        StartCoroutine(ShowPulse());
    }

    private IEnumerator ShowPulse()
    {
        GameObject ring = new GameObject("AttackPulse");
        ring.transform.position = transform.position;
        SpriteRenderer sr = ring.AddComponent<SpriteRenderer>();
        sr.sprite = RuntimeSprites.Ring;
        sr.sortingOrder = 20;

        float t = 0f;
        const float duration = 0.25f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = t / duration;
            ring.transform.position = transform.position;
            ring.transform.localScale = Vector3.one * (radius * 2f * k);
            sr.color = new Color(0.7f, 0.95f, 1f, 1f - k);
            yield return null;
        }
        Destroy(ring);
    }
}
