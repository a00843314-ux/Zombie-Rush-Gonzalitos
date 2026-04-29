using UnityEngine;

public class GameManager : MonoBehaviour
{
	// Instancia global para que cualquier script pueda acceder
	public static GameManager instancia;

	[Header("Configuracion")]
	public float multiplicadorVelocidad = 1.5f; // cuanto aumenta la velocidad al fallar
	public float tiempoEnemigo          = 4f;   // segundos que aparece el enemigo

	// Estado interno
	private int   fallos        = 0;
	private bool  juegoTerminado = false;

	// Referencia al enemigo para controlarlo desde aqui
	private Enemigo enemigo;

	void Awake()
	{
		// Patron Singleton: solo puede existir un GameManager
		if (instancia == null)
			instancia = this;
		else
			Destroy(gameObject);
	}

	void Start()
	{
		enemigo = FindObjectOfType<Enemigo>();

		// Al inicio el enemigo aparece unos segundos y luego se va
		if (enemigo != null)
			enemigo.AparecerTemporalmente(tiempoEnemigo);
	}

	// Se llama desde Player cuando choca con un obstaculo
	public void RegistrarFallo()
	{
		if (juegoTerminado) return;

		fallos++;
		Debug.Log("Fallos: " + fallos);

		if (fallos >= 2)
		{
			GameOver();
		}
		else
		{
			// Primer fallo: aumentar velocidad y mostrar enemigo
			AumentarVelocidad();

			if (enemigo != null)
				enemigo.AparecerTemporalmente(tiempoEnemigo);
		}
	}

	void AumentarVelocidad()
	{
		// Busca el Player y multiplica su velocidad maxima
		/*Player player = FindObjectOfType<Player>();
		if (player != null)
		{
			player.velocidadMaxima  *= multiplicadorVelocidad;
			player.velocidadInicial *= multiplicadorVelocidad;
		}*/

		// También acelera al enemigo
		if (enemigo != null)
			enemigo.AumentarVelocidad(multiplicadorVelocidad);
	}

	void GameOver()
	{
		juegoTerminado = true;
		Debug.Log("GAME OVER");

		// El enemigo alcanza al jugador y se detiene todo
		if (enemigo != null)
			enemigo.AlcanzarJugador();

		// Aqui puedes agregar: cargar escena de Game Over, mostrar UI, etc.
		// Ejemplo: SceneManager.LoadScene("GameOver");
		Time.timeScale = 0f; // pausa el juego
	}
}