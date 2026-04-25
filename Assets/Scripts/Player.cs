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

	[Header("Daño")]
	public float fuerzaKnockback  = 8f;
	public float tiempoInvencible = 3f;

	private Rigidbody2D rb;
	private Animator    anim;
	private float       velocidadActual;
	private bool        enSuelo      = false;
	private float       timerDisparo = 0f;

	private bool  invencible      = false;
	private float timerInvencible = 0f;
	private bool  muerto          = false;

	private static readonly int HashGrounded    = Animator.StringToHash("isGrounded");
	private static readonly int HashVelocityY   = Animator.StringToHash("velocityY");
	private static readonly int HashIsShooting  = Animator.StringToHash("isShooting");
	private static readonly int HashRecibirDano = Animator.StringToHash("RecibirDano");
	private static readonly int HashMuerte      = Animator.StringToHash("Muerte");

	void Start()
	{
		rb              = GetComponent<Rigidbody2D>();
		anim            = GetComponent<Animator>();
		velocidadActual = velocidadInicial;
	}

	void Update()
	{
		if (muerto) return;

		velocidadActual = Mathf.Min(
			velocidadActual + aceleracion * Time.deltaTime,
			velocidadMaxima
		);

		if (invencible)
		{
			timerInvencible -= Time.deltaTime;
			if (timerInvencible <= 0f)
				invencible = false;
		}

		if (Input.GetKeyDown(teclaSalto) && enSuelo)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
			rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
			enSuelo = false;
		}

		if (timerDisparo > 0f)
			timerDisparo -= Time.deltaTime;

		if (Input.GetKeyDown(teclaDisparo) && timerDisparo <= 0f)
			Disparar();

		anim.SetBool (HashGrounded,  enSuelo);
		anim.SetFloat(HashVelocityY, rb.linearVelocity.y);
	}

	void FixedUpdate()
	{
		if (muerto) return;
		rb.linearVelocity = new Vector2(velocidadActual, rb.linearVelocity.y);
	}

	void RecibirGolpe()
	{
		if (invencible || muerto) return;

		anim.SetTrigger(HashRecibirDano);

		rb.linearVelocity = Vector2.zero;
		rb.AddForce(Vector2.left * fuerzaKnockback, ForceMode2D.Impulse);

		invencible      = true;
		timerInvencible = tiempoInvencible;
	}

	public void Morir()
	{
		if (muerto) return;

		muerto            = true;
		rb.linearVelocity = Vector2.zero;
		rb.bodyType       = RigidbodyType2D.Kinematic;

		anim.SetTrigger(HashMuerte);

		if (GameManager.instancia != null)
			GameManager.instancia.RegistrarFallo();
		else
			Debug.LogWarning("No hay GameManager en la escena.");
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

		Instantiate(prefabBala, puntoDeDisparo.position, puntoDeDisparo.rotation);
		timerDisparo = cooldownDisparo;
		anim.SetTrigger(HashIsShooting);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
			enSuelo = true;

		if (collision.gameObject.CompareTag("RecibirDano"))
			RecibirGolpe();
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
			enSuelo = false;
	}

	public Vector2 GetDirection()
	{
		return Vector2.right;
	}

	void OnEnable()
	{
		muerto          = false;
		invencible      = false;
		timerInvencible = 0f;
		velocidadActual = velocidadInicial;
		if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
	}
}