using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Recompensas y Efectos")]
    [Tooltip("Prefab del BasicItem que va a dropear el NPC correcto")]
    public GameObject itemRecompensaPrefab;
    [Tooltip("Sonido que se reproduce al matar al correcto")]
    public AudioClip sonidoAcierto;
    public GameObject huecoSalidaPrefab;

    public int ObjetivoID {  get; private set; }
    private AudioSource audioSource;
    private bool puzzleTerminado = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
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

    public void IntentarAsesinato (int npcID, GameObject npcObject, Vector3 posicionSuelo)
    {
        if (puzzleTerminado) return;

        puzzleTerminado = true;
        if (npcID == ObjetivoID)
        {
            Debug.Log("<color=green> El jugador mato al NPC correcto.</color>");
            StartCoroutine(SecuenciaAcierto(npcObject, posicionSuelo));
        }
        else
        {
            Debug.Log("<color=red>NPC equivocado!</color>");
            SceneManager.LoadScene(escenaAnterior);
        }
    }

    private IEnumerator SecuenciaAcierto(GameObject npcObject, Vector3 posicionSuelo)
    {
        yield return new WaitForSeconds(0.5f);

        if (sonidoAcierto != null) audioSource.PlayOneShot(sonidoAcierto);

        if (itemRecompensaPrefab != null)
        {
            Instantiate(itemRecompensaPrefab, npcObject.transform.position, Quaternion.identity);
        }

        if (huecoSalidaPrefab != null)
        {
            Instantiate(huecoSalidaPrefab, posicionSuelo, Quaternion.Euler(90, 0, 0));
        }

        Destroy(npcObject);
    }
}
 