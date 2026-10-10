using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Vida del buzo + barra de vida (se crea sola por codigo, abajo al centro).
/// Se agrega automaticamente al objeto "Mermaid" desde GameBootstrap.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("Vida")]
    public float maxHealth = 100f;
    public float invincibleTime = 1f;   // segundos sin recibir dano despues de un golpe

    [Header("Barra de vida (UI)")]
    public Vector2 barSize = new Vector2(360f, 34f);
    public Vector2 barMargin = new Vector2(30f, 30f);

    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    private float invTimer;
    private SpriteRenderer[] renderers;
    private Color[] originalColors;

    private Image fillImage;
    private Text label;

    private void Awake()
    {
        Instance = this;
        CurrentHealth = maxHealth;

        renderers = GetComponentsInChildren<SpriteRenderer>(true);
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++) originalColors[i] = renderers[i].color;

        BuildUI();
        RefreshUI();
    }

    private void Update()
    {
        if (invTimer > 0f)
        {
            invTimer -= Time.deltaTime;
            // parpadeo rojo mientras es invencible
            bool red = Mathf.FloorToInt(Time.time * 10f) % 2 == 0;
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] == null) continue;
                renderers[i].color = red ? new Color(1f, 0.35f, 0.35f, originalColors[i].a) : originalColors[i];
            }
            if (invTimer <= 0f) RestoreColors();
        }
    }

    private void RestoreColors()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null) renderers[i].color = originalColors[i];
        }
    }

    /// <summary>Devuelve true si el golpe hizo dano.</summary>
    public bool TakeDamage(float amount)
    {
        if (IsDead || invTimer > 0f) return false;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        invTimer = invincibleTime;
        RefreshUI();

        if (CurrentHealth <= 0f)
        {
            StartCoroutine(Die());
        }
        return true;
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        RefreshUI();
    }

    private IEnumerator Die()
    {
        IsDead = true;
        RestoreColors();

        if (MermaidMovement.Instance != null) MermaidMovement.Instance.enabled = false;
        var body = GetComponent<Rigidbody2D>();
        if (body != null) body.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("GameOver");
    }

    // ---------------- UI ----------------

    private void BuildUI()
    {
        GameObject canvasGO = new GameObject("HealthBarCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 50;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        // Fondo (borde oscuro)
        GameObject bg = new GameObject("HealthBarBG");
        bg.transform.SetParent(canvasGO.transform, false);
        Image bgImg = bg.AddComponent<Image>();
        bgImg.sprite = RuntimeSprites.Square;
        bgImg.color = new Color(0f, 0f, 0f, 0.65f);
        RectTransform bgRt = bg.GetComponent<RectTransform>();
        // abajo al centro (arriba ya estan el timer, la basura y los botones)
        bgRt.anchorMin = bgRt.anchorMax = bgRt.pivot = new Vector2(0.5f, 0f);
        bgRt.anchoredPosition = new Vector2(0f, barMargin.y);
        bgRt.sizeDelta = barSize;

        // Relleno (se vacia de derecha a izquierda)
        GameObject fill = new GameObject("HealthBarFill");
        fill.transform.SetParent(bg.transform, false);
        fillImage = fill.AddComponent<Image>();
        fillImage.sprite = RuntimeSprites.Square;
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        RectTransform fillRt = fill.GetComponent<RectTransform>();
        fillRt.anchorMin = Vector2.zero;
        fillRt.anchorMax = Vector2.one;
        fillRt.offsetMin = new Vector2(4f, 4f);
        fillRt.offsetMax = new Vector2(-4f, -4f);

        // Texto "Vida 100/100"
        GameObject txt = new GameObject("HealthBarText");
        txt.transform.SetParent(bg.transform, false);
        label = txt.AddComponent<Text>();
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.fontSize = 22;
        label.fontStyle = FontStyle.Bold;
        label.alignment = TextAnchor.MiddleCenter;
        label.color = Color.white;
        RectTransform txtRt = txt.GetComponent<RectTransform>();
        txtRt.anchorMin = Vector2.zero;
        txtRt.anchorMax = Vector2.one;
        txtRt.offsetMin = Vector2.zero;
        txtRt.offsetMax = Vector2.zero;
    }

    private void RefreshUI()
    {
        if (fillImage == null) return;

        float pct = CurrentHealth / maxHealth;
        fillImage.fillAmount = pct;
        // verde -> amarillo -> rojo
        fillImage.color = pct > 0.5f
            ? Color.Lerp(Color.yellow, new Color(0.2f, 0.85f, 0.3f), (pct - 0.5f) * 2f)
            : Color.Lerp(new Color(0.9f, 0.15f, 0.15f), Color.yellow, pct * 2f);

        if (label != null) label.text = $"Vida {Mathf.CeilToInt(CurrentHealth)}/{Mathf.CeilToInt(maxHealth)}";
    }
}
