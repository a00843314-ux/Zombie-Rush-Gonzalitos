using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]

public class Player : MonoBehaviour
{
	[Header("Salto")]
	public float fuerzaSalto = 10f;
	public KeyCode teclaSalto = KeyCode.Space;
	public float tiempoCoyote = 0.15f;

	[Header("Posición fija del Player")]
	public bool mantenerXInicial = true;

	[Header("Dirección de disparo")]
	public Vector2 direccionDisparo = Vector2.right;

	[Header("Vida y daño")]
	public int vidaMaxima = 3;
	public float tiempoInvulnerable = 1.5f;
	public float fuerzaKnockback = 5f;

	[Header("Tags que hacen daño")]
	public string tagEnemigo = "Enemy";
	public string tagObstaculo = "Obstaculo";
	public string tagProyectilEnemigo = "EnemyBullet";

	[Header("Game Over")]
	public string nombreEscenaGameOver = "GameOver";
	public float tiempoAntesDeGameOver = 1.2f;

	[Header("Puntos por distancia")]
	public TMP_Text textoPuntos;

	[Tooltip("Puntos que se ganan por segundo.")]
	public float puntosPorSegundo = 10f;

	// Hashes del Animator (igual que tu código antiguo)
	private static readonly int HashGrounded    = Animator.StringToHash("isGrounded");
	private static readonly int HashVelocityY   = Animator.StringToHash("velocityY");
	private static readonly int HashIsShooting  = Animator.StringToHash("isShooting");
	private static readonly int HashRecibirDano = Animator.StringToHash("RecibirDano");
	private static readonly int HashMuerte      = Animator.StringToHash("Muerte");

	private Rigidbody2D rb;
	private Animator anim;

	private float xInicial;
	private float contadorCoyote;

	private int vidaActual;
	private int contactosSuelo = 0;

	private bool enSuelo = false;
	private bool estaMuerto = false;
	private bool estaRecibiendoDano = false;
	private bool estaDisparando = false;
	private bool esInvulnerable = false;

	private float puntosDistancia = 0f;
	private int puntosTotales = 0;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		xInicial = transform.position.x;
		direccionDisparo = Vector2.right;

		vidaActual = vidaMaxima;

		ActualizarTextoPuntos();
	}

	void Update()
	{
		if (estaMuerto) return;

		ActualizarCoyoteTime();
		LeerSalto();
		SumarPuntosPorDistancia();

		// Actualiza animaciones con bools y floats como tu código antiguo
		anim.SetBool(HashGrounded, enSuelo);
		anim.SetFloat(HashVelocityY, rb.linearVelocity.y);
	}

	void FixedUpdate()
	{
		if (estaMuerto) return;

		MantenerPosicionX();
	}

	void ActualizarCoyoteTime()
	{
		enSuelo = contactosSuelo > 0;

		if (enSuelo)
		{
			contadorCoyote = tiempoCoyote;
		}
		else
		{
			contadorCoyote -= Time.deltaTime;
		}
	}

	void LeerSalto()
	{
		if (Input.GetKeyDown(teclaSalto) && contadorCoyote > 0f)
		{
			rb.linearVelocity = new Vector2(0f, fuerzaSalto);
			contadorCoyote = 0f;
			contactosSuelo = 0;
			enSuelo = false;
		}
	}

	void MantenerPosicionX()
	{
		if (!mantenerXInicial) return;

		transform.position = new Vector3(
			xInicial,
			transform.position.y,
			transform.position.z
		);

		rb.linearVelocity = new Vector2(
			0f,
			rb.linearVelocity.y
		);
	}

	void SumarPuntosPorDistancia()
	{
		puntosDistancia += puntosPorSegundo * Time.deltaTime;

		int nuevosPuntosTotales = Mathf.FloorToInt(puntosDistancia);

		if (nuevosPuntosTotales != puntosTotales)
		{
			puntosTotales = nuevosPuntosTotales;
			ActualizarTextoPuntos();
		}
	}

	void ActualizarTextoPuntos()
	{
		if (textoPuntos != null)
		{
			textoPuntos.text = "Metros: " + puntosTotales.ToString();
		}
	}

	public Vector2 GetDirection()
	{
		return direccionDisparo.normalized;
	}

	public void AnimarDisparo()
	{
		if (estaMuerto) return;
		anim.SetTrigger(HashIsShooting);
	}

	public void RecibirDano(int cantidadDano)
	{
		if (estaMuerto) return;
		if (esInvulnerable) return;

		vidaActual -= cantidadDano;

		if (vidaActual <= 0)
		{
			Morir();
		}
		else
		{
			StartCoroutine(SecuenciaRecibirDano());
		}
	}

	IEnumerator SecuenciaRecibirDano()
	{
		esInvulnerable = true;
		estaRecibiendoDano = true;

		anim.SetTrigger(HashRecibirDano);

		rb.linearVelocity = new Vector2(
			-fuerzaKnockback,
			rb.linearVelocity.y
		);

		yield return new WaitForSeconds(0.35f);

		estaRecibiendoDano = false;

		yield return new WaitForSeconds(tiempoInvulnerable);

		esInvulnerable = false;
	}

	void Morir()
	{
		if (estaMuerto) return;

		estaMuerto = true;

		rb.linearVelocity = Vector2.zero;
		rb.bodyType = RigidbodyType2D.Kinematic;

		anim.SetTrigger(HashMuerte);

		StartCoroutine(IrAGameOver());
	}

	IEnumerator IrAGameOver()
	{
		yield return new WaitForSeconds(tiempoAntesDeGameOver);

		SceneManager.LoadScene(nombreEscenaGameOver);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (EsSueloValido(collision))
		{
			contactosSuelo++;
		}

		if (HaceDano(collision.gameObject))
		{
			RecibirDano(1);
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (EsSueloValido(collision))
		{
			contactosSuelo = Mathf.Max(contactosSuelo, 1);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (EsSueloValido(collision))
		{
			contactosSuelo--;
			contactosSuelo = Mathf.Max(contactosSuelo, 0);
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (HaceDano(collision.gameObject))
		{
			RecibirDano(1);
		}
	}

	bool EsSueloValido(Collision2D collision)
	{
		foreach (ContactPoint2D contacto in collision.contacts)
		{
			if (contacto.normal.y > 0.5f)
			{
				return true;
			}
		}

		return false;
	}

	bool HaceDano(GameObject objeto)
	{
		string tagObjeto = objeto.tag;

		if (tagObjeto == tagEnemigo) return true;
		if (tagObjeto == tagObstaculo) return true;
		if (tagObjeto == tagProyectilEnemigo) return true;

		return false;
	}
}