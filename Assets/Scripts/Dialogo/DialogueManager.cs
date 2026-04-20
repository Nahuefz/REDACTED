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

    private Queue<string> oraciones;
    private bool talking = false;
    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        oraciones = new Queue<string>();
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
        textoNombre.text = dialogo.nombrePersonaje;

        oraciones.Clear();

        foreach (string oracion in dialogo.lineasDeDialogo)
        {
            oraciones.Enqueue(oracion);
        }

        MostrarSiguienteOracion();
    }

    public void MostrarSiguienteOracion()
    {
        if (oraciones.Count == 0 )
        {
            TerminarDialogo();
            return;
        }

        string oracionActual = oraciones.Dequeue();
        textoDialogo.text = oracionActual;

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
