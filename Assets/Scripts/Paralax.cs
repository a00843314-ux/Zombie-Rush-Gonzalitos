using UnityEngine;

public class Paralax : MonoBehaviour
{
	[Header("Paralax")]
	public float velocidadParalax = 0.5f;

	private Transform jugador;
	private float     ultimaPosicionX;

	void Start()
	{
		Player p = FindObjectOfType<Player>();
		if (p != null)
			jugador = p.transform;
		else
			Debug.LogWarning("No se encontró al Player en la escena.");

		ultimaPosicionX = jugador != null ? jugador.position.x : 0f;
	}

	void LateUpdate()
	{
		if (jugador == null) return;

		float diferencia    = jugador.position.x - ultimaPosicionX;
		transform.position += new Vector3(diferencia * velocidadParalax, 0f, 0f);
		ultimaPosicionX     = jugador.position.x;
	}
}
