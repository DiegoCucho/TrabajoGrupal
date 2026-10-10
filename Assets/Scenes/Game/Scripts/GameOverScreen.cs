using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Pantalla de Game Over: quita la sirena y el texto viejo de la escena y deja solo
/// "GAME OVER" (y el boton de la casa que ya existe para volver al menu).
/// La llama GameBootstrap cuando carga la escena "GameOver".
/// </summary>
public static class GameOverScreen
{
    public static void Setup()
    {
        // Fuera la sirena (Logo) y el texto viejo
        Hide("Logo");
        Hide("Text (Legacy)");

        GameObject canvasGO = new GameObject("GameOverTextCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 5; // encima del fondo; el boton de la casa sigue clicable (el texto no bloquea clics)
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        string reason = PlayerHealth.DiedLastRun
            ? "¡Te quedaste sin vida!"
            : "¡Se acabó el tiempo!";

        MakeText("Titulo", canvasGO.transform, "GAME OVER", 170, new Color(0.95f, 0.2f, 0.2f),
                 new Vector2(0f, 120f), new Vector2(1600f, 240f));
        MakeText("Motivo", canvasGO.transform, reason, 60, Color.white,
                 new Vector2(0f, -30f), new Vector2(1600f, 90f));
        MakeText("Progreso", canvasGO.transform,
                 $"Basura eliminada: {TrashTextScript.TrashItemCount}/{GoalManager.Goal}", 44,
                 new Color(1f, 1f, 1f, 0.9f), new Vector2(0f, -120f), new Vector2(1600f, 80f));
    }

    private static void Hide(string objectName)
    {
        GameObject go = GameObject.Find(objectName);
        if (go != null) go.SetActive(false);
    }

    private static void MakeText(string name, Transform parent, string content, int size, Color color,
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
        t.raycastTarget = false;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;

        Outline outline = go.AddComponent<Outline>();
        outline.effectColor = new Color(0f, 0.1f, 0.2f, 0.9f);
        outline.effectDistance = new Vector2(4f, -4f);

        RectTransform rt = t.rectTransform;
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta = boxSize;
    }
}
