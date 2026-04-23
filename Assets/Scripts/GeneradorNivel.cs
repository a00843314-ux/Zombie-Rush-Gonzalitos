using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorNivel : MonoBehaviour
{
	[SerializeField] private GameObject[] partesNivel;
	[SerializeField] private float distanciaMinima;
	[SerializeField] private Transform puntoFinal;
	[SerializeField] private int cantidadInicial;

	private Transform jugador;
	private bool generando = false;

	private void Start()
	{
		jugador = GameObject.FindGameObjectWithTag("Player").transform;

		for (int i = 0; i < cantidadInicial; i++)
		{
			GenerarParteNivel();
		}
	}

	private void Update()
	{
		if (generando) return;

		if ((jugador.position - puntoFinal.position).sqrMagnitude < distanciaMinima * distanciaMinima)
		{
			generando = true;
			GenerarParteNivel();
			generando = false;
		}
	}

	private void GenerarParteNivel()
	{
		int numeroAleatorio = Random.Range(0, partesNivel.Length);
		GameObject nivel = Instantiate(partesNivel[numeroAleatorio], puntoFinal.position, Quaternion.identity);

		Transform nuevoPunto = BuscarPuntoFinal(nivel, "PuntoFinal");

		if (nuevoPunto != null)
			puntoFinal = nuevoPunto;
		else
			Debug.LogWarning($"El prefab '{nivel.name}' no tiene un objeto con tag 'PuntoFinal' en ningún nivel.");
	}

	private Transform BuscarPuntoFinal(GameObject parteNivel, string etiqueta)
	{
		foreach (Transform ubicacion in parteNivel.GetComponentsInChildren<Transform>())
		{
			if (ubicacion.CompareTag(etiqueta))
				return ubicacion;
		}

		Debug.LogWarning($"El prefab '{parteNivel.name}' no tiene un objeto con tag '{etiqueta}' en ningún nivel.");
		return null;
	}
}