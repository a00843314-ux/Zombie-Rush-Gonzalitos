using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private float speed;

	private Rigidbody2D rb;
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
		rb.velocity = direction * speed;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemigo"))
		{
			Destroy(gameObject);
		}
	}

	public void SetDirection(Vector2 _direction)
	{
		direction = _direction;
	}
}