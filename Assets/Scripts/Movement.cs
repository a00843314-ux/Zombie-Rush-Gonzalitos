using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
	[Header("Movimiento")]
	public float velocidadInicial = 5f;
	public float aceleracion      = 1.2f;
	public float velocidadMaxima  = 20f;

	[Header("Salto")]
	public float   fuerzaSalto = 10f;
	public KeyCode teclaSalto  = KeyCode.Space;

	[Header("Disparo")]
	public GameObject prefabBala;
	public Transform  puntoDeDisparo;
	public float      cooldownDisparo = 0.3f;
	public KeyCode    teclaDisparo    = KeyCode.E;

	private Rigidbody2D rb;
	private Animator    anim;
	private float       velocidadActual;
	private bool        enSuelo      = false;
	private float       timerDisparo = 0f;

	private static readonly int HashGrounded   = Animator.StringToHash("isGrounded");
	private static readonly int HashVelocityY  = Animator.StringToHash("velocityY");
	private static readonly int HashIsShooting = Animator.StringToHash("isShooting");

	void Start()
	{
		rb              = GetComponent<Rigidbody2D>();
		anim            = GetComponent<Animator>();
		velocidadActual = velocidadInicial;
	}

	void Update()
	{
		// Aceleracion con tope
		velocidadActual = Mathf.Min(
			velocidadActual + aceleracion * Time.deltaTime,
			velocidadMaxima
		);

		// Salto
		if (Input.GetKeyDown(teclaSalto) && enSuelo)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
			rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
			enSuelo = false;
		}

		// Cooldown de disparo
		if (timerDisparo > 0f)
			timerDisparo -= Time.deltaTime;

		// Disparo
		if (Input.GetKeyDown(teclaDisparo) && timerDisparo <= 0f)
			Disparar();

		// Animator
		anim.SetBool (HashGrounded,  enSuelo);
		anim.SetFloat(HashVelocityY, rb.linearVelocity.y);
	}

	void FixedUpdate()
	{
		rb.linearVelocity = new Vector2(velocidadActual, rb.linearVelocity.y);
	}

	void Disparar()
	{
		if (prefabBala == null)
		{
			Debug.LogWarning("Falta asignar el Prefab de la bala en el Inspector.");
			return;
		}
		if (puntoDeDisparo == null)
		{
			Debug.LogWarning("Falta asignar el Punto de Disparo en el Inspector.");
			return;
		}

		Instantiate(prefabBala, puntoDeDisparo.position, Quaternion.identity);
		timerDisparo = cooldownDisparo;
		anim.SetTrigger(HashIsShooting);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		// Suelo
		if (collision.gameObject.CompareTag("Ground"))
			enSuelo = true;

		// Obstaculo: avisar al GameManager
		if (collision.gameObject.CompareTag("Obstacle"))
		{
			Debug.Log("Chocaste con un obstaculo");

			if (GameManager.instancia != null)
				GameManager.instancia.RegistrarFallo();
			else
				Debug.LogWarning("No hay GameManager en la escena.");
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
			enSuelo = false;
	}
}