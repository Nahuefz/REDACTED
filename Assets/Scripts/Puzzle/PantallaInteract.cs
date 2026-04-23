using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PantallaInteract : MonoBehaviour, IOutlined
{
    /* LOGICA
     * QUIERO QUE CON UN INPUT PUEDA CAMBIAR EL COLOR DE LAS "PANTALLAS"
     * CICLICAMENTE CON LA REFERENCIA DEL ARRAY DE COLORES DE PANTALLACOLOR
     * 
     */
    
    [SerializeField] private GameObject[] _pantallas;
    [SerializeField] private Material[] colors;
    [SerializeField] private PlayerInputs _changeColorAction;
    
    [SerializeField] private PantallaColor colorReference;

    [SerializeField] private Outline[] outline;
    private void Awake()
    {
        colorReference = GetComponentInChildren<PantallaColor>();
        _pantallas = colorReference._pantallas;
        Debug.Log("BRODER TENES QUE USAR RAYCAST!");
        
    } 
    

    private void Start()
    {
        colors = colorReference._pantallaColor;
        
        outline = new Outline[_pantallas.Length];
        for (int i = 0; i < outline.Length; i++)
        {
            outline[i] = _pantallas[i].GetComponent<Outline>();
        }
    }
    
    // Dentro de PantallaInteract.cs, puedes sobrecargar el método así:
    public void DrawOutline(GameObject objetoGolpeado)
    {
        // Buscamos en qué posición del array está el objeto que el Raycast tocó
        int index = Array.IndexOf(_pantallas, objetoGolpeado);
    
        // Si lo encuentra (índice diferente de -1), llama a tu función original
        if (index != -1) 
        {
            DrawOutline(index);
        }
    }
    public void EraseOutline(GameObject objetoGolpeado)
    {
        // Buscamos en qué posición del array está el objeto que el Raycast tocó
        int index = Array.IndexOf(_pantallas, objetoGolpeado);
    
        // Si lo encuentra (índice diferente de -1), llama a tu función original
        if (index != -1) 
        {
            EraseOutline(index);
        }
    }

    public void DrawOutline(int outlineIndex)
    {
        outline[outlineIndex].enabled = true;
    }

    public void EraseOutline(int  outlineIndex)
    {
        outline[outlineIndex].enabled = false;
    }

    public void EraseOutline()
    {
        for (int i = 0; i < outline.Length; i++)
        {
            outline[i].enabled = false;
        }
    }
}
