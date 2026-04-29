using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTree : MonoBehaviour
{
	[Header("UI Monedas")]
	public TextMeshProUGUI textoMonedas;

	[Header("Doble Salto")]
	public Button          botonDobleSalto;
	public int             costoDobleSalto = 10;
	public TextMeshProUGUI textoDobleSalto;

	[Header("Escudo")]
	public Button          botonEscudo;
	public int             costoEscudo = 15;
	public TextMeshProUGUI textoEscudo;

	void Start()
	{
		botonDobleSalto.onClick.AddListener(ComprarDobleSalto);
		botonEscudo.onClick.AddListener(ComprarEscudo);
		ActualizarUI();
	}

	void ActualizarUI()
	{
		if (PlayerStats.instancia == null) return;

		textoMonedas.text = $"Monedas: {PlayerStats.instancia.monedas}";

		bool tieneDobleSalto = PlayerStats.instancia.tieneDobleSalto;
		botonDobleSalto.interactable = !tieneDobleSalto;
		textoDobleSalto.text = tieneDobleSalto
			? "✓ Comprado"
			: $"Doble Salto\n{costoDobleSalto} monedas";

		bool tieneEscudo = PlayerStats.instancia.tieneEscudo;
		botonEscudo.interactable = !tieneEscudo;
		textoEscudo.text = tieneEscudo
			? "✓ Comprado"
			: $"Escudo\n{costoEscudo} monedas";
	}

	void ComprarDobleSalto()
	{
		/*if (PlayerStats.instancia.GastarMonedas(costoDobleSalto))
		{
			PlayerStats.instancia.tieneDobleSalto = true;
			ActualizarUI();
		}
		else
		Debug.Log("No hay monedas suficientes.");*/
	}

	void ComprarEscudo()
	{
		/*if (PlayerStats.instancia.GastarMonedas(costoEscudo))
		{
			PlayerStats.instancia.tieneEscudo  = true;
			PlayerStats.instancia.escudoActivo = true;
			ActualizarUI();
		}
		else
		Debug.Log("No hay monedas suficientes.");*/
	}
}