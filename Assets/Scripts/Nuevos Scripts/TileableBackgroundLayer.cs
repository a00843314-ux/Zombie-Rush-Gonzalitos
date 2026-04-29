using UnityEngine;
 
[RequireComponent(typeof(Renderer))]
public class TileableBackgroundLayer : MonoBehaviour
{
	[Header("Movimiento de la textura")]
	public Vector2 velocidadScroll = new Vector2(0.05f, 0f);
 
	[Header("Repetición de la textura")]
	public Vector2 tiling = new Vector2(3f, 1f);
 
	[Header("Día / noche")]
	public bool usarDiaNoche = true;
	public Color colorDia = Color.white;
	public Color colorNoche = new Color(0.25f, 0.3f, 0.55f, 1f);
	public float duracionCiclo = 20f;
 
	private Renderer renderizador;
	private Material materialInstanciado;
	private Vector2 offsetActual;
 
	private string propiedadTextura;
 
	void Start()
	{
		renderizador = GetComponent<Renderer>();
 
		// Crea una copia del material para este objeto
		materialInstanciado = renderizador.material;
 
		// Detecta si el shader usa _BaseMap o _MainTex
		if (materialInstanciado.HasProperty("_BaseMap"))
		{
			propiedadTextura = "_BaseMap";
		}
		else if (materialInstanciado.HasProperty("_MainTex"))
		{
			propiedadTextura = "_MainTex";
		}
		else
		{
			Debug.LogWarning("El material no tiene _BaseMap ni _MainTex. Cambia el shader a URP/Unlit o URP/Lit.");
			return;
		}
 
		// Forzar repetición
		Texture textura = materialInstanciado.GetTexture(propiedadTextura);
 
		if (textura != null)
		{
			textura.wrapMode = TextureWrapMode.Repeat;
		}
 
		// Aplicar tiling inicial
		materialInstanciado.SetTextureScale(propiedadTextura, tiling);
	}
 
	void Update()
	{
		MoverTextura();
 
		if (usarDiaNoche)
		{
			AplicarDiaNoche();
		}
	}
 
	void MoverTextura()
	{
		if (materialInstanciado == null) return;
		if (string.IsNullOrEmpty(propiedadTextura)) return;
 
		offsetActual += velocidadScroll * Time.deltaTime;
 
		materialInstanciado.SetTextureOffset(propiedadTextura, offsetActual);
		materialInstanciado.SetTextureScale(propiedadTextura, tiling);
	}
 
	void AplicarDiaNoche()
	{
		float ciclo = (Mathf.Sin(Time.time / duracionCiclo * Mathf.PI * 2f) + 1f) / 2f;
 
		Color colorActual = Color.Lerp(colorNoche, colorDia, ciclo);
 
		if (materialInstanciado.HasProperty("_BaseColor"))
		{
			materialInstanciado.SetColor("_BaseColor", colorActual);
		}
		else if (materialInstanciado.HasProperty("_Color"))
		{
			materialInstanciado.SetColor("_Color", colorActual);
		}
	}
}
 
 