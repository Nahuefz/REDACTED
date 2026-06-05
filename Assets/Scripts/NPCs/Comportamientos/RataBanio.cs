using System.Collections; // <--- ESTO ES LO QUE TE FALTA
using UnityEngine;

public class ComportamientoRata : MonoBehaviour, IInteractable
{
    [SerializeField] private BathroomManager manager;
    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Interact(GameObject interactor)
    {
        if (manager != null)
        {
            Debug.Log("La rata detectó el click y llamó al Manager.");
            manager.TriggerEventoRataBanio();
        }
        else
        {
            Debug.LogError("¡ERROR: El script de la Rata no tiene asignado el BathroomManager en el Inspector!");
        }
    }
    public void IniciarFadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        // Deshabilitamos el collider para que no sea más interactuable
        GetComponent<Collider>().enabled = false;

        float duration = 1.0f; // Duración del fade en segundos
        float elapsed = 0f;
        Color startColor = sprite.color;

        // Lógica del fade
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            sprite.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null; // Espera al siguiente frame
        }

        // Opcional: desactivar el objeto al terminar
        gameObject.SetActive(false);
    }
}