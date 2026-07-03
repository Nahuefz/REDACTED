using UnityEngine;
using System.Collections.Generic; // REQUERIDO PARA USAR LISTAS

public class OficinistaNPC : MonoBehaviour, IInteractable
{
    [Header("Datos de Dialogo (Normal)")]
    [Tooltip("Si no hay un objeto requerido, elegirá uno al azar de aquí.")]
    public DialogoData[] listaDialogos;

    [Header("Misión de Objetos (Múltiples)")]
    [Tooltip("Lista de objetos que el NPC va a pedir al jugador.")]
    public List<ItemData> objetosRequeridos = new List<ItemData>();
    
    [Tooltip("¿Requiere un tipo específico? (Si se usa esto, ignorará la lista de objetos individuales de arriba)")]
    public ItemType tipoRequerido = ItemType.Default;

    [Header("Recompensas de Misión")]
    [Tooltip("Objetos que el NPC le dará al jugador al completar la misión.")]
    public List<ItemData> objetosDeRecompensa = new List<ItemData>();
    
    [Space(5)]
    public DialogoData dialogoNegativo;      // Si no tiene el objeto
    public DialogoData dialogoAgradecimiento; // Justo al entregarlo
    public DialogoData dialogoYaCompletado;   // Cuando hablas después de entregar
    public GameObject objetoADesbloquear;     // Por ejemplo, una puerta o bloqueo
    public bool introduccionHecha = false;    // ¿Ya dio su diálogo inicial?
    public bool yaEntregado = false;

    [Header("Estado Lógico")]
    public bool interaccionHabilitada = true;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] sonidosInteraccion;

    [Header("Ancla Cinematografica")]
    public Transform anclaDeInteraccion;
    public Transform anclaDeMirada;

    [Header("Director de Camaras (Opcional)")]
    [Tooltip("Si este dialogo involucra a otros personajes, agregarlos aca.")]
    public ActorDialogo[] actoresEnEscena;

    private bool seEstaAlineando = false;
    private Inventory _cachedInventory;

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    public void Interact(GameObject interactor)
    {
        if (!interaccionHabilitada) return;

        if (DialogueManager.Instance.EstaHablando())
        {
            DialogueManager.Instance.MostrarSiguienteOracion();
            return;
        }
        
        _cachedInventory = interactor.GetComponent<Inventory>();

        if (seEstaAlineando) return;

        // Proceso de alineación y cámara
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && anclaDeInteraccion != null)
        {
            PlayerAlignment alignment = player.GetComponent<PlayerAlignment>();
            if (alignment != null)
            {
                seEstaAlineando = true;
                alignment.Alinear(anclaDeInteraccion, transform, anclaDeMirada, () =>
                {
                    seEstaAlineando = false;
                    ProcesarLogicaDeDialogo();
                });
            }
        }
        else
        {
            ProcesarLogicaDeDialogo();
        }
    }

    private void ProcesarLogicaDeDialogo()
    {
        ReproducirSonidoRandom();

        // Es misión si pide un tipo general o si la lista de objetos tiene elementos
        bool esMision = (tipoRequerido != ItemType.Default || (objetosRequeridos != null && objetosRequeridos.Count > 0));

        // 1. Si es de misión y NO ha dado la introducción, la damos primero.
        if (esMision && !introduccionHecha && listaDialogos != null && listaDialogos.Length > 0)
        {
            DialogueManager.Instance.EmpezarDialogo(listaDialogos[0], _cachedInventory, actoresEscena: actoresEnEscena);
            introduccionHecha = true;
            return;
        }

        // 2. ¿Es un NPC de misión?
        if (esMision)
        {
            LogicaDeMision();
        }
        // 3. Si no, es un NPC normal con diálogos aleatorios
        else if (listaDialogos != null && listaDialogos.Length > 0)
        {
            int indiceDialogo = Random.Range(0, listaDialogos.Length);
            DialogueManager.Instance.EmpezarDialogo(listaDialogos[indiceDialogo], _cachedInventory, actoresEnEscena);
        }
        else
        {
            Debug.LogWarning("El NPC " + gameObject.name + " no tiene diálogos asignados!");
        }
    }

    private void LogicaDeMision()
    {
        if (yaEntregado)
        {
            LanzarDialogo(dialogoYaCompletado);
            return;
        }

        // Buscamos si el jugador cumple los requisitos
        List<ItemData> itemsAEntregar = BuscarItemsRequeridos();

        // Si la lista no es nula significa que el jugador TIENE TODO lo necesario
        if (itemsAEntregar != null && itemsAEntregar.Count > 0)
        {
            yaEntregado = true;

            // 1. Quitar los objetos requeridos del inventario
            foreach (var item in itemsAEntregar)
            {
                _cachedInventory.RemoveItem(item);
                Debug.Log($"NPC {gameObject.name} recibió: {item.itemName}");
            }
            
            // 2. DAR RECOMPENSAS AL JUGADOR
            if (objetosDeRecompensa != null && objetosDeRecompensa.Count > 0)
            {
                foreach (var recompensa in objetosDeRecompensa)
                {
                    if (recompensa != null)
                    {
                        // IMPORTANTE: Asegúrate de que tu script Inventory tenga un método "AddItem" o similar.
                        // Si tu método se llama distinto (ej. RecibirItem), cambia esta línea:
                        _cachedInventory.TryAddItem(recompensa); 
                        Debug.Log($"Jugador recibió recompensa: {recompensa.itemName}");
                    }
                }
            }

            if (objetoADesbloquear != null) 
                objetoADesbloquear.SetActive(false);

            LanzarDialogo(dialogoAgradecimiento);
        }
        else
        {
            // No tiene todos los objetos aún
            LanzarDialogo(dialogoNegativo);
        }
    }

    // Devuelve la lista de items exactos a remover si se cumple la condición completa, o null si falta algo
    private List<ItemData> BuscarItemsRequeridos()
    {
        if (_cachedInventory == null) return null;

        List<ItemData> encontrados = new List<ItemData>();

        // Caso de Misión por Tipo General (Ej: Cualquier llave)
        if (tipoRequerido != ItemType.Default)
        {
            foreach (var item in _cachedInventory.GetInventory)
            {
                if (item != null && item.itemType == tipoRequerido)
                {
                    encontrados.Add(item);
                    return encontrados; // Con uno que coincida basta en este modo anterior
                }
            }
            return null; 
        }

        // Caso de Misión por Objetos Específicos (Ej: Un Queso Y Un Café)
        // Clonamos la lista del inventario para hacer una simulación de descarte y no bugearse con duplicados
        List<ItemData> copiaInventario = new List<ItemData>(_cachedInventory.GetInventory);

        foreach (var req in objetosRequeridos)
        {
            if (req == null) continue;

            // Buscamos si el requerimiento está en la copia del inventario
            if (copiaInventario.Contains(req))
            {
                encontrados.Add(req);
                copiaInventario.Remove(req); // Lo removemos de la copia temporal para evitar falsos positivos si pide 2 del mismo ítem
            }
            else
            {
                // Si tan solo uno de los objetos requeridos NO está en el inventario, la misión no se puede completar
                return null; 
            }
        }

        return encontrados;
    }

    private void LanzarDialogo(DialogoData data)
    {
        if (data != null)
        {
            DialogueManager.Instance.EmpezarDialogo(data, _cachedInventory, actoresEnEscena);
        }
        else
        {
            Debug.LogWarning($"Falta asignar un diálogo en el estado actual de {gameObject.name}");
        }
    }

    private void ReproducirSonidoRandom()
    {
        if (audioSource != null && sonidosInteraccion != null && sonidosInteraccion.Length > 0)
        {
            int indice = Random.Range(0, sonidosInteraccion.Length);
            audioSource.clip = sonidosInteraccion[indice];
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Debug.Log("Jugador cerca de " + gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) Debug.Log("Jugador se alejó de " + gameObject.name);
    }
}