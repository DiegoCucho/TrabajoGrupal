using UnityEngine;

/// <summary>
/// Genera sprites simples por codigo (cuadrado, circulo, anillo y enemigo)
/// para no depender de imagenes nuevas. Si luego tienen arte propio,
/// pueden asignarlo en EnemySpawner (campo "enemySprite").
/// </summary>
public static class RuntimeSprites
{
    private static Sprite _square;
    private static Sprite _ring;
    private static Sprite _enemy;

    public static Sprite Square
    {
        get
        {
            if (_square == null)
            {
                Texture2D tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);
                Color[] px = new Color[16];
                for (int i = 0; i < px.Length; i++) px[i] = Color.white;
                tex.SetPixels(px);
                tex.Apply();
                _square = Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 4f);
            }
            return _square;
        }
    }

    // Anillo blanco (para el efecto visual del ataque)
    public static Sprite Ring
    {
        get
        {
            if (_ring == null)
            {
                const int size = 128;
                Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
                tex.filterMode = FilterMode.Bilinear;
                float c = (size - 1) / 2f;
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        float d = Vector2.Distance(new Vector2(x, y), new Vector2(c, c));
                        bool inside = d <= c && d >= c - 6f;
                        tex.SetPixel(x, y, inside ? Color.white : Color.clear);
                    }
                }
                tex.Apply();
                _ring = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
            }
            return _ring;
        }
    }

    // Enemigo: bolita roja con ojos enojados. Mira hacia la derecha.
    public static Sprite Enemy
    {
        get
        {
            if (_enemy == null)
            {
                const int size = 64;
                Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
                tex.filterMode = FilterMode.Bilinear;
                float c = (size - 1) / 2f;
                Color body = new Color(0.86f, 0.16f, 0.2f, 1f);
                Color dark = new Color(0.45f, 0.05f, 0.1f, 1f);
                Vector2 eyeA = new Vector2(38, 38);
                Vector2 eyeB = new Vector2(46, 28);

                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        Vector2 p = new Vector2(x, y);
                        float d = Vector2.Distance(p, new Vector2(c, c));
                        Color col = Color.clear;
                        if (d <= c - 1f)
                        {
                            col = d > c - 5f ? dark : body;
                            if (Vector2.Distance(p, eyeA) <= 7f) col = Color.white;
                            if (Vector2.Distance(p, eyeB) <= 7f) col = Color.white;
                            if (Vector2.Distance(p, eyeA + new Vector2(2, 0)) <= 3f) col = Color.black;
                            if (Vector2.Distance(p, eyeB + new Vector2(2, 0)) <= 3f) col = Color.black;
                        }
                        tex.SetPixel(x, y, col);
                    }
                }
                tex.Apply();
                _enemy = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
            }
            return _enemy;
        }
    }
}
