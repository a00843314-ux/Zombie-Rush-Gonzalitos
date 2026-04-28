using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Vida")]
	public int vida = 1; // cuántos disparos aguanta

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Bullet"))
		{
			vida--;

			if (vida <= 0)
				Morir();
		}
	}

	private void Morir()
	{
		// Aquí puedes agregar animación, partículas, sonido, etc.
		Destroy(gameObject);
	}
}
