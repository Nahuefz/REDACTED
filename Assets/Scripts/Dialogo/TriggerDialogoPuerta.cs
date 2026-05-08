using UnityEngine;

public class TriggerDialogoPuerta : MonoBehaviour
{
    [Header("Personaje Interceptor")]
    public DialogosCindy npcInterceptora;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcInterceptora.InterceptarJugador(other.transform);
        }
    }
}
