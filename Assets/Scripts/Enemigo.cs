using UnityEngine;

public class Enemigo : MonoBehaviour
{
	[Header("Seguimiento")]
	public float velocidad        = 4f;   // velocidad base del enemigo
	public float distanciaDetras  = 5f;   // cuanto se queda atras del player normalmente
	public float velocidadAlcance = 15f;  // velocidad cuando va a alcanzar al player

	// Referencias
	private Transform player;
	private bool      activo        = false; // si el enemigo esta visible y siguiendo
	private bool      alcanzando    = false; // modo game over: va a maxima velocidad
	private float     timerDesaparecer = 0f;

	void Start()
	{
		// Buscar al player automaticamente
		player = FindObjectOfType<Player>().transform;

		// Empezar invisible
		gameObject.SetActive(true);
	}

	void Update()
	{
		if (!activo || player == null) return;

		// --- Modo alcance (game over) ---
		if (alcanzando)
		{
			MoverHaciaPlayer(velocidadAlcance);
			return;
		}

		// --- Modo normal: seguir manteniendo distancia ---
		// Posicion objetivo: detras del player
		float xObjetivo = player.position.x - distanciaDetras;

		// Solo avanzar si el enemigo esta mas atras que su posicion objetivo
		if (transform.position.x < xObjetivo)
		{
			MoverHaciaPlayer(velocidad);
		}

		// --- Temporizador para desaparecer ---
		if (timerDesaparecer > 0f)
		{
			timerDesaparecer -= Time.deltaTime;

			if (timerDesaparecer <= 0f)
				Desaparecer();
		}
	}

	void MoverHaciaPlayer(float vel)
	{
		// Mover en X hacia el player
		float nuevaX = Mathf.MoveTowards(
			transform.position.x,
			player.position.x,
			vel * Time.deltaTime
		);

		transform.position = new Vector3(nuevaX, transform.position.y, transform.position.z);
	}

	// Llamado desde GameManager: aparecer N segundos y luego irse
	public void AparecerTemporalmente(float segundos)
	{
		gameObject.SetActive(true);
		activo             = true;
		alcanzando         = false;
		timerDesaparecer   = segundos;

		// Colocarlo detras del player al aparecer
		if (player != null)
		{
			transform.position = new Vector3(
				player.position.x - distanciaDetras,
				transform.position.y,
				transform.position.z
			);
		}
	}

	void Desaparecer()
	{
		activo = false;
		gameObject.SetActive(false);
		Debug.Log("El enemigo desaparecio");
	}

	// Llamado desde GameManager en game over
	public void AlcanzarJugador()
	{
		activo     = true;
		alcanzando = true;
		gameObject.SetActive(true);
	}

	// Llamado desde GameManager al aumentar dificultad
	public void AumentarVelocidad(float multiplicador)
	{
		velocidad *= multiplicador;
	}
}