using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] private Transform pivot;
	[SerializeField] private GameObject bulletPrefab;

	private Player playerController;

	private void Awake()
	{
		playerController = GetComponent<Player>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0)) // <-- estaba mal escrito
		{
			Shoot(); // <-- estaba en minúscula
		}
	}

	public void Shoot()
	{
		GameObject bullet = Instantiate(bulletPrefab, pivot.position, Quaternion.identity);
		bullet.GetComponent<Bullet>().SetDirection(playerController.GetDirection());
	}
}