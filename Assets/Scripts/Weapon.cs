using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] private Transform pivot;
	[SerializeField] private GameObject bulletPrefab;

	private Player movement;

	private void Awake()
	{
		movement = GetComponent<Player>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
	}

	public void Shoot()
	{
		GameObject bullet = Instantiate(bulletPrefab, pivot.position, Quaternion.identity);
		bullet.GetComponent<Bullet>().SetDirection(movement.GetDirection());
	}
}