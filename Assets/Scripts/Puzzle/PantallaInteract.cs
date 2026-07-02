using System;
using UnityEngine;

public class PantallaInteract : MonoBehaviour, IOutlined, IInteractable
{
    [SerializeField] private GameObject[] _pantallas;
    private Material[] colors;
    private Outline[] outline;

    private Bomb1Puzzle _puzzleLogic;
    private MeshRenderer[] _renderers;
    [SerializeField] private int[] _currentColorIndex;
    [SerializeField] private int _focusedIndex = -1;

    private void Awake()
    {
        _puzzleLogic = GetComponentInParent<Bomb1Puzzle>();
        if (_puzzleLogic != null)
        {
            colors = _puzzleLogic.ClueColors;
        }
    }

    private void Start()
    {
        outline = new Outline[_pantallas.Length];
        _renderers = new MeshRenderer[_pantallas.Length];
        _currentColorIndex = new int[_pantallas.Length];

        for (int i = 0; i < _pantallas.Length; i++)
        {
            if (_pantallas[i] != null)
            {
                outline[i] = _pantallas[i].GetComponent<Outline>();
                _renderers[i] = _pantallas[i].GetComponent<MeshRenderer>();
            }
            _currentColorIndex[i] = 0;
        }
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Interact called, focused index = {_focusedIndex}");
        CyclicColorChange();
    }

    private void CyclicColorChange()
    {
        if (_focusedIndex < 0 || _focusedIndex >= _renderers.Length) return;

        _currentColorIndex[_focusedIndex] = (_currentColorIndex[_focusedIndex] + 1) % colors.Length;
        
        if (_renderers[_focusedIndex] != null)
        {
            _renderers[_focusedIndex].material = colors[_currentColorIndex[_focusedIndex]];
        }

        // Chequear si con este cambio se resolvió el puzzle
        if (_puzzleLogic != null)
        {
            _puzzleLogic.CheckSolution(_currentColorIndex);
        }
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

        if (outline[outlineIndex] != null)
        {
            outline[outlineIndex].OutlineWidth = 6.8f;
        }
        _focusedIndex = outlineIndex;
    }

    public void EraseOutline(int outlineIndex)
    {
        if (outlineIndex < 0 || outlineIndex >= outline.Length) return;

        if (outline[outlineIndex] != null)
        {
            outline[outlineIndex].OutlineWidth = 0f;
        }

        if (_focusedIndex == outlineIndex)
        {
            _focusedIndex = -1;
        }
    }

    #endregion
}
