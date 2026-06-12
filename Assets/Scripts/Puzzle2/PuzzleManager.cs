using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    [Header("Configuracion del Puzzle")]
    [Tooltip("Cantidad total de NPCs")]
    public int cantidadDeNPCs;
#if UNITY_EDITOR
    [Tooltip("Escena a la que el jugador cae si falla")]
    [SerializeField] private SceneAsset escenaAnteriorAsset;
#endif

    [HideInInspector]
    public string escenaAnterior;

    [Header("Recompensas")]
    [Tooltip("Prefab del BasicItem que va a dropear el NPC correcto")]
    public GameObject itemRecompensaPrefab;

    public int ObjetivoID {  get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ObjetivoID = Random.Range(0, cantidadDeNPCs);
        Debug.Log("<color=orange>El culpable de este puzzle es el NPC con ID: " + ObjetivoID + "</color>");
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (escenaAnterior != null)
        {
            escenaAnterior = escenaAnteriorAsset.name;
        }
#endif
    }

    public void IntentarAsesinato (int npcID, Transform npcTransform)
    {
        if (npcID == ObjetivoID)
        {
            Debug.Log("<color=green> El jugador mato al NPC correcto.</color>");

            if (itemRecompensaPrefab != null)
            {
                Instantiate(itemRecompensaPrefab, npcTransform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("<color=red>NPC equivocado!</color>");
                SceneManager.LoadScene(escenaAnterior);
            }
        }
    }

}
 