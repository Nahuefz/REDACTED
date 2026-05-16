using System;
using UnityEngine;

using System;
using UnityEngine;

public class OutlineTesting : MonoBehaviour, IOutlined
{
    private Outline outline;
    [SerializeField] float _outlineWidth = 4f;

    private void Awake()
    {
        outline = GetComponent<Outline>();

        // Apagamos el outline al inicio para probar que el trigger funcione
        if (outline != null) outline.OutlineWidth = 0f;
    }

    public void DrawOutline(GameObject obj)
    {
        if (outline != null) outline.OutlineWidth = _outlineWidth;
    }

    public void EraseOutline(GameObject obj)
    {
        if (outline != null) outline.OutlineWidth = 0f;
    }

    // --- MÉTODOS DE PRUEBA CON EL MOUSE ---

    // Se ejecuta cuando el puntero del mouse entra en el cubo
    private void OnMouseEnter()
    {
        DrawOutline(this.gameObject);
    }

    // Se ejecuta cuando el puntero del mouse sale del cubo
    private void OnMouseExit()
    {
        EraseOutline(this.gameObject);
    }
}
