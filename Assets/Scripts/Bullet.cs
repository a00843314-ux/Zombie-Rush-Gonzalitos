using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private float speed = 15f; // súbelo aquí si va lenta

	private Rigidbody2D rb; // <-- estaba RigidBody2D con B mayúscula, error de compilación
	private Vector2 direction;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		Destroy(gameObject, 2f);
	}

	private void FixedUpdate()
	{
		rb.linearVelocity = direction * speed;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			Destroy(gameObject);
		}
	}

	public void SetDirection(Vector2 _direction) // <-- faltaba espacio entre Vector2 y _direction
	{
		direction = _direction;
	}
}