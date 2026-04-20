using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Componentes UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDialogo;

    private Queue<string> oraciones;
    private bool talking = false;
    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        oraciones = new Queue<string>();
        panelDialogo.SetActive(false);
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
