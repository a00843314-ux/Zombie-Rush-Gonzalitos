using UnityEngine;
 
public class MoverNivelIzquierda : MonoBehaviour

{

	[Header("Movimiento")]

	public float velocidad = 3f;
 
	[Header("Destrucción")]

	public float limiteIzquierdo = -30f;
 
	void Update()

	{

		transform.position += Vector3.left * velocidad * Time.deltaTime;
 
		Bounds bounds = ObtenerBounds();
 
		if (bounds.max.x < limiteIzquierdo)

		{

			Destroy(gameObject);

		}

	}
 
	Bounds ObtenerBounds()

	{

		Renderer[] renderers = GetComponentsInChildren<Renderer>();
 
		if (renderers.Length > 0)

		{

			Bounds bounds = renderers[0].bounds;
 
			for (int i = 1; i < renderers.Length; i++)

			{

				bounds.Encapsulate(renderers[i].bounds);

			}
 
			return bounds;

		}
 
		Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
 
		if (colliders.Length > 0)

		{

			Bounds bounds = colliders[0].bounds;
 
			for (int i = 1; i < colliders.Length; i++)

			{

				bounds.Encapsulate(colliders[i].bounds);

			}
 
			return bounds;

		}
 
		return new Bounds(transform.position, Vector3.one);

	}

}
 