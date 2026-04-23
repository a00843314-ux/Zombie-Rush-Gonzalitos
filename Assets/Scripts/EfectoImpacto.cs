using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EfectoImpacto : MonoBehaviour
{
    // Nombre exacto del estado final en tu Animator
    private const string ESTADO_FINAL = "Impacto";

    void Start()
    {
        Animator anim = GetComponent<Animator>();

        // Calcula la duración del clip actual y destruye al terminar
        AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
        if (clips.Length > 0)
        {
            float duracion = clips[0].clip.length;
            Destroy(gameObject, duracion);
        }
        else
        {
            Destroy(gameObject, 0.5f); // fallback por si no hay clip listo en Start
        }
    }
}
