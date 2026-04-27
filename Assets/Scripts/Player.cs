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
	public float   fuerzaSalto  = 10f;
	public float   tiempoCoyote = 0.15f;
	public KeyCode teclaSalto   = KeyCode.Space;

	[Header("Disparo")]
	public GameObject prefabBala;
	public Transform  puntoDeDisparo;
	public float      cooldownDisparo = 0.3f;
	public KeyCode    teclaDisparo    = KeyCode.E;

	[Header("Daño")]
	public float fuerzaKnockback  = 8f;
	public float tiempoInvencible = 3f;
	public float tiempoSinHitbox  = 2f;

	private Rigidbody2D rb;
	private Animator    anim;
	private Collider2D  colSuelo;
	private Collider2D  colDano;

	private float velocidadActual;
	private bool  enSuelo           = false;
	private float timerDisparo      = 0f;
	private bool  invencible        = false;
	private float timerInvencible   = 0f;
	private bool  muerto            = false;
	private float timerCoyote       = 0f;
	private bool  puedeSaltarCoyote = false;

	private static readonly int HashGrounded    = Animator.StringToHash("isGrounded");
	private static readonly int HashVelocityY   = Animator.StringToHash("velocityY");
	private static readonly int HashIsShooting  = Animator.StringToHash("isShooting");
	private static readonly int HashRecibirDano = Animator.StringToHash("RecibirDano");
	private static readonly int HashMuerte      = Animator.StringToHash("Muerte");

	void Start()
	{
		rb   = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		Collider2D[] cols = GetComponents<Collider2D>();
		if (cols.Length >= 2)
		{
			colSuelo = cols[0];
			colDano  = cols[1];
		}
		else
		{
			Debug.LogWarning("El jugador necesita dos Collider2D — uno para suelo y uno para daño.");
			colSuelo = cols[0];
			colDano  = cols[0];
		}

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

		if (enSuelo)
		{
			puedeSaltarCoyote = true;
			timerCoyote       = tiempoCoyote;
		}
		else
		{
			timerCoyote -= Time.deltaTime;
			if (timerCoyote <= 0f)
				puedeSaltarCoyote = false;
		}

		if (Input.GetKeyDown(teclaSalto) && puedeSaltarCoyote)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
			rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
			puedeSaltarCoyote = false;
			timerCoyote       = 0f;
			enSuelo           = false;
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
		Debug.Log($"RecibirGolpe llamado | invencible: {invencible} | muerto: {muerto}");
		if (invencible || muerto) return;
		Debug.Log("Activando animacion RecibirDano");

		anim.SetTrigger(HashRecibirDano);

		rb.linearVelocity = Vector2.zero;
		rb.AddForce(Vector2.left * fuerzaKnockback, ForceMode2D.Impulse);

		invencible      = true;
		timerInvencible = tiempoInvencible;

		colDano.enabled = false;
		Invoke(nameof(ReactivarHitbox), tiempoSinHitbox);
	}

	void ReactivarHitbox()
	{
		if (!muerto)
			colDano.enabled = true;
	}

	public void Morir()
	{
		if (muerto) return;

		muerto            = true;
		rb.linearVelocity = Vector2.zero;
		rb.bodyType       = RigidbodyType2D.Kinematic;
		colSuelo.enabled  = false;
		colDano.enabled   = false;

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
		Debug.Log($"Colision con: {collision.gameObject.tag} | colSuelo activo: {colSuelo.enabled} | colDano activo: {colDano.enabled}");

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

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log($"Trigger con: {other.gameObject.tag}");
		if (other.CompareTag("RecibirDano"))
			RecibirGolpe();
	}

	public Vector2 GetDirection()
	{
		return Vector2.right;
	}

	void OnEnable()
	{
		muerto            = false;
		invencible        = false;
		timerInvencible   = 0f;
		velocidadActual   = velocidadInicial;
		puedeSaltarCoyote = false;
		timerCoyote       = 0f;
		if (rb       != null) rb.bodyType      = RigidbodyType2D.Dynamic;
		if (colSuelo != null) colSuelo.enabled = true;
		if (colDano  != null) colDano.enabled  = true;
	}
}