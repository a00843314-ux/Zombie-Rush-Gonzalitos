using UnityEngine;

public class Vida : MonoBehaviour
{
    public int vidaMaxima  = 100;
    public int vidaActual;

    void Start() => vidaActual = vidaMaxima;

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log($"{gameObject.name} recibió {cantidad} de daño. Vida: {vidaActual}/{vidaMaxima}");

        if (vidaActual <= 0)
            Morir();
    }

    void Morir()
    {
        Debug.Log($"{gameObject.name} murió.");
        Destroy(gameObject);
    }
}
