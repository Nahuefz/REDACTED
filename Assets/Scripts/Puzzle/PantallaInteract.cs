using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PantallaInteract : MonoBehaviour
{
    /* LOGICA
     * QUIERO QUE CON UN INPUT PUEDA CAMBIAR EL COLOR DE LAS "PANTALLAS"
     * CICLICAMENTE CON LA REFERENCIA DEL ARRAY DE COLORES DE PANTALLACOLOR
     * 
     */
    
    [SerializeField] private GameObject[] _pantallas;
    [SerializeField] private Material[] _colors;
    [SerializeField] private PlayerInputs _changeColorAction;
    
    [SerializeField] private PantallaColor _colorReference; 
    private void Awake()
    {
        _colorReference = GetComponentInChildren<PantallaColor>();
        _pantallas = _colorReference._pantallas;
        Debug.Log("BRODER TENES QUE USAR RAYCAST!");
    } 
    

    private void Start()
    {
        _colors = _colorReference._pantallaColor;
    }
}
