using System.Collections;
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

    [Header("Componentes UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDialogo;
    public GameObject iconoContinuar;

    [Header("Nota")]
    public GameObject notaEnCamara;

    [Header("Cinematica y Maquina de Escribir")]
    public bool modoAutomatico = false;
    public bool usarEfectoMaquina = true;
    public float tiempoEntreLetras = 0.045f;

    [Header("Sonidos tipo Animal Crossing")]
    public AudioSource audioSourceVoces;
    public AudioClip[] sonidosLetras;
    [Tooltip("Cada cuantas letras suena un blip (2 o 3 suele quedar bien).")]
    public int sonarCadaCuantasLetras = 2;
    [Range(0.5f, 1.5f)] public float pitchMin = 0.9f;
    [Range(0.5f, 1.5f)] public float pitchMax = 1.15f;

    private Queue<LineaDeDialogo> oraciones;
    private Coroutine _rutinaEscritura;
    private bool talking = false;
    private bool looking = false;

    public bool EstaEscribiendo { get; private set; }

    [SerializeField] Image interactButtonImage;
    private Inventory _currentInventory;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        oraciones = new Queue<LineaDeDialogo>();
        if (panelDialogo != null) panelDialogo.SetActive(false);
        if (notaEnCamara != null) notaEnCamara.SetActive(false);
    }

    private void Update()
    {
        // En modo automatico no avanza con click
        if (talking && !modoAutomatico && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            MostrarSiguienteOracion();
        }
    }

    public void EmpezarDialogo(DialogoData dialogo, Inventory inventory = null, ActorDialogo[] actoresEscena = null)
    {
        _currentInventory = inventory;
        talking = true;
        if (panelDialogo != null) panelDialogo.SetActive(true);
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

        if (textoNombre != null) textoNombre.text = oracionActual.nombrePersonaje;

        // Arrancamos escritura (con o sin efecto maquina)
        if (_rutinaEscritura != null) StopCoroutine(_rutinaEscritura);

        if (usarEfectoMaquina)
        {
            _rutinaEscritura = StartCoroutine(EscribirLetraPorLetra(oracionActual.texto));
        }
        else
        {
            textoDialogo.text = oracionActual.texto;
            textoDialogo.maxVisibleCharacters = oracionActual.texto.Length;
            EstaEscribiendo = false;
        }

        // Logica de alineacion de camara (solo corre si hay Player y actores)
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

        // Logica de la nota
        if (oracionActual.imagen != null)
        {
            if (notaEnCamara != null)
            {
                SpriteRenderer image = notaEnCamara.GetComponentInChildren<SpriteRenderer>();
                if (image != null) image.sprite = oracionActual.imagen;
                notaEnCamara.SetActive(true);
            }
        }
        else
        {
            if (notaEnCamara != null) notaEnCamara.SetActive(false);
        }

        if (iconoContinuar != null) iconoContinuar.SetActive(!modoAutomatico && oraciones.Count > 0);
    }

    private IEnumerator EscribirLetraPorLetra(string textoCompleto)
    {
        EstaEscribiendo = true;
        textoDialogo.text = textoCompleto;
        textoDialogo.maxVisibleCharacters = 0;

        int contadorSonido = 0;

        for (int i = 0; i < textoCompleto.Length; i++)
        {
            textoDialogo.maxVisibleCharacters = i + 1;

            char letra = textoCompleto[i];
            if (!char.IsWhiteSpace(letra))
            {
                if (contadorSonido % Mathf.Max(1, sonarCadaCuantasLetras) == 0)
                {
                    ReproducirSonidoLetra();
                }
                contadorSonido++;
            }

            yield return new WaitForSeconds(tiempoEntreLetras);
        }

        EstaEscribiendo = false;
    }

    private void ReproducirSonidoLetra()
    {
        if (audioSourceVoces == null || sonidosLetras == null || sonidosLetras.Length == 0) return;

        AudioClip clip = sonidosLetras[Random.Range(0, sonidosLetras.Length)];
        if (clip == null) return;

        audioSourceVoces.pitch = Random.Range(pitchMin, pitchMax);
        audioSourceVoces.PlayOneShot(clip);
    }

    public void TerminarDialogo()
    {
        if (_rutinaEscritura != null) StopCoroutine(_rutinaEscritura);
        EstaEscribiendo = false;
        talking = false;
        _currentInventory = null;

        if (panelDialogo != null) panelDialogo.SetActive(false);
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