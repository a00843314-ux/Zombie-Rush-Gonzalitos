using UnityEngine;

public class FondoMovimiento : MonoBehaviour
{
	[SerializeField] private Vector2 VelocidadMovimiento;
	
	private Vector2 offset; 
	
	private Material material; 
	
	private void Awake ()
	{
		material = GetComponent<SpriteRenderer>().material;
	}
	
	private void Update () 
	{	
		offset = VelocidadMovimiento * Time.deltaTime;
		material.mainTextureOffset += offset;
	}
}
