using System.Collections.Generic;

using UnityEngine;
 
public class GeneradorBackgroundRunner : MonoBehaviour

{

	[Header("Prefabs del Background")]

	public GameObject[] prefabsBackground;
 
	[Header("Cámara")]

	public Transform camara;
 
	[Header("Movimiento")]

	public float velocidadBackground = 1.5f;

	public float velocidadMaxima = 4f;

	public float incrementoVelocidadPorSegundo = 0.03f;
 
	[Header("Generación infinita")]

	public float distanciaGeneracionDerecha = 40f;

	public float distanciaDestruccionIzquierda = 25f;

	public float xInicial = 0f;
 
	[Header("Separación entre fondos")]

	public float separacionMinima = 0f;

	public float separacionMaxima = 0f;
 
	[Header("Altura")]

	public bool usarAlturaAleatoria = false;

	public float alturaFija = 0f;

	public float alturaMinima = -1f;

	public float alturaMaxima = 1f;
 
	[Header("Profundidad / Z")]

	public float posicionZ = 10f;
 
	[Header("Control interno")]

	public int fondosIniciales = 5;
 
	private List<GameObject> fondosActivos = new List<GameObject>();
 
	void Start()

	{

		if (camara == null && Camera.main != null)

		{

			camara = Camera.main.transform;

		}
 
		GenerarFondosIniciales();

	}
 
	void Update()

	{

		AumentarVelocidadProgresivamente();
 
		LimpiarLista();
 
		AplicarVelocidadATodos();
 
		DestruirFondosQueSalieron();
 
		while (NecesitaGenerarFondo())

		{

			GenerarFondoAlFinal();

		}

	}
 
	void AumentarVelocidadProgresivamente()

	{

		velocidadBackground += incrementoVelocidadPorSegundo * Time.deltaTime;

		velocidadBackground = Mathf.Clamp(velocidadBackground, 0f, velocidadMaxima);

	}
 
	void GenerarFondosIniciales()

	{

		float siguienteBordeDerecho = xInicial;
 
		for (int i = 0; i < fondosIniciales; i++)

		{

			siguienteBordeDerecho = GenerarFondoEnX(siguienteBordeDerecho);

		}

	}
 
	bool NecesitaGenerarFondo()

	{

		float bordeDerechoMasLejano = ObtenerBordeDerechoMasLejano();
 
		float xCamara = camara != null ? camara.position.x : 0f;

		float limiteDerechoNecesario = xCamara + distanciaGeneracionDerecha;
 
		return bordeDerechoMasLejano < limiteDerechoNecesario;

	}
 
	void GenerarFondoAlFinal()

	{

		float bordeDerechoMasLejano = ObtenerBordeDerechoMasLejano();
 
		if (bordeDerechoMasLejano < xInicial)

		{

			bordeDerechoMasLejano = xInicial;

		}
 
		GenerarFondoEnX(bordeDerechoMasLejano);

	}
 
	float GenerarFondoEnX(float bordeDerechoAnterior)

	{

		if (prefabsBackground == null || prefabsBackground.Length == 0)

		{

			Debug.LogWarning("No hay prefabs asignados en Prefabs Background.");

			return bordeDerechoAnterior;

		}
 
		GameObject prefab = prefabsBackground[Random.Range(0, prefabsBackground.Length)];
 
		float separacion = Random.Range(separacionMinima, separacionMaxima);
 
		float altura = usarAlturaAleatoria

			? Random.Range(alturaMinima, alturaMaxima)

			: alturaFija;
 
		Vector3 posicionTemporal = new Vector3(

			bordeDerechoAnterior + separacion,

			altura,

			posicionZ

		);
 
		GameObject fondo = Instantiate(prefab, posicionTemporal, Quaternion.identity);
 
		MoverBackgroundIzquierda mover = fondo.GetComponent<MoverBackgroundIzquierda>();
 
		if (mover == null)

		{

			mover = fondo.AddComponent<MoverBackgroundIzquierda>();

		}
 
		mover.velocidad = velocidadBackground;
 
		Bounds bounds = ObtenerBoundsFondo(fondo);
 
		float bordeIzquierdoActual = bounds.min.x;

		float bordeIzquierdoDeseado = bordeDerechoAnterior + separacion;
 
		float correccionX = bordeIzquierdoDeseado - bordeIzquierdoActual;
 
		fondo.transform.position += new Vector3(correccionX, 0f, 0f);
 
		bounds = ObtenerBoundsFondo(fondo);
 
		fondosActivos.Add(fondo);
 
		return bounds.max.x;

	}
 
	void DestruirFondosQueSalieron()

	{

		float xCamara = camara != null ? camara.position.x : 0f;

		float limiteDestruccion = xCamara - distanciaDestruccionIzquierda;
 
		for (int i = fondosActivos.Count - 1; i >= 0; i--)

		{

			GameObject fondo = fondosActivos[i];
 
			if (fondo == null)

			{

				fondosActivos.RemoveAt(i);

				continue;

			}
 
			Bounds bounds = ObtenerBoundsFondo(fondo);
 
			if (bounds.max.x < limiteDestruccion)

			{

				fondosActivos.RemoveAt(i);

				Destroy(fondo);

			}

		}

	}
 
	float ObtenerBordeDerechoMasLejano()

	{

		LimpiarLista();
 
		if (fondosActivos.Count == 0)

		{

			return xInicial;

		}
 
		float maxX = -999999f;
 
		foreach (GameObject fondo in fondosActivos)

		{

			if (fondo == null) continue;
 
			Bounds bounds = ObtenerBoundsFondo(fondo);
 
			if (bounds.max.x > maxX)

			{

				maxX = bounds.max.x;

			}

		}
 
		return maxX;

	}
 
	void AplicarVelocidadATodos()

	{

		foreach (GameObject fondo in fondosActivos)

		{

			if (fondo == null) continue;
 
			MoverBackgroundIzquierda mover = fondo.GetComponent<MoverBackgroundIzquierda>();
 
			if (mover == null)

			{

				mover = fondo.AddComponent<MoverBackgroundIzquierda>();

			}
 
			mover.velocidad = velocidadBackground;

		}

	}
 
	void LimpiarLista()

	{

		fondosActivos.RemoveAll(fondo => fondo == null);

	}
 
	Bounds ObtenerBoundsFondo(GameObject fondo)

	{

		Renderer[] renderers = fondo.GetComponentsInChildren<Renderer>();
 
		if (renderers.Length > 0)

		{

			Bounds bounds = renderers[0].bounds;
 
			for (int i = 1; i < renderers.Length; i++)

			{

				bounds.Encapsulate(renderers[i].bounds);

			}
 
			return bounds;

		}
 
		Collider2D[] colliders = fondo.GetComponentsInChildren<Collider2D>();
 
		if (colliders.Length > 0)

		{

			Bounds bounds = colliders[0].bounds;
 
			for (int i = 1; i < colliders.Length; i++)

			{

				bounds.Encapsulate(colliders[i].bounds);

			}
 
			return bounds;

		}
 
		return new Bounds(fondo.transform.position, Vector3.one);

	}

}
 
public class MoverBackgroundIzquierda : MonoBehaviour

{

	public float velocidad = 1.5f;
 
	void Update()

	{

		transform.Translate(Vector3.left * velocidad * Time.deltaTime, Space.World);

	}

}
 