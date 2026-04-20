using System;
using UnityEngine;

public class PantallaColor : MonoBehaviour
{
    [Header("Materiales")]
    [SerializeField] private Material[] _pantallaColor =  new Material[4];
    [Space(2)]
    [Header("Pantallas")]
    [SerializeField] private GameObject[] _pantallas = new GameObject[6];
    [SerializeField] private MeshRenderer[] _pantallasRenderers;
    [Space(2)] 
    [SerializeField] private Bomb1Puzzle _puzzleLogic;

    private void Awake()
    {
        _puzzleLogic = GetComponentInParent<Bomb1Puzzle>();

        for (int i = 0; i < _pantallas.Length; i++)
        {
            _pantallas[i] = GameObject.Find("pantalla" + (i + 1));
        }
        
        _pantallasRenderers = new MeshRenderer[_pantallas.Length];
        for (int i = 0; i < _pantallas.Length; i++)
        {
            _pantallasRenderers[i] = _pantallas[i].GetComponent<MeshRenderer>();
        }
    }
    

    public void SetPantallaColor()
    {
        for (int i = 0; i < _pantallasRenderers.Length; i++)
        {
            _pantallasRenderers[i].material = _pantallaColor[_puzzleLogic._puzzleSolution[i] - 1];  
        }
    }
}
