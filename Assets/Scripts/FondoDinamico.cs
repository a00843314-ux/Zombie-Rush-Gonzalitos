using UnityEngine;
 
public class FondoDinamico : MonoBehaviour

{

	[Header("Padre que contiene los fondos")]

	public Transform contenedorFondos;
 
	[Header("Material compatible con SpriteRenderer")]

	public bool cambiarMaterialAutomaticamente = true;
 
	[Header("Colores del ciclo")]

	public Color colorDia = Color.white;

	public Color colorNoche = new Color(0.10f, 0.13f, 0.30f, 1f);
 
	[Header("Tiempos")]

	public float duracionTransicion = 5f;

	public float tiempoEntreCambios = 3f;
 
	private SpriteRenderer[] fondos;
 
	private bool esDeDia = true;

	private bool enTransicion = false;
 
	private float temporizador = 0f;

	private float progreso = 0f;
 
	private Color colorInicio;

	private Color colorDestino;
 
	private Material materialSpriteCompatible;
 
	void Start()

	{

		BuscarFondos();
 
		if (cambiarMaterialAutomaticamente)

		{

			CrearMaterialCompatible();

			AplicarMaterialCompatible();

		}
 
		AplicarColor(colorDia);
 
		Debug.Log("DayNightCycleAuto2D iniciado. Fondos encontrados: " + fondos.Length);

	}
 
	void Update()

	{

		if (fondos == null || fondos.Length == 0) return;
 
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
 
			Color colorActual = Color.Lerp(colorInicio, colorDestino, progreso);

			AplicarColor(colorActual);
 
			if (progreso >= 1f)

			{

				enTransicion = false;

				temporizador = 0f;

				progreso = 0f;

				esDeDia = !esDeDia;

			}

		}

	}
 
	void BuscarFondos()

	{

		if (contenedorFondos != null)

		{

			fondos = contenedorFondos.GetComponentsInChildren<SpriteRenderer>();

		}

		else

		{

			fondos = FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);

		}

	}
 
	void CrearMaterialCompatible()

	{

		Shader shaderCompatible = null;
 
		shaderCompatible = Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default");
 
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
 
		materialSpriteCompatible = new Material(shaderCompatible);

		materialSpriteCompatible.name = "MAT_Sprite_Compatible_Auto";

	}
 
	void AplicarMaterialCompatible()

	{

		if (materialSpriteCompatible == null) return;
 
		foreach (SpriteRenderer fondo in fondos)

		{

			if (fondo != null)

			{

				fondo.material = materialSpriteCompatible;

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
 
	void AplicarColor(Color color)

	{

		foreach (SpriteRenderer fondo in fondos)

		{

			if (fondo != null)

			{

				fondo.color = color;

			}

		}

	}

}
 