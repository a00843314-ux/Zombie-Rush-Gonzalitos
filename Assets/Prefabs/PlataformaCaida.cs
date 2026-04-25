using UnityEngine;

public class PlataformaCaida : MonoBehaviour
{
	[Header("Temblor")]
	public float tiempoTemblor     = 1.2f;
	public float intensidadTemblor = 0.05f;

	[Header("Caída")]
	public float velocidadCaida  = 3f;
	public float tiempoDestruir  = 3f;

	private bool    temblando        = false;
	private bool    cayendo          = false;
	private float   timerTemblor     = 0f;
	private float   timerDestruir    = 0f;
	private Vector3 posicionOriginal;
	private Vector3 posicionVisualOriginal;
	private Rigidbody2D rb;
	private Transform   visual;

	void Start()
	{
		posicionOriginal = transform.position;

		// Buscar el hijo Visual
		visual = transform.Find("Visual");
		if (visual == null)
			Debug.LogWarning($"{gameObject.name} no tiene un hijo llamado 'Visual'.");
		else
			posicionVisualOriginal = visual.localPosition;

		// Agregar Rigidbody2D si no existe
		rb = GetComponent<Rigidbody2D>();
		if (rb == null)
			rb = gameObject.AddComponent<Rigidbody2D>();

		rb.bodyType     = RigidbodyType2D.Kinematic;
		rb.gravityScale = 0f;
	}

	void Update()
	{
		if (temblando)
		{
			timerTemblor -= Time.deltaTime;

			// Solo mover el visual, no el objeto raíz
			if (visual != null)
			{
				visual.localPosition = posicionVisualOriginal + new Vector3(
					Random.Range(-intensidadTemblor, intensidadTemblor),
					Random.Range(-intensidadTemblor, intensidadTemblor),
					0f
				);
			}

			if (timerTemblor <= 0f)
			{
				temblando = false;
				if (visual != null)
					visual.localPosition = posicionVisualOriginal;
				Caer();
			}
		}

		if (cayendo)
		{
			transform.position += Vector3.down * velocidadCaida * Time.deltaTime;

			timerDestruir -= Time.deltaTime;
			if (timerDestruir <= 0f)
				Destroy(gameObject);
		}
	}

	void Caer()
	{
		cayendo        = true;
		timerDestruir  = tiempoDestruir;
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && !temblando && !cayendo)
		{
			temblando        = true;
			timerTemblor     = tiempoTemblor;
			posicionOriginal = transform.position;
		}
	}
}