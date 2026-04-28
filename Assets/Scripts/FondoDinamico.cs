using UnityEngine;

public class FondoDinamico : MonoBehaviour
{
	[Header("Colores")]
	public Color colorInicial = new Color(0.5f, 0.7f, 1f);
	public Color colorFinal   = new Color(0.05f, 0.05f, 0.1f);

	[Header("Configuracion")]
	public float tiempoParaOscurecer = 120f;

	private SpriteRenderer sr;
	private float          timerJuego  = 0f;
	private bool           oscureciendo = true;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		if (sr == null)
			Debug.LogWarning("FondoDinamico necesita un SpriteRenderer en el mismo objeto.");

		sr.color = colorInicial;
	}

	void Update()
	{
		if (sr == null) return;

		timerJuego += Time.deltaTime;

		float t = Mathf.Clamp01(timerJuego / tiempoParaOscurecer);

		if (oscureciendo)
			sr.color = Color.Lerp(colorInicial, colorFinal, t);
		else
			sr.color = Color.Lerp(colorFinal, colorInicial, t);

		if (t >= 1f)
		{
			oscureciendo = !oscureciendo;
			timerJuego   = 0f;
		}
	}

	public void Resetear()
	{
		timerJuego   = 0f;
		oscureciendo = true;
		if (sr != null)
			sr.color = colorInicial;
	}
}