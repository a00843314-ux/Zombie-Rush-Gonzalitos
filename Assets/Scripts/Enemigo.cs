using UnityEngine;

public class Enemigo : MonoBehaviour
{
	[Header("Seguimiento")]
	public float velocidad        = 4f;
	public float distanciaDetras  = 5f;
	public float velocidadAlcance = 15f;
	public float tiempoSeguimiento = 5f;

	private Transform player;
	private bool      activo           = true;
	private bool      alcanzando       = false;
	private float     timerDesaparecer = 0f;

	void Start()
	{
		Player p = FindObjectOfType<Player>();
		if (p != null)
			player = p.transform;
		else
			Debug.LogWarning("No se encontró al Player en la escena.");

		gameObject.SetActive(true);
		activo           = true;
		timerDesaparecer = tiempoSeguimiento;
	}

	void Update()
	{
		if (!activo || player == null) return;

		if (alcanzando)
		{
			MoverHaciaPlayer(velocidadAlcance);
			return;
		}

		float xObjetivo = player.position.x - distanciaDetras;

		if (transform.position.x < xObjetivo)
			MoverHaciaPlayer(velocidad);

		if (timerDesaparecer > 0f)
		{
			timerDesaparecer -= Time.deltaTime;
			if (timerDesaparecer <= 0f)
				Desaparecer();
		}
	}

	void MoverHaciaPlayer(float vel)
	{
		float nuevaX = Mathf.MoveTowards(
			transform.position.x,
			player.position.x,
			vel * Time.deltaTime
		);
		transform.position = new Vector3(nuevaX, transform.position.y, transform.position.z);
	}

	public void AparecerTemporalmente(float segundos)
	{
		gameObject.SetActive(true);
		activo           = true;
		alcanzando       = false;
		timerDesaparecer = segundos;

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
		Debug.Log("El enemigo desapareció");
	}

	public void AlcanzarJugador()
	{
		activo     = true;
		alcanzando = true;
		gameObject.SetActive(true);
	}

	public void AumentarVelocidad(float multiplicador)
	{
		velocidad *= multiplicador;
	}
}