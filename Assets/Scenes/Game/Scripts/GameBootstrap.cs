using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Se ejecuta solo al cargar la escena "Game": le agrega al buzo la vida y el ataque,
/// y crea el generador de enemigos. No hace falta tocar la escena en el Inspector.
/// </summary>
public static class GameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        if (SceneManager.GetActiveScene().name != "Game") return;

        GameObject player = GameObject.FindGameObjectWithTag("Mermaid");
        if (player == null)
        {
            Debug.LogWarning("GameBootstrap: no se encontro el objeto con tag 'Mermaid'.");
            return;
        }

        if (player.GetComponent<PlayerHealth>() == null) player.AddComponent<PlayerHealth>();
        if (player.GetComponent<PlayerAttack>() == null) player.AddComponent<PlayerAttack>();

        if (Object.FindObjectOfType<EnemySpawner>() == null)
        {
            new GameObject("EnemySpawner").AddComponent<EnemySpawner>();
        }
    }
}
