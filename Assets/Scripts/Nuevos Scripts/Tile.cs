using UnityEngine;

using UnityEngine.U2D;

using UnityEngine.UI;

using System.Collections;
 
public class Tile : MonoBehaviour

{

	public float speed;

	[SerializeField]

	private Renderer bgRenderer;

	public SpriteRenderer sprite;

	void Start()

	{
 
 
	}
 
	// Update is called once per frame

	void Update()

	{

		bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);

	}
 
}
 