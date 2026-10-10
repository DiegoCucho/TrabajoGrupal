using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Objetivo del nivel: dar "goal" basuras al pez antes de que se acabe el tiempo.
/// - Si llegas a la meta: pantalla de victoria.
/// - Si se acaba el tiempo (TimerScript) o te quedas sin vida (PlayerHealth): GameOver.
/// Se crea sola desde GameBootstrap.
/// </summary>
public class GoalManager : MonoBehaviour
{
    public static GoalManager Instance;
    public static int Goal = 10;      // lo lee TrashTextScript para mostrar "x/meta"

    [Tooltip("Basuras que hay que dar al pez para ganar")]
    public int goal = 10;

    public bool Won { get; private set; }

    private void Awake()
    {
        Instance = this;
        Goal = goal;
        Won = false;
        Time.timeScale = 1f;
        TrashTextScript.TrashItemCount = 0;
    }

    private void Update()
    {
        if (Won) return;
        if (PlayerHealth.Instance != null && PlayerHealth.Instance.IsDead) return;

        if (TrashTextScript.TrashItemCount >= Goal)
        {
            Win();
        }
    }

    private void Win()
    {
        Won = true;
        Time.timeScale = 0f; // congela el juego (tambien el timer, para que no salte GameOver)
        BuildVictoryUI();
    }

    // ---------------- UI de victoria ----------------

    private void BuildVictoryUI()
    {
        GameObject canvasGO = new GameObject("VictoryCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;
        canvasGO.AddComponent<GraphicRaycaster>();

        // fondo oscuro
        Image dim = MakeImage("Dim", canvasGO.transform, new Color(0f, 0.08f, 0.15f, 0.8f));
        Stretch(dim.rectTransform);

        MakeText("Titulo", canvasGO.transform, "¡WINNER!", 120, new Color(1f, 0.92f, 0.3f),
                 new Vector2(0f, 190f), new Vector2(1400f, 160f));
        MakeText("Subtitulo", canvasGO.transform,
                 $"¡Ganaste! Salvaste el mar con {TrashTextScript.TrashItemCount} basuras recogidas.",
                 44, Color.white, new Vector2(0f, 70f), new Vector2(1500f, 80f));

        MakeButton("BtnJugar", canvasGO.transform, "Jugar de nuevo", new Vector2(-230f, -90f),
                   () => { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
        MakeButton("BtnMenu", canvasGO.transform, "Menu", new Vector2(230f, -90f),
                   () => { Time.timeScale = 1f; SceneManager.LoadScene("Menu"); });
    }

    private static Image MakeImage(string name, Transform parent, Color color)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        Image img = go.AddComponent<Image>();
        img.sprite = RuntimeSprites.Square;
        img.color = color;
        return img;
    }

    private static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private static Text MakeText(string name, Transform parent, string content, int size, Color color,
                                 Vector2 pos, Vector2 boxSize)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        Text t = go.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.text = content;
        t.fontSize = size;
        t.fontStyle = FontStyle.Bold;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = color;
        t.horizontalOverflow = HorizontalWrapMode.Wrap;
        RectTransform rt = t.rectTransform;
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta = boxSize;
        return t;
    }

    private static void MakeButton(string name, Transform parent, string label, Vector2 pos,
                                   UnityEngine.Events.UnityAction onClick)
    {
        Image img = MakeImage(name, parent, new Color(0.1f, 0.55f, 0.85f, 1f));
        RectTransform rt = img.rectTransform;
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(400f, 110f);

        Button btn = img.gameObject.AddComponent<Button>();
        btn.targetGraphic = img;
        btn.onClick.AddListener(onClick);

        Text t = MakeText("Label", img.transform, label, 46, Color.white, Vector2.zero, Vector2.zero);
        Stretch(t.rectTransform);
    }
}
