using UnityEngine;

public class FondoDinamico : MonoBehaviour
{
	[Header("Parallax")]
	public float velocidadParallax = 0.05f;

	[Header("Oscurecimiento")]
	public float tiempoParaOscurecer = 5f;
	// Arrastra aquí un GameObject con SpriteRenderer negro,
	// mismo tamaño que el fondo, Order in Layer = fondo + 1
	public SpriteRenderer overlayNegro;

	private SpriteRenderer sr;
	private Material       mat;
	private float          timerJuego = 0f;
	private float          offsetX    = 0f;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		if (sr == null)
		{
			Debug.LogWarning("FondoDinamico necesita un SpriteRenderer.");
			return;
		}

		mat = sr.material;

		if (overlayNegro != null)
			overlayNegro.color = new Color(0f, 0f, 0f, 0f);
	}

	void Update()
	{
		if (mat == null) return;

		// --- Parallax ---
		offsetX += velocidadParallax * Time.deltaTime;
		mat.mainTextureOffset = new Vector2(offsetX, 0f);

		// --- Oscurecimiento via overlay ---
		if (overlayNegro == null) return;

		timerJuego += Time.deltaTime;
		float t = Mathf.Clamp01(timerJuego / tiempoParaOscurecer);
		overlayNegro.color = new Color(0f, 0f, 0f, t);
	}

	public void Resetear()
	{
		timerJuego = 0f;
		offsetX    = 0f;

		if (mat != null)
			mat.mainTextureOffset = Vector2.zero;

		if (overlayNegro != null)
			overlayNegro.color = new Color(0f, 0f, 0f, 0f);
	}
}