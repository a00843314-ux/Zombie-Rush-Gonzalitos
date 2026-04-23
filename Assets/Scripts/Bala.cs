using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidad    = 15f;
    public float tiempoDeVida = 3f;
    public int   danio        = 10;

    [Header("Impacto")]
    public GameObject prefabImpacto; // arrastra tu prefab aquí en el Inspector

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.right * velocidad;
        Destroy(gameObject, tiempoDeVida);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ── Intentar aplicar daño ──────────────────────────────
        Vida vida = collision.gameObject.GetComponent<Vida>();
        if (vida != null)
            vida.RecibirDanio(danio);

        // ── Spawnear efecto de impacto ─────────────────────────
        if (prefabImpacto != null)
        {
            // Punto y normal exactos del primer contacto
            ContactPoint2D contacto = collision.GetContact(0);
            Instantiate(prefabImpacto, contacto.point, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
