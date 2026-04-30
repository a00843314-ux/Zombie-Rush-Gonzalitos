using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ReproductorAleatorio : MonoBehaviour
{
	[Header("Canciones")]
	public AudioClip[] canciones;

	[Header("Configuración")]
	public bool reproducirAlInicio = true;
	public bool repetirLista       = true;
	public bool mezclarLista       = true;
	[Range(0f, 1f)]
	public float volumen           = 1f;

	private AudioSource audioSource;
	private int[]       ordenMezclado;
	private int         indiceActual = 0;

	void Start()
	{
		audioSource        = GetComponent<AudioSource>();
		audioSource.volume = volumen;

		if (canciones == null || canciones.Length == 0)
		{
			Debug.LogWarning("ReproductorAleatorio: No hay canciones asignadas.");
			return;
		}

		GenerarOrden();

		if (reproducirAlInicio)
			StartCoroutine(ReproducirLista());
	}

	void GenerarOrden()
	{
		ordenMezclado = new int[canciones.Length];
		for (int i = 0; i < canciones.Length; i++)
			ordenMezclado[i] = i;

		if (mezclarLista)
		{
			// Fisher-Yates shuffle
			for (int i = ordenMezclado.Length - 1; i > 0; i--)
			{
				int j   = Random.Range(0, i + 1);
				int tmp = ordenMezclado[i];
				ordenMezclado[i] = ordenMezclado[j];
				ordenMezclado[j] = tmp;
			}
		}
	}

	IEnumerator ReproducirLista()
	{
		indiceActual = 0;

		while (true)
		{
			if (indiceActual >= canciones.Length)
			{
				if (!repetirLista) yield break;
				GenerarOrden(); // vuelve a mezclar al reiniciar
				indiceActual = 0;
			}

			AudioClip clip = canciones[ordenMezclado[indiceActual]];

			if (clip != null)
			{
				audioSource.clip = clip;
				audioSource.Play();
				Debug.Log($"Reproduciendo: {clip.name}");
				yield return new WaitForSeconds(clip.length);
			}
			else
			{
				Debug.LogWarning($"Canción en índice {ordenMezclado[indiceActual]} es null, saltando.");
			}

			indiceActual++;
		}
	}

	// Métodos públicos para controlar desde otros scripts o botones UI
	public void Siguiente()
	{
		StopAllCoroutines();
		indiceActual++;
		StartCoroutine(ReproducirLista());
	}

	public void Pausar()  => audioSource.Pause();
	public void Reanudar() => audioSource.UnPause();

	public void CambiarVolumen(float nuevoVolumen)
	{
		volumen            = Mathf.Clamp01(nuevoVolumen);
		audioSource.volume = volumen;
	}
}