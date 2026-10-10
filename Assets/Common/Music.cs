using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Boton de musica (Menu y Game). El estado (con musica / silenciado) es ESTATICO,
/// asi que se mantiene al cambiar de escena: si silencias en el Menu, el juego empieza
/// silenciado, y si lo vuelves a activar, vuelve a sonar.
/// Se silencia TODO el audio con AudioListener.volume (la musica sigue corriendo en
/// silencio y vuelve a sonar donde iba al reactivarla).
/// </summary>
public class Music : MonoBehaviour
{
    public Sprite onMusic;
    public Sprite offMusic;

    public Button MusicButton;
    public bool isOn;
    public AudioSource ad;   // se deja por compatibilidad con la escena (ya no hace falta)

    // Estado compartido entre escenas. Por defecto, con musica.
    private static bool musicOn = true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetState()
    {
        musicOn = true;
        AudioListener.volume = 1f;
    }

    private void Awake()
    {
        // antes de que arranque cualquier sonido de la escena
        AudioListener.volume = musicOn ? 1f : 0f;
    }

    private void Start()
    {
        isOn = musicOn;
        Apply();
    }

    public void OffSound()
    {
        musicOn = !musicOn;
        isOn = musicOn;
        Apply();
    }

    private void Apply()
    {
        AudioListener.volume = isOn ? 1f : 0f;

        if (MusicButton != null)
        {
            Image img = MusicButton.GetComponent<Image>();
            if (img != null) img.sprite = isOn ? onMusic : offMusic;
        }
    }
}
