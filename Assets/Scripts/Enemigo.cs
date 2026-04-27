using UnityEngine;

public class Enemigo : MonoBehaviour
{
	[Header("Seguimiento")]
	public float velocidad         = 4f;
	public float distanciaDetras   = 5f;
	public float velocidadAlcance  = 15f;
	public float tiempoSeguimiento = 5f;

	[Header("Retraso de salto")]
	public float retrasoSalto = 0.2f;

	[Header("Ajuste visual")]
	public float offsetY = 0f;

	private Transform player;
	private bool      activo           = true;
	private bool      alcanzando       = false;
	private float     timerDesaparecer = 0f;

	private float[] historialY;
	private int     indiceHistorial = 0;
	private int     tamanoHistorial;

	void Start()
	{
		Player p = FindObjectOfType<Player>();
		if (p != null)
			player = p.transform;
		else
			Debug.LogWarning("No se encontró al Player en la escena.");

		tamanoHistorial = Mathf.Max(1, Mathf.RoundToInt(retrasoSalto / Time.fixedDeltaTime));
		historialY      = new float[tamanoHistorial];

		for (int i = 0; i < tamanoHistorial; i++)
			historialY[i] = player != null ? player.position.y : transform.position.y;

		gameObject.SetActive(true);
		activo           = true;
		timerDesaparecer = tiempoSeguimiento;
	}

	void Update()
	{
		if (!activo || player == null) return;

		historialY[indiceHistorial] = player.position.y;
		indiceHistorial = (indiceHistorial + 1) % tamanoHistorial;

		float yRetrasada = historialY[indiceHistorial] + offsetY;

		if (alcanzando)
		{
			MoverHaciaPlayer(velocidadAlcance, yRetrasada);
			return;
		}

		float xObjetivo = player.position.x - distanciaDetras;
		float nuevaX    = Mathf.MoveTowards(transform.position.x, xObjetivo, velocidad * Time.deltaTime);

		transform.position = new Vector3(nuevaX, yRetrasada, transform.position.z);

		if (timerDesaparecer > 0f)
		{
			timerDesaparecer -= Time.deltaTime;
			if (timerDesaparecer <= 0f)
				Desaparecer();
		}
	}

	void MoverHaciaPlayer(float vel, float yRetrasada)
	{
		float nuevaX = Mathf.MoveTowards(
			transform.position.x,
			player.position.x,
			vel * Time.deltaTime
		);
		transform.position = new Vector3(nuevaX, yRetrasada, transform.position.z);
	}

	public void AparecerTemporalmente(float segundos)
	{
		gameObject.SetActive(true);
		activo           = true;
		alcanzando       = false;
		timerDesaparecer = segundos;

		if (player != null)
		{
			for (int i = 0; i < tamanoHistorial; i++)
				historialY[i] = player.position.y;

			transform.position = new Vector3(
				player.position.x - distanciaDetras,
				player.position.y + offsetY,
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