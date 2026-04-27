using UnityEngine;

public class FondoDinamico : MonoBehaviour
{
	[Header("Colores")]
	public Color colorInicial = new Color(0.5f, 0.7f, 1f);
	public Color colorFinal   = new Color(0.05f, 0.05f, 0.1f);

	[Header("Configuracion")]
	public float tiempoParaOscurecer = 120f;

	private SpriteRenderer sr;
	private float          timerJuego = 0f;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		if (sr == null)
			Debug.LogWarning("FondoDinamico necesita un SpriteRenderer en el mismo objeto.");

		sr.color = colorInicial;
	}

	void Update()
	{
		timerJuego += Time.deltaTime;
		Debug.Log($"Timer: {timerJuego} | Color actual: {sr.color}");

		float t = Mathf.Clamp01(timerJuego / tiempoParaOscurecer);
		sr.color = Color.Lerp(colorInicial, colorFinal, t);
	}

	public void Resetear()
	{
		timerJuego = 0f;
		if (sr != null)
			sr.color = colorInicial;
	}
}