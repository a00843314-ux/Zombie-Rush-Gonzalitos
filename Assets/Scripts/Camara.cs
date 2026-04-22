using UnityEngine;

public class CamaraSeguir : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	// Update is called once per frame
	public Transform jugador;
	public float speed = 5.0f;
	void Update()
	{
		Vector3 Destino = new Vector3 (jugador.position.x, jugador.position.y,transform.position.z);
		transform.position = Vector3.Lerp (transform.position, Destino, speed* Time.deltaTime);
	}
}
