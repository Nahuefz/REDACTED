using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Componentes UI")] public GameObject panelDialogo;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDialogo;
    public GameObject iconoContinuar;

    [Header("Nota")] public GameObject notaEnCamara;

    private Queue<LineaDeDialogo> oraciones;
    private bool talking = false;
    private bool looking = false;

    [SerializeField] Image interactButtonImage;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        oraciones = new Queue<LineaDeDialogo>();
        panelDialogo.SetActive(false);

        if (notaEnCamara != null) notaEnCamara.SetActive(false);
    }

    private void Update()
    {
        if (talking && Mouse.current.leftButton.wasPressedThisFrame)
        {
            MostrarSiguienteOracion();
        }
    }

    public void EmpezarDialogo(DialogoData dialogo)
    {
        talking = true;
        panelDialogo.SetActive(true);

        if (interactButtonImage != null)
        {
            interactButtonImage.enabled = false;
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

        if (oracionActual.imagen != null)
        {
            if (notaEnCamara != null)
            {
                //  �Para cuando sea en 3D la imagen!
                //
                //SpriteRenderer rederizador = notaEnCamara.GetComponent<SpriteRenderer>();
                //
                //if (rederizador != null)
                //{
                //    rederizador.sprite = oracionActual.imagen;
                //}
                //
                //notaEnCamara.SetActive(true);

                Image image = notaEnCamara.GetComponent<Image>();

                if (image != null)
                {
                    image.sprite = oracionActual.imagen;
                }

                notaEnCamara.SetActive(true);
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
        panelDialogo.SetActive(false);

        if (notaEnCamara != null) notaEnCamara.SetActive(false);
        if (interactButtonImage != null) interactButtonImage.enabled = looking;
    }

    public bool EstaHablando()
    {
        return talking;
    }

    void InteractableSeen(bool isSeen)
    {
        //Debug.Log($"<color=red>INTERACTABLES IS SEEN {isSeen}</color>");

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

    private void OnDestroy()
    {
        // Ya se desuscribe en OnDisable, pero lo dejamos por si acaso o lo removemos si OnDisable es suficiente.
        // En Unity, OnDisable se llama antes que OnDestroy.
    }
}