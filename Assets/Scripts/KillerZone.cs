using UnityEngine;
using UnityEngine.SceneManagement;
public class KillerZone : MonoBehaviour
{
	// En el Inspector: añade un BoxCollider2D con IsTrigger = true
	// y estíralo horizontalmente para cubrir toda la zona de caída

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(
				UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
			);
		}
	}
}