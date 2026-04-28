using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	[Header("Elementos UI")]
	public Text tituloText;
	public Text puntuacionMaximaText;
	public Button botonStart;
	public Button botonTienda;

	[Header("Configuracion")]
	public string nombreJuego      = "MI JUEGO";
	public string nombreEscenaJuego  = "Juego";
	public string nombreEscenaTienda = "Tienda";

	void Start()
	{
		if (tituloText != null)
			tituloText.text = nombreJuego;

		ActualizarPuntuacionMaxima();

		if (botonStart != null)
			botonStart.onClick.AddListener(IniciarJuego);

		if (botonTienda != null)
			botonTienda.onClick.AddListener(AbrirTienda);
	}

	void ActualizarPuntuacionMaxima()
	{
		int maxScore = PlayerPrefs.GetInt("PuntuacionMaxima", 0);
		if (puntuacionMaximaText != null)
			puntuacionMaximaText.text = $"Mejor: {maxScore}";
	}

	public void IniciarJuego()
	{
		SceneManager.LoadScene(nombreEscenaJuego);
	}

	public void AbrirTienda()
	{
		SceneManager.LoadScene(nombreEscenaTienda);
	}
}