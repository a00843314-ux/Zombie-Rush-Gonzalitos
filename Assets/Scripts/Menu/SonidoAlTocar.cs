using UnityEngine;

public class SonidoAlTocar : MonoBehaviour
{
	[Header("Audio")]
	public AudioClip sonido;

	private AudioSource audioSource;

	void Start()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
	}

	void OnMouseDown()
	{
		Debug.Log("Click detectado en objeto");
		if (sonido != null)
			audioSource.PlayOneShot(sonido);
	}
}