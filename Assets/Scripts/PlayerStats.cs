using UnityEngine;

using UnityEngine.SceneManagement;

using TMPro;
 
public class PlayerStats : MonoBehaviour

{

	public static PlayerStats instancia;
 
	[Header("Monedas")]

	public int monedas = 0;
 
	[Header("UI")]

	[Tooltip("Arrastra aquí el TextMeshProUGUI. Si está en otra escena, " +

	"se buscará automáticamente por la etiqueta 'TextoMonedas'.")]

	public TextMeshProUGUI textoMonedas;
 
	[Header("Habilidades")]

	public bool tieneDobleSalto = false;

	public bool tieneEscudo     = false;
 
	[HideInInspector] public int  saltosRestantes = 1;

	[HideInInspector] public bool escudoActivo    = false;
 
	private const string CLAVE_MONEDAS = "MonedasJugador";
 
	// Bandera para evitar doble suscripción al evento de escena

	private bool suscritoAEvento = false;
 
	// ───────────────────────────────────────────────

	// CICLO DE VIDA

	// ───────────────────────────────────────────────

	void Awake()

	{

		// Singleton: si ya existe otra instancia, esta se autodestruye.

		if (instancia != null && instancia != this)

		{

			Destroy(gameObject);

			return;

		}
 
		instancia = this;

		DontDestroyOnLoad(gameObject);

		CargarMonedas();

	}
 
	void OnEnable()

	{

		if (instancia == this && !suscritoAEvento)

		{

			SceneManager.sceneLoaded += AlCargarEscena;

			suscritoAEvento = true;

		}

	}
 
	void OnDisable()

	{

		if (suscritoAEvento)

		{

			SceneManager.sceneLoaded -= AlCargarEscena;

			suscritoAEvento = false;

		}

	}
 
	void Start()

	{

		BuscarTextoEnEscena();

		ActualizarUI();

	}
 
	// Guardar al cerrar el juego (PC)

	void OnApplicationQuit()

	{

		GuardarMonedas();

	}
 
	// Guardar cuando la app pasa a segundo plano (móvil)

	void OnApplicationPause(bool pausado)

	{

		if (pausado) GuardarMonedas();

	}
 
	// ───────────────────────────────────────────────

	// EVENTOS DE ESCENA

	// ───────────────────────────────────────────────

	private void AlCargarEscena(Scene escena, LoadSceneMode modo)

	{

		BuscarTextoEnEscena();

		ActualizarUI();

	}
 
	private void BuscarTextoEnEscena()

	{

		if (textoMonedas == null)

		{

			GameObject obj = GameObject.FindWithTag("TextoMonedas");

			if (obj != null)

				textoMonedas = obj.GetComponent<TextMeshProUGUI>();

		}

	}
 
	// ───────────────────────────────────────────────

	// MONEDAS

	// ───────────────────────────────────────────────

	public void AgregarMonedas(int cantidad)

	{

		monedas += cantidad;

		ActualizarUI();

	}
 
	private void ActualizarUI()

	{

		if (textoMonedas == null)

			BuscarTextoEnEscena();
 
		if (textoMonedas != null)

			textoMonedas.text = "Monedas: " + monedas;

	}
 
	// ───────────────────────────────────────────────

	// PERSISTENCIA (PlayerPrefs)

	// ───────────────────────────────────────────────

	private void GuardarMonedas()

	{

		PlayerPrefs.SetInt(CLAVE_MONEDAS, monedas);

		PlayerPrefs.Save();

	}
 
	private void CargarMonedas()

	{

		monedas = PlayerPrefs.GetInt(CLAVE_MONEDAS, 0);

	}
 
	// Llama a esto para forzar el guardado (p. ej. al pasar de nivel)

	public void GuardarManual()

	{

		GuardarMonedas();

	}
 
	// Reiniciar el contador (nueva partida)

	public void ReiniciarMonedas()

	{

		monedas = 0;

		GuardarMonedas();

		ActualizarUI();

	}

}
 