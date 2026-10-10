using System.Collections;
using UnityEngine;

/// <summary>
/// Genera enemigos fuera de la pantalla y van apareciendo cada vez mas rapido.
/// Se crea sola desde GameBootstrap, pero tambien se puede poner a mano en la escena.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    public static int EnemiesKilled = 0;

    [Header("Enemigos")]
    public Sprite enemySprite;          // opcional: dejar vacio para usar el enemigo generado
    public float enemyScale = 1.1f;
    public int maxEnemies = 8;

    [Header("Aparicion")]
    public float firstDelay = 3f;
    public float startInterval = 3f;     // segundos entre enemigos al inicio
    public float minInterval = 1.2f;     // limite de rapidez
    public float intervalDecrease = 0.05f; // cuanto baja el intervalo por cada enemigo
    public float spawnMargin = 1.5f;     // que tan lejos del borde aparecen

    [Header("Dificultad")]
    public float speedIncreasePerKill = 0.03f;
    public float maxSpeed = 3.6f;

    private float currentInterval;

    private void Start()
    {
        EnemiesKilled = 0;
        currentInterval = startInterval;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(firstDelay);

        while (true)
        {
            bool playerDead = PlayerHealth.Instance != null && PlayerHealth.Instance.IsDead;
            if (!playerDead && CountEnemies() < maxEnemies)
            {
                SpawnEnemy();
                currentInterval = Mathf.Max(minInterval, currentInterval - intervalDecrease);
            }
            yield return new WaitForSeconds(currentInterval);
        }
    }

    private int CountEnemies()
    {
        return FindObjectsOfType<EnemyChaser>().Length;
    }

    private void SpawnEnemy()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector2 c = cam.transform.position;

        // escoger un borde al azar (0=izq, 1=der, 2=arriba, 3=abajo)
        Vector2 pos;
        switch (Random.Range(0, 4))
        {
            case 0: pos = new Vector2(c.x - halfW - spawnMargin, c.y + Random.Range(-halfH, halfH)); break;
            case 1: pos = new Vector2(c.x + halfW + spawnMargin, c.y + Random.Range(-halfH, halfH)); break;
            case 2: pos = new Vector2(c.x + Random.Range(-halfW, halfW), c.y + halfH + spawnMargin); break;
            default: pos = new Vector2(c.x + Random.Range(-halfW, halfW), c.y - halfH - spawnMargin); break;
        }

        GameObject enemy = new GameObject("Enemy");
        enemy.transform.position = new Vector3(pos.x, pos.y, 0f);
        enemy.transform.localScale = Vector3.one * enemyScale;

        SpriteRenderer sr = enemy.AddComponent<SpriteRenderer>();
        sr.sprite = enemySprite != null ? enemySprite : RuntimeSprites.Enemy;
        sr.sortingOrder = 5;

        // Rigidbody cinematico + collider trigger para detectar al buzo
        Rigidbody2D rb = enemy.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        CircleCollider2D col = enemy.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.4f;

        EnemyChaser chaser = enemy.AddComponent<EnemyChaser>();
        chaser.speed = Mathf.Min(maxSpeed, chaser.speed + EnemiesKilled * speedIncreasePerKill);
    }
}
