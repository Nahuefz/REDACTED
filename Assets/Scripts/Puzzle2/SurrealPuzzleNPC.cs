using UnityEngine;

public class SurrealPuzzleNPC : MonoBehaviour, IInteractable
{
    [Header("Identidad")]
    public int miID;

    [Header("Dialogos Dinamicos")]
    public DialogoData[] dialogosPorEscenario;

    [Header("Punto del Hueco")]
    public Transform puntoHueco;

    public void Interact(GameObject player)
    {
        int objetivoActual = PuzzleManager.Instance.ObjetivoID;

        if (objetivoActual >= dialogosPorEscenario.Length || dialogosPorEscenario[objetivoActual] == null)
        {
            Debug.LogWarning("Falta asignar el dialogo para el escenario " + objetivoActual + " en el NPC " + miID);
            return;
        }

        DialogoData dialogoAEjecutar = dialogosPorEscenario[objetivoActual];
        Inventory inventarioJugador = player.GetComponent<Inventory>();
        DialogueManager.Instance.EmpezarDialogo(dialogoAEjecutar, inventarioJugador);
        Debug.Log("Iniciando dialogo del escenario: " + objetivoActual);
    }

    public void RecibirAtaqueMortal()
    {
        Debug.Log("El NPC " + miID + " ha sido acusado.");
        Vector3 posicionSuelo = puntoHueco != null ? puntoHueco.position : transform.position;
        PuzzleManager.Instance.IntentarAsesinato(miID, this.gameObject, posicionSuelo);
    }
}
