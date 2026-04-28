using UnityEngine;

public class Moneda : MonoBehaviour
{
	public int valor = 1;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerStats.instancia.AgregarMonedas(valor);
			Destroy(gameObject);
		}
	}
}