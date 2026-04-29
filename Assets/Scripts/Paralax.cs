using UnityEngine;
 
[RequireComponent(typeof(SpriteRenderer))]

public class Paralax : MonoBehaviour

{

	[Header("Parallax")]

	public float velocidadParallax = 0.2f;

	public bool moverEnX = true;

	public bool moverEnY = false;

	public bool invertirMovimiento = true;
 
	[Header("Día / Noche")]

	public bool activarDiaNoche = true;

	public Color colorDia = Color.white;

	public Color colorNoche = new Color(0.10f, 0.13f, 0.30f, 1f);
 
	[Header("Tiempos")]

	public float duracionTransicion = 5f;

	public float tiempoEntreCambios = 3f;
 
	[Header("Material")]

	public bool cambiarMaterialAutomaticamente = true;
 
	private Transform camara;

	private SpriteRenderer spriteRenderer;
 
	private Vector3 posicionInicialFondo;

	private Vector3 posicionInicialCamara;
 
	private bool esDeDia = true;

	private bool enTransicion = false;
 
	private float temporizador = 0f;

	private float progreso = 0f;
 
	private Color colorInicio;

	private Color colorDestino;
 
	void Start()

	{

		spriteRenderer = GetComponent<SpriteRenderer>();
 
		if (Camera.main != null)

		{

			camara = Camera.main.transform;

			posicionInicialCamara = camara.position;

		}
 
		posicionInicialFondo = transform.position;
 
		if (cambiarMaterialAutomaticamente)

		{

			CambiarMaterialCompatible();

		}
 
		spriteRenderer.color = colorDia;

	}
 
	void Update()

	{

		if (activarDiaNoche)

		{

			ActualizarDiaNoche();

		}

	}
 
	void LateUpdate()

	{

		AplicarParallax();

	}
 
	void AplicarParallax()

	{

		if (camara == null) return;
 
		Vector3 desplazamientoCamara = camara.position - posicionInicialCamara;
 
		float direccion = invertirMovimiento ? -1f : 1f;
 
		float nuevoX = posicionInicialFondo.x;

		float nuevoY = posicionInicialFondo.y;
 
		if (moverEnX)

		{

			nuevoX = posicionInicialFondo.x + desplazamientoCamara.x * velocidadParallax * direccion;

		}
 
		if (moverEnY)

		{

			nuevoY = posicionInicialFondo.y + desplazamientoCamara.y * velocidadParallax * direccion;

		}
 
		transform.position = new Vector3(

			nuevoX,

			nuevoY,

			posicionInicialFondo.z

		);

	}
 
	void ActualizarDiaNoche()

	{

		if (!enTransicion)

		{

			temporizador += Time.deltaTime;
 
			if (temporizador >= tiempoEntreCambios)

			{

				IniciarTransicion();

			}

		}

		else

		{

			progreso += Time.deltaTime / duracionTransicion;

			progreso = Mathf.Clamp01(progreso);
 
			spriteRenderer.color = Color.Lerp(colorInicio, colorDestino, progreso);
 
			if (progreso >= 1f)

			{

				enTransicion = false;

				temporizador = 0f;

				progreso = 0f;

				esDeDia = !esDeDia;

			}

		}

	}
 
	void IniciarTransicion()

	{

		enTransicion = true;

		progreso = 0f;
 
		colorInicio = esDeDia ? colorDia : colorNoche;

		colorDestino = esDeDia ? colorNoche : colorDia;

	}
 
	void CambiarMaterialCompatible()

	{

		Shader shaderCompatible = Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default");
 
		if (shaderCompatible == null)

		{

			shaderCompatible = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");

		}
 
		if (shaderCompatible == null)

		{

			shaderCompatible = Shader.Find("Sprites/Default");

		}
 
		if (shaderCompatible == null)

		{

			Debug.LogWarning("No se encontró un shader compatible para SpriteRenderer.");

			return;

		}
 
		Material materialCompatible = new Material(shaderCompatible);

		materialCompatible.name = "MAT_Sprite_Compatible_" + gameObject.name;
 
		spriteRenderer.material = materialCompatible;

	}

}
 