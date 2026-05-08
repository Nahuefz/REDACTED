using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Componentes UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDialogo;
    public GameObject iconoContinuar;

    private Queue<LineaDeDialogo> oraciones;
    private bool talking = false;
    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        oraciones = new Queue<LineaDeDialogo>();
        panelDialogo.SetActive(false);
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
        oraciones.Clear();

        foreach (LineaDeDialogo linea in dialogo.dialogos)
        {
            LineaDeDialogo lineaProcesada = linea;

            if (string.IsNullOrWhiteSpace(lineaProcesada.nombrePersonaje))
            {
                lineaProcesada.nombrePersonaje = dialogo.nombrePorDefecto;
            }
            oraciones.Enqueue(linea);
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

        iconoContinuar.SetActive(oraciones.Count > 0);
    }

    public void TerminarDialogo()
    {
        talking = false;
        panelDialogo.SetActive(false); 
    }

    public bool EstaHablando()
    {
        return talking;
    }

}
