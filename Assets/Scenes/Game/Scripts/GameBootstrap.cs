using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Cada vez que se carga la escena "Game" (ya sea abriendola directo o entrando desde el Menu)
/// le agrega al buzo la vida y el ataque, y crea el generador de enemigos.
/// No hace falta tocar la escena en el Inspector.
/// </summary>
public static class GameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Register()
    {
        // se registra una sola vez al iniciar el juego y escucha TODAS las cargas de escena
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Game") return;

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

        Debug.Log("GameBootstrap: vida, ataque y enemigos activados.");
    }
}
