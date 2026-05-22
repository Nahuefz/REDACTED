using UnityEngine;

public class OficinistaNPC : MonoBehaviour, IInteractable
{
    [Header("Datos de Dialogo (Normal)")]
    [Tooltip("Si no hay un objeto requerido, elegirá uno al azar de aquí.")]
    public DialogoData[] listaDialogos;

    [Header("Misión de Objeto (Opcional)")]
    public ItemData objetoRequerido;
    public ItemType tipoRequerido = ItemType.Default;
    public DialogoData dialogoPeticion;      // Si no tiene el objeto
    public DialogoData dialogoAgradecimiento; // Justo al entregarlo
    public DialogoData dialogoYaCompletado;   // Cuando hablas después de entregar
    public GameObject objetoADesbloquear;     // Por ejemplo, una puerta o bloqueo
    public bool yaEntregado = false;

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

        // 1. ¿Es un NPC de misión?
        if (objetoRequerido != null || tipoRequerido != ItemType.Default)
        {
            LogicaDeMision();
        }
        // 2. Si no, es un NPC normal con diálogos aleatorios
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

        ItemData itemEncontrado = BuscarItemRequerido();

        if (itemEncontrado != null)
        {
            // Entregar el objeto
            yaEntregado = true;
            _cachedInventory.RemoveItem(itemEncontrado);
            
            if (objetoADesbloquear != null) 
                objetoADesbloquear.SetActive(false);

            LanzarDialogo(dialogoAgradecimiento);
            Debug.Log($"NPC {gameObject.name} recibió: {itemEncontrado.itemName}");
        }
        else
        {
            // No tiene el objeto aún
            LanzarDialogo(dialogoPeticion);
        }
    }

    private ItemData BuscarItemRequerido()
    {
        if (_cachedInventory == null) return null;

        foreach (var item in _cachedInventory.GetInventory)
        {
            if (item == null) continue;

            if (objetoRequerido != null)
            {
                if (item == objetoRequerido) return item;
            }
            else if (tipoRequerido != ItemType.Default && item.itemType == tipoRequerido)
            {
                return item;
            }
        }
        return null;
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
