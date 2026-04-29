using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
	public void Ir(string escena) => SceneManager.LoadScene(escena);
}