using UnityEngine;
using TMPro;

public class PuntajeManager : MonoBehaviour
{
	[Header("UI")]
	public TextMeshProUGUI textoPuntaje;
	public TextMeshProUGUI textoMultiplicador;

	[Header("Distancia")]
	public Transform jugador; // arrastra el Player aquí

	private float distanciaBase   = 0f;
	private float xInicial        = 0f;

	void Start()
	{
		if (jugador == null)
			jugador = GameObject.FindGameObjectWithTag("Player")?.transform;

		if (jugador != null)
			xInicial = jugador.position.x;

		if (textoMultiplicador != null)
			textoMultiplicador.text = $"x{MultiplierData.multiplicadorActual}";
	}

	void Update()
	{
		if (jugador == null) return;

		distanciaBase = jugador.position.x - xInicial;
		float puntajeFinal = distanciaBase * MultiplierData.multiplicadorActual;

		if (textoPuntaje != null)
			textoPuntaje.text = $"Distancia: {puntajeFinal:F0}";
	}

	// Llama esto al morir para guardar score si lo necesitas
	public float ObtenerPuntajeFinal()
	{
		return (jugador.position.x - xInicial) * MultiplierData.multiplicadorActual;
	}
}