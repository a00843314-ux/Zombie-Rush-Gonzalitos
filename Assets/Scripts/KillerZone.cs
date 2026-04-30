using UnityEngine;
using UnityEngine.SceneManagement;

public class KillerZone : MonoBehaviour
{
	public string nombreEscenaMuerte = "Muerte"; // cambia por el nombre exacto de tu escena

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			SceneManager.LoadScene(nombreEscenaMuerte);
		}
	}
}