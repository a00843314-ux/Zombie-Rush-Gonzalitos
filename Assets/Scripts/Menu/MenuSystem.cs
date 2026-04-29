using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
	public void Jugar()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void IrATienda()
	{
		SceneManager.LoadScene("Tienda");
	}

	public void IrAMenu()
	{
		SceneManager.LoadScene("Menu"); // cambia por el nombre exacto de tu escena de menú
	}

	public void Salir()
	{
		Debug.Log("Saliendo del Juego...");
		Application.Quit();
	}
}
