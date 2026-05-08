using UnityEngine;

public class TriggerDialogoPuerta : MonoBehaviour
{
    [Header("Personaje Interceptor")]
    public DialogosCindy npcInterceptora;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && npcInterceptora != null)
        {
            IInterceptor interceptor = npcInterceptora.GetComponent<IInterceptor>();

            if(interceptor != null)
            {
                interceptor.InterceptPlayer(other.transform);
            }
        }
    }
}
