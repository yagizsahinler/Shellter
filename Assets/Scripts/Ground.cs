using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField]
    private Texture2D baseTexture;

    private Texture2D cloneTexture;
    private SpriteRenderer spriteRenderer;

    private float _widthWorld, _heightWorld;
    private float _widthPixel, _heightPixel;

    // Baþlangýçta çalýþtýrýlýr
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Texture kopyasý oluþtur
        cloneTexture = Instantiate(baseTexture);

        // Transparanlýk iþlemek için manuel kontrol
        ProcessAlphaChannel(cloneTexture);

        UpdateTexture();

        // Polygon Collider ekle
        gameObject.AddComponent<PolygonCollider2D>();
    }

    // Dünya geniþliði
    public float WidthWorld
    {
        get
        {
            if (_widthWorld == 0)
                _widthWorld = spriteRenderer.bounds.size.x;

            return _widthWorld;
        }
    }

    // Dünya yüksekliði
    public float HeightWorld
    {
        get
        {
            if (_heightWorld == 0)
                _heightWorld = spriteRenderer.bounds.size.y;

            return _heightWorld;
        }
    }

    // Piksel geniþliði
    public float WidthPixel
    {
        get
        {
            if (_widthPixel == 0)
                _widthPixel = spriteRenderer.sprite.texture.width;

            return _widthPixel;
        }
    }

    // Piksel yüksekliði
    public float HeightPixel
    {
        get
        {
            if (_heightPixel == 0)
                _heightPixel = spriteRenderer.sprite.texture.height;

            return _heightPixel;
        }
    }

    // Texture güncelleme
    void UpdateTexture()
    {
        cloneTexture.Apply();
        spriteRenderer.sprite = Sprite.Create(
            cloneTexture,
            new Rect(0, 0, cloneTexture.width, cloneTexture.height),
            new Vector2(0.5f, 0.5f), 50f);
    }

    // Alfa kanalý transparanlýk iþleme
    void ProcessAlphaChannel(Texture2D texture)
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color pixel = texture.GetPixel(x, y);

                // Alfa deðeri düþükse pikselleri þeffaf yap
                if (pixel.a < 0.1f)
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }
        texture.Apply();
    }

    // Dünya koordinatlarýný piksel koordinatlarýna dönüþtür
    Vector2Int World2Pixel(Vector2 pos)
    {
        Vector2Int v = Vector2Int.zero;

        float dx = (pos.x - transform.position.x);
        float dy = (pos.y - transform.position.y);

        v.x = Mathf.RoundToInt(0.5f * WidthPixel + dx * (WidthPixel / WidthWorld));
        v.y = Mathf.RoundToInt(0.5f * HeightPixel + dy * (HeightPixel / HeightWorld));

        return v;
    }

    // Delik açma iþlemi
    void MakeAHole(CircleCollider2D col)
    {
        Vector2Int c = World2Pixel(col.bounds.center);
        int r = Mathf.RoundToInt(col.bounds.size.x * WidthPixel / WidthWorld);

        int px, nx, py, ny, d;

        for (int i = 0; i <= r; i++)
        {
            d = Mathf.RoundToInt(Mathf.Sqrt(r * r - i * i));

            for (int j = 0; j <= d; j++)
            {
                px = c.x + i;
                nx = c.x - i;
                py = c.y + j;
                ny = c.y - j;

                if (IsValidPixel(px, py)) cloneTexture.SetPixel(px, py, Color.clear);
                if (IsValidPixel(nx, py)) cloneTexture.SetPixel(nx, py, Color.clear);
                if (IsValidPixel(px, ny)) cloneTexture.SetPixel(px, ny, Color.clear);
                if (IsValidPixel(nx, ny)) cloneTexture.SetPixel(nx, ny, Color.clear);
            }
        }

        UpdateTexture();
        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    // Geçerli piksel kontrolü
    bool IsValidPixel(int x, int y)
    {
        return x >= 0 && x < cloneTexture.width && y >= 0 && y < cloneTexture.height;
    }

    // Çarpýþma algýlama
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")) return;

        CircleCollider2D col = collision.GetComponent<CircleCollider2D>();
        if (col == null) return;

        MakeAHole(col);
        Destroy(collision.gameObject, 0.1f);
    }
}
