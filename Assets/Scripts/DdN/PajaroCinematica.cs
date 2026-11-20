using System.Collections;
using UnityEngine;

public class PajaroCinematica : MonoBehaviour
{
    [Header("Referencias (si los dejás vacíos los busca solo)")]
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    [Header("Aparición y Alejamiento")]
    [Tooltip("Opcional: un Empty detrás o al costado de la cámara para que entre planeando. Si está vacío aparece directo en su lugar.")]
    public Transform puntoEntrada;
    [Tooltip("Cuánto se aleja en cada diálogo (X=costado, Y=arriba, Z=adelante).")]
    public Vector3 avancePorDialogo = new Vector3(0f, 1.2f, 3.5f);
    public float velocidadMovimiento = 2.5f;

    [Header("Planeo sutil (poné 0 si lo querés quieto)")]
    public float amplitudPlaneo = 0.15f;
    public float velocidadPlaneo = 2f;
    public float inclinacionPlaneo = 4f;

    [Header("Transformaciones")]
    public Material materialEstupido;
    public Mesh meshPrimeraPrimitiva;
    public Mesh[] meshesPrimitivas;
    public float intervaloBuclePrimitivas = 0.25f;
    public bool restaurarMaterialAlVolver = true;

    [Tooltip("Por si el tamaño del pájaro es muy distinto al de las primitivas de Unity (que miden 1m).")]
    public bool usarEscalaParaPrimitivas = false;
    public Vector3 escalaPrimitivas = Vector3.one;

    [Header("Escape final al cielo")]
    public Vector3 direccionEscape = new Vector3(0f, 1.5f, 1f);
    public float velocidadEscapeInicial = 10f;
    public float aceleracionEscape = 8f;

    [Header("Inicio")]
    public bool empezarOculto = true;

    private Mesh _meshOriginal;
    private Material _materialOriginal;
    private Vector3 _escalaOriginal;
    private Vector3 _posicionInicialEscena;
    private Quaternion _rotacionOriginal;

    private Vector3 _posicionBase;
    private Vector3 _posicionObjetivo;
    private bool _activo = false;
    private bool _escapando = false;
    private float _velocidadActualEscape;
    private Coroutine _rutinaBucle;

    private void Awake()
    {
        if (meshFilter == null) meshFilter = GetComponentInChildren<MeshFilter>();
        if (meshRenderer == null) meshRenderer = GetComponentInChildren<MeshRenderer>();

        if (meshFilter != null) _meshOriginal = meshFilter.sharedMesh;
        if (meshRenderer != null) _materialOriginal = meshRenderer.sharedMaterial;

        _escalaOriginal = transform.localScale;
        _posicionInicialEscena = transform.position;
        _rotacionOriginal = transform.rotation;
        _posicionBase = _posicionInicialEscena;
        _posicionObjetivo = _posicionInicialEscena;
    }

    private void Start()
    {
        if (empezarOculto && meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
    }

    private void Update()
    {
        if (!_activo) return;

        if (_escapando)
        {
            _velocidadActualEscape += aceleracionEscape * Time.deltaTime;
            _posicionBase += direccionEscape.normalized * (_velocidadActualEscape * Time.deltaTime);
        }
        else
        {
            _posicionBase = Vector3.Lerp(_posicionBase, _posicionObjetivo, Time.deltaTime * velocidadMovimiento);
        }

        // Flotación y leve balanceo de planeo
        float flotacionY = Mathf.Sin(Time.time * velocidadPlaneo) * amplitudPlaneo;
        transform.position = _posicionBase + Vector3.up * flotacionY;

        float roll = Mathf.Sin(Time.time * velocidadPlaneo * 0.7f) * inclinacionPlaneo;
        transform.rotation = _rotacionOriginal * Quaternion.Euler(0f, 0f, roll);
    }

    // --- ACCIONES PARA LA CORRUTINA MAESTRA ---

    [ContextMenu("1. Aparecer")]
    public void Aparecer()
    {
        _activo = true;
        _escapando = false;
        if (meshRenderer != null) meshRenderer.enabled = true;

        _posicionObjetivo = _posicionInicialEscena;
        _posicionBase = (puntoEntrada != null) ? puntoEntrada.position : _posicionInicialEscena;
        transform.position = _posicionBase;
    }

    [ContextMenu("2. Alejar un paso")]
    public void AlejarUnPaso()
    {
        _posicionObjetivo += avancePorDialogo;
    }

    [ContextMenu("3. Cambiar Material")]
    public void CambiarMaterial()
    {
        if (meshRenderer != null && materialEstupido != null)
        {
            meshRenderer.material = materialEstupido;
        }
    }

    [ContextMenu("4. Cambiar a 1 Primitiva")]
    public void CambiarAPrimitivaUnica()
    {
        if (meshFilter != null && meshPrimeraPrimitiva != null)
        {
            meshFilter.mesh = meshPrimeraPrimitiva;
            if (usarEscalaParaPrimitivas) transform.localScale = escalaPrimitivas;
        }
    }

    [ContextMenu("5. Iniciar Bucle Primitivas")]
    public void IniciarBuclePrimitivas()
    {
        if (meshesPrimitivas == null || meshesPrimitivas.Length == 0) return;
        if (_rutinaBucle != null) StopCoroutine(_rutinaBucle);
        _rutinaBucle = StartCoroutine(BuclePrimitivasRoutine());
    }

    private IEnumerator BuclePrimitivasRoutine()
    {
        if (usarEscalaParaPrimitivas) transform.localScale = escalaPrimitivas;

        int indice = 0;
        while (true)
        {
            if (meshFilter != null && meshesPrimitivas[indice] != null)
            {
                meshFilter.mesh = meshesPrimitivas[indice];
            }
            indice = (indice + 1) % meshesPrimitivas.Length;
            yield return new WaitForSeconds(intervaloBuclePrimitivas);
        }
    }

    [ContextMenu("6. Volver al Pajaro Inicial")]
    public void VolverAlPajaroInicial()
    {
        if (_rutinaBucle != null)
        {
            StopCoroutine(_rutinaBucle);
            _rutinaBucle = null;
        }

        if (meshFilter != null && _meshOriginal != null)
        {
            meshFilter.mesh = _meshOriginal;
        }

        if (restaurarMaterialAlVolver && meshRenderer != null && _materialOriginal != null)
        {
            meshRenderer.material = _materialOriginal;
        }

        transform.localScale = _escalaOriginal;
    }

    [ContextMenu("7. Volar al Cielo")]
    public void VolarAlCielo()
    {
        _escapando = true;
        _velocidadActualEscape = velocidadEscapeInicial;
    }
}