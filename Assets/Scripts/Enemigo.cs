using UnityEngine;
 
public class Enemigo : MonoBehaviour
 
{
 
	[Header("Referencia")]
 
	public Transform player;
 
	[Header("Posiciones fijas en pantalla")]
 
	[Tooltip("X normal del enemigo. Debe quedar a la izquierda del Player.")]
 
	public float posicionXNormal = -7f;
 
	[Tooltip("X cercana del enemigo. Debe quedar más cerca, pero todavía a la izquierda del Player.")]
 
	public float posicionXCercana = -4f;
 
	[Tooltip("Nunca podrá pasar de esta X. Úsalo como límite de seguridad.")]
 
	public float limiteXMaximo = -3.2f;
 
	[Header("Movimiento")]
 
	public float velocidadNormal = 4f;
 
	public float velocidadAcercamiento = 6f;
 
	public float velocidadRegreso = 5f;
 
	[Header("Acercamientos aleatorios")]
 
	public bool usarAcercamientosAleatorios = true;
 
	public float tiempoMinEntreAcercamientos = 4f;
 
	public float tiempoMaxEntreAcercamientos = 8f;
 
	public float duracionMinAcercamiento = 1f;
 
	public float duracionMaxAcercamiento = 2f;
 
	[Header("Seguimiento vertical")]
 
	public float retrasoSalto = 0.2f;
 
	public float offsetY = 0f;
 
	[Header("Desaparición")]
 
	public bool desaparecerConTiempo = false;
 
	public float tiempoSeguimiento = 5f;
 
	private bool activo = true;
 
	private bool acercandose = false;
 
	private float timerDesaparecer;
 
	private float timerProximoAcercamiento;
 
	private float timerDuracionAcercamiento;
 
	private float[] historialY;
 
	private int indiceHistorial = 0;
 
	private int tamanoHistorial;
 
	void Start()
 
	{
 
		if (player == null)
 
		{
 
			Player p = FindObjectOfType<Player>();
 
			if (p != null)
 
			{
 
				player = p.transform;
 
			}
 
			else
 
			{
 
				Debug.LogWarning("No se encontró al Player en la escena.");
 
				return;
 
			}
 
		}
 
		tamanoHistorial = Mathf.Max(1, Mathf.RoundToInt(retrasoSalto / Time.fixedDeltaTime));
 
		historialY = new float[tamanoHistorial];
 
		for (int i = 0; i < tamanoHistorial; i++)
 
		{
 
			historialY[i] = player.position.y;
 
		}
 
		activo = true;
 
		acercandose = false;
 
		timerDesaparecer = tiempoSeguimiento;
 
		ProgramarSiguienteAcercamiento();
 
		transform.position = new Vector3(
 
			posicionXNormal,
 
			player.position.y + offsetY,
 
			transform.position.z
 
		);
 
	}
 
	void Update()
 
	{
 
		if (!activo || player == null) return;
 
		ActualizarHistorialY();
 
		if (usarAcercamientosAleatorios)
 
		{
 
			ControlarAcercamientosAleatorios();
 
		}
 
		float yRetrasada = historialY[indiceHistorial] + offsetY;
 
		float xObjetivo = acercandose ? posicionXCercana : posicionXNormal;
 
		xObjetivo = Mathf.Min(xObjetivo, limiteXMaximo);
 
		float velocidadActual = acercandose ? velocidadAcercamiento : velocidadRegreso;
 
		float nuevaX = Mathf.MoveTowards(
 
			transform.position.x,
 
			xObjetivo,
 
			velocidadActual * Time.deltaTime
 
		);
 
		nuevaX = Mathf.Min(nuevaX, limiteXMaximo);
 
		transform.position = new Vector3(
 
			nuevaX,
 
			yRetrasada,
 
			transform.position.z
 
		);
 
		if (desaparecerConTiempo)
 
		{
 
			timerDesaparecer -= Time.deltaTime;
 
			if (timerDesaparecer <= 0f)
 
			{
 
				Desaparecer();
 
			}
 
		}
 
	}
 
	void ActualizarHistorialY()
 
	{
 
		historialY[indiceHistorial] = player.position.y;
 
		indiceHistorial = (indiceHistorial + 1) % tamanoHistorial;
 
	}
 
	void ControlarAcercamientosAleatorios()
 
	{
 
		if (!acercandose)
 
		{
 
			timerProximoAcercamiento -= Time.deltaTime;
 
			if (timerProximoAcercamiento <= 0f)
 
			{
 
				IniciarAcercamiento();
 
			}
 
		}
 
		else
 
		{
 
			timerDuracionAcercamiento -= Time.deltaTime;
 
			if (timerDuracionAcercamiento <= 0f)
 
			{
 
				TerminarAcercamiento();
 
			}
 
		}
 
	}
 
	void IniciarAcercamiento()
 
	{
 
		acercandose = true;
 
		timerDuracionAcercamiento = Random.Range(
 
			duracionMinAcercamiento,
 
			duracionMaxAcercamiento
 
		);
 
	}
 
	void TerminarAcercamiento()
 
	{
 
		acercandose = false;
 
		ProgramarSiguienteAcercamiento();
 
	}
 
	void ProgramarSiguienteAcercamiento()
 
	{
 
		timerProximoAcercamiento = Random.Range(
 
			tiempoMinEntreAcercamientos,
 
			tiempoMaxEntreAcercamientos
 
		);
 
	}
 
	public void AparecerTemporalmente(float segundos)
 
	{
 
		gameObject.SetActive(true);
 
		activo = true;
 
		acercandose = false;
 
		timerDesaparecer = segundos;
 
		desaparecerConTiempo = true;
 
		ProgramarSiguienteAcercamiento();
 
		if (player != null)
 
		{
 
			for (int i = 0; i < tamanoHistorial; i++)
 
			{
 
				historialY[i] = player.position.y;
 
			}
 
			transform.position = new Vector3(
 
				posicionXNormal,
 
				player.position.y + offsetY,
 
				transform.position.z
 
			);
 
		}
 
	}
 
	public void AparecerPermanente()
 
	{
 
		gameObject.SetActive(true);
 
		activo = true;
 
		acercandose = false;
 
		desaparecerConTiempo = false;
 
		ProgramarSiguienteAcercamiento();
 
		if (player != null)
 
		{
 
			transform.position = new Vector3(
 
				posicionXNormal,
 
				player.position.y + offsetY,
 
				transform.position.z
 
			);
 
		}
 
	}
 
	public void AlcanzarJugador()
 
	{
 
		gameObject.SetActive(true);
 
		activo = true;
 
		acercandose = true;
 
		desaparecerConTiempo = false;
 
		timerDuracionAcercamiento = Random.Range(
 
			duracionMinAcercamiento,
 
			duracionMaxAcercamiento
 
		);
 
	}
 
	void Desaparecer()
 
	{
 
		activo = false;
 
		gameObject.SetActive(false);
 
	}
 
	public void AumentarVelocidad(float multiplicador)
 
	{
 
		velocidadNormal *= multiplicador;
 
		velocidadAcercamiento *= multiplicador;
 
		velocidadRegreso *= multiplicador;
 
	}
 
}
 