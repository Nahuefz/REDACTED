using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PantallaInteract : MonoBehaviour, IOutlined, IInteractable
{
    [SerializeField] private GameObject[] _pantallas;
    [SerializeField] private Material[] colors;
    [SerializeField] private PantallaColor colorReference;
    [SerializeField] private Outline[] outline;

    private MeshRenderer[] _renderers;
    private int[] _currentColorIndex;
    private int _focusedIndex = -1;
    private void Awake()
    {
        colorReference = this.transform.parent.Find("Pantallas").GetComponent<PantallaColor>();
        //_pantallas = colorReference._pantallas;
    }
    

    private void Start()
    {
        colors = colorReference._pantallaColor;

        outline = new Outline[_pantallas.Length];
        _renderers = new MeshRenderer[_pantallas.Length];
        _currentColorIndex = new int[_pantallas.Length];

        for (int i = 0; i < _pantallas.Length; i++)
        {
            outline[i] = _pantallas[i].GetComponent<Outline>();
            _renderers[i] = _pantallas[i].GetComponent<MeshRenderer>();
            _currentColorIndex[i] = 0;
        }
    }
    public void Interact()
    {
        CyclicColorChange();
    }
    private void CyclicColorChange()
    {
        if (_focusedIndex < 0 || _focusedIndex >= _renderers.Length) return;

        _currentColorIndex[_focusedIndex]++;
        if (_currentColorIndex[_focusedIndex] >= colors.Length)
        {
            _currentColorIndex[_focusedIndex] = 0;
        }

        _renderers[_focusedIndex].material = colors[_currentColorIndex[_focusedIndex]];
    }
    
    #region OutlineMethods

    public void DrawOutline(GameObject objetoGolpeado)
    {
        int index = Array.IndexOf(_pantallas, objetoGolpeado);
        if (index >= 0)
        {
            DrawOutline(index);
        }
    }

    public void EraseOutline(GameObject objetoGolpeado)
    {
        int index = Array.IndexOf(_pantallas, objetoGolpeado);
        if (index >= 0)
        {
            EraseOutline(index);
        }
    }

    public void DrawOutline(int outlineIndex)
    {
        if (outlineIndex < 0 || outlineIndex >= outline.Length) return;

        outline[outlineIndex].enabled = true;
        _focusedIndex = outlineIndex;
    }


    public void EraseOutline(int outlineIndex)
    {
        if (outlineIndex < 0 || outlineIndex >= outline.Length) return;

        outline[outlineIndex].enabled = false;

        if (_focusedIndex == outlineIndex)
        {
            _focusedIndex = -1;
        }
    }

    #endregion
}
