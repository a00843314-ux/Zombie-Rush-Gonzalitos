using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorDeNivel : MonoBehaviour
{
	[SerializeField] private GameObject[] partesNivel;
	
	[SerializeField] private float distanciaMinima; 
	
	[SerializeField] private Transform puntoFinal;
	
	[SerializeField] private int cantidadInicial; 
	
	private Transform Jugador; 
	
	private void start() {
		Jugador = GameObject.FindGameObjectWithTag("player").transform;	
	}

	private void Update() {
	 if(Vector2.Distance(Jugador.position, puntoFinal.position)<distanciaMinima){
	 	//GenerarParteNivel();
	}
	}
	private void GenerarParteNivel()