using System.Collections.Generic;

using UnityEngine;
 
public class GeneradorNivelRunner : MonoBehaviour

{

	[Header("Prefabs del nivel")]

	public GameObject[] prefabsNivel;
 
	[Header("Cámara")]

	public Transform camara;
 
	[Header("Movimiento")]

	public float velocidadNivel = 2f;

	public float velocidadMaxima = 6f;

	public float incrementoVelocidadPorSegundo = 0.08f;
 
	[Header("Generación endless")]

	public float distanciaGeneracionDerecha = 35f;

	public float xInicial = 10f;

	public float limiteIzquierdo = -30f;
 
	[Header("Separación entre piezas")]

	public float separacionMinima = 1.5f;

	public float separacionMaxima = 3f;
 
	[Header("Altura")]

	public bool usarAlturaAleatoria = false;

	public float alturaFija = -3f;

	public float alturaMinima = -3f;

	public float alturaMaxima = -1f;
 
	[Header("Control interno")]

	public int piezasIniciales = 8;
 
	private List<GameObject> piezasActivas = new List<GameObject>();
 
	void Start()

	{

		if (camara == null && Camera.main != null)

		{

			camara = Camera.main.transform;

		}
 
		GenerarPiezasIniciales();

	}
 
	void Update()

	{

		AumentarVelocidadProgresivamente();
 
		LimpiarLista();
 
		AplicarVelocidadATodas();
 
		if (NecesitaGenerarPieza())

		{

			GenerarPiezaAlFinal();

		}

	}
 
	void AumentarVelocidadProgresivamente()

	{

		velocidadNivel += incrementoVelocidadPorSegundo * Time.deltaTime;

		velocidadNivel = Mathf.Clamp(velocidadNivel, 0f, velocidadMaxima);

	}
 
	void GenerarPiezasIniciales()

	{

		float siguienteBordeDerecho = xInicial;
 
		for (int i = 0; i < piezasIniciales; i++)

		{

			siguienteBordeDerecho = GenerarPiezaEnX(siguienteBordeDerecho);

		}

	}
 
	bool NecesitaGenerarPieza()

	{

		float bordeDerechoMasLejano = ObtenerBordeDerechoMasLejano();
 
		float xCamara = camara != null ? camara.position.x : 0f;

		float limiteDerechoNecesario = xCamara + distanciaGeneracionDerecha;
 
		return bordeDerechoMasLejano < limiteDerechoNecesario;

	}
 
	void GenerarPiezaAlFinal()

	{

		float bordeDerechoMasLejano = ObtenerBordeDerechoMasLejano();
 
		if (bordeDerechoMasLejano < xInicial)

		{

			bordeDerechoMasLejano = xInicial;

		}
 
		GenerarPiezaEnX(bordeDerechoMasLejano);

	}
 
	float GenerarPiezaEnX(float bordeDerechoAnterior)

	{

		if (prefabsNivel == null || prefabsNivel.Length == 0)

		{

			Debug.LogWarning("No hay prefabs asignados en Prefabs Nivel.");

			return bordeDerechoAnterior;

		}
 
		GameObject prefab = prefabsNivel[Random.Range(0, prefabsNivel.Length)];
 
		float separacion = Random.Range(separacionMinima, separacionMaxima);
 
		float altura = usarAlturaAleatoria

			? Random.Range(alturaMinima, alturaMaxima)

			: alturaFija;
 
		Vector3 posicionTemporal = new Vector3(

			bordeDerechoAnterior + separacion,

			altura,

			0f

		);
 
		GameObject pieza = Instantiate(prefab, posicionTemporal, Quaternion.identity);
 
		MoverNivelIzquierda mover = pieza.GetComponent<MoverNivelIzquierda>();
 
		if (mover == null)

		{

			mover = pieza.AddComponent<MoverNivelIzquierda>();

		}
 
		mover.velocidad = velocidadNivel;

		mover.limiteIzquierdo = limiteIzquierdo;
 
		Bounds bounds = ObtenerBoundsPieza(pieza);
 
		float bordeIzquierdoActual = bounds.min.x;

		float bordeIzquierdoDeseado = bordeDerechoAnterior + separacion;
 
		float correccionX = bordeIzquierdoDeseado - bordeIzquierdoActual;
 
		pieza.transform.position += new Vector3(correccionX, 0f, 0f);
 
		bounds = ObtenerBoundsPieza(pieza);
 
		piezasActivas.Add(pieza);
 
		return bounds.max.x;

	}
 
	float ObtenerBordeDerechoMasLejano()

	{

		LimpiarLista();
 
		if (piezasActivas.Count == 0)

		{

			return xInicial;

		}
 
		float maxX = -999999f;
 
		foreach (GameObject pieza in piezasActivas)

		{

			if (pieza == null) continue;
 
			Bounds bounds = ObtenerBoundsPieza(pieza);
 
			if (bounds.max.x > maxX)

			{

				maxX = bounds.max.x;

			}

		}
 
		return maxX;

	}
 
	void AplicarVelocidadATodas()

	{

		foreach (GameObject pieza in piezasActivas)

		{

			if (pieza == null) continue;
 
			MoverNivelIzquierda mover = pieza.GetComponent<MoverNivelIzquierda>();
 
			if (mover == null)

			{

				mover = pieza.AddComponent<MoverNivelIzquierda>();

			}
 
			mover.velocidad = velocidadNivel;

			mover.limiteIzquierdo = limiteIzquierdo;

		}

	}
 
	void LimpiarLista()

	{

		piezasActivas.RemoveAll(pieza => pieza == null);

	}
 
	Bounds ObtenerBoundsPieza(GameObject pieza)

	{

		Renderer[] renderers = pieza.GetComponentsInChildren<Renderer>();
 
		if (renderers.Length > 0)

		{

			Bounds bounds = renderers[0].bounds;
 
			for (int i = 1; i < renderers.Length; i++)

			{

				bounds.Encapsulate(renderers[i].bounds);

			}
 
			return bounds;

		}
 
		Collider2D[] colliders = pieza.GetComponentsInChildren<Collider2D>();
 
		if (colliders.Length > 0)

		{

			Bounds bounds = colliders[0].bounds;
 
			for (int i = 1; i < colliders.Length; i++)

			{

				bounds.Encapsulate(colliders[i].bounds);

			}
 
			return bounds;

		}
 
		return new Bounds(pieza.transform.position, Vector3.one);

	}

}
 