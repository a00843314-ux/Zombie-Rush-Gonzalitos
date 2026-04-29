using UnityEngine;
 
public class Weapon : MonoBehaviour

{

	[SerializeField] private Transform pivot;

	[SerializeField] private GameObject bulletPrefab;
 
	private Player playerController;
 
	private void Awake()

	{

		playerController = GetComponentInParent<Player>();

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

		if (bulletPrefab == null)

		{

			Debug.LogWarning("Falta asignar Bullet Prefab en Weapon.");

			return;

		}
 
		if (pivot == null)

		{

			Debug.LogWarning("Falta asignar Pivot en Weapon.");

			return;

		}
 
		if (playerController == null)

		{

			Debug.LogWarning("No se encontró el componente Player.");

			return;

		}
 
		playerController.AnimarDisparo();
 
		GameObject bullet = Instantiate(

			bulletPrefab,

			pivot.position,

			Quaternion.identity

		);
 
		Bullet bulletScript = bullet.GetComponent<Bullet>();
 
		if (bulletScript != null)

		{

			bulletScript.SetDirection(playerController.GetDirection());

		}

	}

}
 