using UnityEngine;

public class Bomb1Puzzle : MonoBehaviour
{ 
    // LOGICA PRINCIPAL: 1 = rojo, 2 = azul, 3 = verde, 4 = violeta

    [Header("SOLUCION")] 
    [SerializeField] public int[] _puzzleSolution = new int[6]; 
    
    [Header("CONFIGURACIÓN VISUAL")]
    [SerializeField] private Material[] clueColors;
    [SerializeField] private GameObject[] clueScreens;

    private MeshRenderer[] _clueRenderers;

    public Material[] ClueColors => clueColors;

    private void Awake()
    {
        // Cacheamos los mesh renderers de las pantallas de pista
        _clueRenderers = new MeshRenderer[clueScreens.Length];
        for (int i = 0; i < clueScreens.Length; i++)
        {
            if (clueScreens[i] != null)
            {
                _clueRenderers[i] = clueScreens[i].GetComponent<MeshRenderer>();
            }
        }
    }

    private void Start()
    {
        GenerateSolution();
        SetClueScreenColors();
    }

    /// <summary>
    /// Genera la solucion para el puzzle y la muestra en consola.
    /// </summary>
    private void GenerateSolution()
    {
        string stringSolucion = "";
        for (int i = 0; i < _puzzleSolution.Length; i++)
        {
            int randomValue = Random.Range(1, 5);
            _puzzleSolution[i] = randomValue;
      
            string colorHex = DebugHexColors(randomValue);
            stringSolucion += $"<color={colorHex}>{randomValue}</color>";
      
            if (i < _puzzleSolution.Length - 1)
            {
                stringSolucion += ", ";
            }
        }
      
        Debug.Log($"<color=white>SOLUCION =</color> {stringSolucion}");
    }

    private void SetClueScreenColors()
    {
        for (int i = 0; i < _clueRenderers.Length; i++)
        {
            if (_clueRenderers[i] != null)
            {
                int colorIndex = _puzzleSolution[i] - 1;
                _clueRenderers[i].material = clueColors[colorIndex];
            }
        }
    }

    /// <summary>
    /// Compara la combinación del jugador con la solución generada.
    /// </summary>
    public bool CheckSolution(int[] playerCombination)
    {
        if (playerCombination == null || playerCombination.Length != _puzzleSolution.Length)
        {
            return false;
        }

        for (int i = 0; i < _puzzleSolution.Length; i++)
        {
            // El valor de la solución es 1-4, la combinación del jugador es 0-3
            if (_puzzleSolution[i] != playerCombination[i] + 1)
            {
                return false;
            }
        }

        Debug.Log("<color=green>¡PUZZLE RESUELTO!</color>");
        // Aquí se puede disparar lógica adicional (abrir puerta, apagar alarma, etc.)
        return true;
    }

    private string DebugHexColors(int num)
    {
        return num switch
        {
            1 => "#FF0000", // Rojo
            2 => "#0000FF", // Azul
            3 => "#00FF00", // Verde
            4 => "#800080", // Violeta
            _ => "#FFFFFF"  // Blanco
        };
    }
}
