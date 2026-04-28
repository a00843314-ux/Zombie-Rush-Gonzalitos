using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	public static PlayerStats instancia;

	[Header("Monedas")]
	public int monedas = 0;

	[Header("Habilidades")]
	public bool tieneDobleSalto = false;
	public bool tieneEscudo     = false;

	[HideInInspector] public int  saltosRestantes = 1;
	[HideInInspector] public bool escudoActivo    = false;

	void Awake()
	{
		if (instancia == null) instancia = this;
		else Destroy(gameObject);
	}

	public void AgregarMonedas(int cantidad)
	{
		monedas += cantidad;
	}

	public bool GastarMonedas(int cantidad)
	{
		if (monedas >= cantidad)
		{
			monedas -= cantidad;
			return true;
		}
		return false;
	}

	public void ActivarEscudo()
	{
		escudoActivo = true;
	}

	public void UsarEscudo()
	{
		escudoActivo = false;
	}
}