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
 
	[Header("Animaciones")]

	public string animRun = "Run";

	public string animPrepararseSalto = "PrepararseSalto";

	public string animJump = "Jump";

	public string animFall = "Fall";

	public string animDisparar = "Disparar";

	public string animRecibirDano = "RecibirDano";

	public string animMuerte = "Muerte";
 
	[Header("Ajustes de animación")]

	public float duracionPrepararseSalto = 0.08f;

	public float transicionAnimacion = 0.05f;
 
	[Header("Puntos por distancia")]

	public TMP_Text textoPuntos;
 
	[Tooltip("Puntos que se ganan por segundo. Como el Player está fijo en X, esto simula la distancia recorrida.")]

	public float puntosPorSegundo = 10f;
 
	private Rigidbody2D rb;

	private Animator anim;
 
	private float xInicial;

	private float contadorCoyote;
 
	private int vidaActual;

	private int contactosSuelo = 0;
 
	private bool enSuelo = false;

	private bool estaMuerto = false;

	private bool estaRecibiendoDano = false;

	private bool estaPreparandoSalto = false;

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

		ReproducirAnimacion(animRun);

	}
 
	void Update()

	{

		if (estaMuerto) return;
 
		ActualizarCoyoteTime();

		LeerSalto();

		SumarPuntosPorDistancia();

		ActualizarAnimacionMovimiento();

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

			StartCoroutine(SecuenciaSalto());

		}

	}
 
	IEnumerator SecuenciaSalto()

	{

		estaPreparandoSalto = true;
 
		ReproducirAnimacion(animPrepararseSalto);
 
		yield return new WaitForSeconds(duracionPrepararseSalto);
 
		rb.linearVelocity = new Vector2(0f, fuerzaSalto);
 
		contadorCoyote = 0f;

		contactosSuelo = 0;

		enSuelo = false;
 
		estaPreparandoSalto = false;
 
		ReproducirAnimacion(animJump);

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
 
	void ActualizarAnimacionMovimiento()

	{

		if (estaMuerto) return;

		if (estaRecibiendoDano) return;

		if (estaPreparandoSalto) return;

		if (estaDisparando) return;
 
		if (enSuelo)

		{

			ReproducirAnimacion(animRun);

		}

		else

		{

			if (rb.linearVelocity.y > 0.05f)

			{

				ReproducirAnimacion(animJump);

			}

			else if (rb.linearVelocity.y < -0.05f)

			{

				ReproducirAnimacion(animFall);

			}

		}

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
 
		StartCoroutine(SecuenciaDisparo());

	}
 
	IEnumerator SecuenciaDisparo()

	{

		estaDisparando = true;
 
		ReproducirAnimacion(animDisparar);
 
		yield return new WaitForSeconds(0.18f);
 
		estaDisparando = false;

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
 
		ReproducirAnimacion(animRecibirDano);
 
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
 
		ReproducirAnimacion(animMuerte);
 
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
 
	void ReproducirAnimacion(string nombreAnimacion)

	{

		if (anim == null) return;

		if (string.IsNullOrEmpty(nombreAnimacion)) return;
 
		int hash = Animator.StringToHash(nombreAnimacion);
 
		if (anim.HasState(0, hash))

		{

			anim.CrossFade(hash, transicionAnimacion);

		}

	}

}
 