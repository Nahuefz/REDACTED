using System.Collections;
using UnityEngine;

public class PrinterEmissionInteractable : MonoBehaviour, IInteractable
{
    [Header("Configuración de Emisión")]
    [SerializeField] private Renderer vidrioRenderer;
    [SerializeField] private float duracionFotocopia = 1.5f;

    [ColorUsage(true, true)]
    [SerializeField] private Color colorEncendido = Color.green;

    [Header("Configuración de Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Configuración de Spawn (Papel)")]
    [SerializeField] private GameObject prefabHoja; // El clon azul de tu carpeta Prefabs
    [SerializeField] private Transform spawnPointPapel; // El objeto vacío de la bandeja

    private Material materialVidrio;
    private Color colorApagado = Color.black;
    private bool estaFotocopiando = false;

    void Start()
    {
        if (vidrioRenderer != null)
        {
            materialVidrio = vidrioRenderer.material;
            materialVidrio.SetColor("_EmissionColor", colorApagado);
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void Interact(GameObject player)
    {
        if (!estaFotocopiando && materialVidrio != null)
        {
            StartCoroutine(EfectoFotocopiaCompleto());
        }
    }

    private IEnumerator EfectoFotocopiaCompleto()
    {
        estaFotocopiando = true;

        // 1. Encendemos brillo y sonido
        materialVidrio.SetColor("_EmissionColor", colorEncendido);
        if (audioSource != null) audioSource.Play();

        Debug.Log("Impresora: Fotocopiando...");

        // 2. Esperamos que termine el audio / escaneo
        yield return new WaitForSeconds(duracionFotocopia);

        // 3. Apagamos la luz
        materialVidrio.SetColor("_EmissionColor", colorApagado);

        // 4. ¡SPAWNEAMOS LA HOJA!
        if (prefabHoja != null && spawnPointPapel != null)
        {
            // Creamos la copia exacta en la posición y rotación del SpawnPoint
            Instantiate(prefabHoja, spawnPointPapel.position, spawnPointPapel.rotation);
            Debug.Log("Impresora: Hoja impresa con éxito.");
        }
        else
        {
            Debug.LogWarning("DQA Warning: Faltan referencias del prefab o del spawnpoint en la impresora.");
        }

        estaFotocopiando = false;
    }

    private void OnDestroy()
    {
        if (materialVidrio != null) Destroy(materialVidrio);
    }
}