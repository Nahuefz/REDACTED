using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private ActorDialogo[] actoresActuales;
    private PlayerAlignment playerAlignmentCache;

    [Header("Componentes UI")] public GameObject panelDialogo;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDialogo;
    public GameObject iconoContinuar;

    [Header("Nota")] public GameObject notaEnCamara;

    private Queue<LineaDeDialogo> oraciones;
    private bool talking = false;
    private bool looking = false;

    [SerializeField] Image interactButtonImage;
    private Inventory _currentInventory;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        oraciones = new Queue<LineaDeDialogo>();
        panelDialogo.SetActive(false);

        if (notaEnCamara != null) notaEnCamara.SetActive(false);
    }

    private void Update()
    {
        if (talking && Mouse.current.leftButton.wasPressedThisFrame) MostrarSiguienteOracion();
    }

    public void EmpezarDialogo(DialogoData dialogo, Inventory inventory, ActorDialogo[] actoresEscena = null)
    {
        _currentInventory = inventory;
        talking = true;
        panelDialogo.SetActive(true);

        if (interactButtonImage != null) interactButtonImage.enabled = false;

        actoresActuales = actoresEscena;
        if (playerAlignmentCache == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerAlignmentCache = p.GetComponent<PlayerAlignment>();
        }

        oraciones.Clear();

        foreach (LineaDeDialogo linea in dialogo.dialogos)
        {
            LineaDeDialogo lineaProcesada = linea;

            if (string.IsNullOrWhiteSpace(lineaProcesada.nombrePersonaje))
            {
                lineaProcesada.nombrePersonaje = dialogo.nombrePorDefecto;
            }

            oraciones.Enqueue(lineaProcesada);
        }

        MostrarSiguienteOracion();
    }

    public void MostrarSiguienteOracion()
    {
        if (oraciones.Count == 0)
        {
            TerminarDialogo();
            return;
        }

        LineaDeDialogo oracionActual = oraciones.Dequeue();

        textoNombre.text = oracionActual.nombrePersonaje;
        textoDialogo.text = oracionActual.texto;

        // Logica de alineacion de camara (del merge)
        if (actoresActuales != null && playerAlignmentCache != null)
        {
            foreach (ActorDialogo actor in actoresActuales)
            {
                if (actor.nombreEnElScript == oracionActual.nombrePersonaje && actor.anclaMirada != null)
                {
                    playerAlignmentCache.CambiarMirada(actor.anclaMirada, actor.hacerZoom, actor.nivelDeZoom);
                    break;
                }
            }
        }

        // Logica de la nota (tuya)
        if (oracionActual.imagen != null)
        {
            if (notaEnCamara != null)
            {
                Image image = notaEnCamara.GetComponent<Image>();
                if (image != null) image.sprite = oracionActual.imagen;
                
                notaEnCamara.SetActive(true);
                
                if (notaEnCamara.TryGetComponent(out BasicItem item) && _currentInventory != null)
                {
                    _currentInventory.TryAddItem(item.data);
                }
            }
        }
        else
        {
            if (notaEnCamara != null) notaEnCamara.SetActive(false);
        }

        iconoContinuar.SetActive(oraciones.Count > 0);
    }

    public void TerminarDialogo()
    {
        talking = false;
        _currentInventory = null;
        panelDialogo.SetActive(false);

        if (notaEnCamara != null) notaEnCamara.SetActive(false);
        if (interactButtonImage != null) interactButtonImage.enabled = looking;
        if (playerAlignmentCache != null) playerAlignmentCache.RestaurarFOV();
    }

    public bool EstaHablando()
    {
        return talking;
    }

    void InteractableSeen(bool isSeen)
    {
        looking = isSeen;
        if (interactButtonImage != null) interactButtonImage.enabled = looking && !talking;
    }

    private void OnEnable()
    {
        InteractRay.OnInteractSeen += InteractableSeen;
    }

    private void OnDisable()
    {
        InteractRay.OnInteractSeen -= InteractableSeen;
    }
}
