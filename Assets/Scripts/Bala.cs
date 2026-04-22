using UnityEngine;

public class Bala : MonoBehaviour
{
	public float velocidad    = 15f;
	public float tiempoDeVida = 3f;

	void Start()
	{
		// .velocity funciona en todas las versiones de Unity
		GetComponent<Rigidbody2D>().linearVelocity = Vector2.right * velocidad;
		Destroy(gameObject, tiempoDeVida);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Destroy(gameObject);
	}
}
