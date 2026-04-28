using UnityEngine;

public class Paralax : MonoBehaviour
{
	[Header("Paralax")]
	public float velocidadParalax = 0.5f;

	private float posInicialX;
	private Transform camara;

	void Start()
	{
		camara     = Camera.main.transform;
		posInicialX = transform.position.x;
	}

	void LateUpdate()
	{
		if (camara == null) return;

		transform.localPosition = new Vector3(
			camara.position.x * velocidadParalax * -1f,
			transform.localPosition.y,
			transform.localPosition.z
		);
	}
}
