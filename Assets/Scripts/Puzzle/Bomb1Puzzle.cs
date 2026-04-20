using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bomb1Puzzle : MonoBehaviour
{ 
    //LOGICA PRINCIPAL
    /*
        1 = rojo
        2 = azul
        3 = verde
        4 = violeta
    */
        [Header("SOLUCION")] 
        [SerializeField] public int[] _puzzleSolution = new int[6]; 
        private string stringSolucion;
        
        //[SerializeField] private GameObject[] _pantallas = new GameObject[6];
        //private MeshRenderer _meshRenderer;
        [SerializeField] private PantallaColor _pantallaColor;

        private void Awake()
        {
            _pantallaColor = GetComponentInChildren<PantallaColor>();
        }

        private void Start()
        {
            GenerateSolution();
            _pantallaColor.SetPantallaColor();
        }
  
        /// <summary>
        /// Genera la solucion para el puzzle!
        /// Y la muestra en consola.
        /// </summary>
        void GenerateSolution()
        {
            for (int i = 0; i < _puzzleSolution.Length; i++)
            {
                int randomValue = Random.Range(1, 5);
                _puzzleSolution[i] = randomValue;
          
                string colorHex = DebugHexColors(randomValue);
          
                stringSolucion += $"<color={colorHex}>{randomValue}</color>";
          
                if (i < _puzzleSolution.Length - 1)
                stringSolucion += ", ";
            }
          
            Debug.Log($"<color=white>SOLUCION =</color> {stringSolucion}");
        }

        private string DebugHexColors(int num)
        {
            switch (num)
            {
                case 1:
                    return "#FF0000";
                case 2:
                    return "#0000FF";
                case 3:
                    return "#00FF00";
                case 4 :  
                    return "#800080";
                default:
                    return "#FFFFFF";
            }
        }
}
