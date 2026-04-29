using UnityEngine;

public class EnemigoChico : MonoBehaviour
{
	public float velocity = 2f;
	public float distanciaVision =  5f;
	public Transform player;
	public Transform puntoSuelo;
	public float distanciaSuelo = 0.5f;
	public LayerMask capaSuelo;
	
	private Rigidbody2D	rb;
	private SpriteRenderer spriteRenderer;
	private int direccion =1;
	private float tiempoEspera = 0f;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	    rb = GetComponent<Rigidbody2D>();
	    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
	void FixedUpdate()
    {
	    if (puntoSuelo == null) return;
	    tiempoEspera -= Time.fixedDeltaTime;
	    bool haySuelo = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);
	    
	    //Sin suelo adelante: voltear (solamente si no acaba de voltear)
	    if (haySuelo && tiempoEspera <= 0f)
	    {
	    	Voltear();
	    	// Esperar antes de voltear otra vez
	    	tiempoEspera = 0.3f;
	    }
	    
	    //Si el player está cerca, lo persigue
	    if(player !=null && Vector2.Distance(transform.position, player.position) <= distanciaVision)
	    {
	    	int	nuevaDir = player.position.x > transform.position.x ? 1: -1;
	    	if (nuevaDir != direccion) Voltear();
	    }
	    //Siempre se mueva no se detenga al voltear
	    {
	    	rb.linearVelocity = new Vector2(velocity*direccion, rb.linearVelocity.y);
	    }
    }
    
	void Voltear()
	{
		direccion *= -1;
		spriteRenderer.flipX = direccion < 0;
		
		Vector3 pos = puntoSuelo.localPosition;
		pos.x = Mathf.Abs(pos.x) * direccion;
		puntoSuelo.localPosition = pos;
	}
	public void Morir()
	{
		Destroy(gameObject);
	}
	
	void OnDrawGizmosSelected()
	{
		if (puntoSuelo != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(puntoSuelo.position, puntoSuelo.position + Vector3.down * distanciaSuelo);
		}
		
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, distanciaVision);
	}
}
