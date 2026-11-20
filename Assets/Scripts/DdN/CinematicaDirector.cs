using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CinematicaDirector : MonoBehaviour
{
    [Header("Referencias de Escena")]
    public DialogoData dialogoCinematica;
    public PajaroCinematica pajaro;
    public CamaraCinematica camara;
    public AudioSource musicaAudioSource;

    [Header("UI (Fades y Loops)")]
    public Image pantallaFade;
    public CanvasGroup tituloCanvasGroup;
    public CanvasGroup creditosCanvasGroup;

    [Header("Tiempos - Intro (en segundos)")]
    public float esperaInicialEnNegro = 1.5f;
    public float fadeInTitulo = 2.0f;
    public float duracionTituloVisible = 4.0f;
    public float fadeOutNegroYTitulo = 2.5f;
    public float sobrevueloAntesDeDialogos = 3.0f;

    [Header("Tiempos de lectura DESPUÉS de que termina de escribir cada frase")]
    public float t01_Bienvenido = 3.0f;
    public float t02_LugarFeliz = 3.0f;
    public float t03_LugarEstupido = 3.5f;
    public float t04_NoSiemprePensar = 3.5f;
    public float t05_VolverseEstupido = 3.5f;
    public float t06_MasFeliz = 3.5f;
    public float t07_ComoEl_AparecePajaro = 5.0f;
    public float t08_MasEstupido_Material = 5.0f;
    public float t09_MasPrimitivo_Mesh = 5.5f;
    public float t10_TuBelleza_Bucle = 5.5f;
    public float t11_YaSosInteligente = 5.5f;
    public float t12_LoQueTeFalta = 4.5f;

    [Header("Tiempos - Climax y Final")]
    public float pausaVueltaAlPajaroInicial = 3.5f;
    public float t13_EsSerFeliz_AntesDeBlanco = 2.5f;
    public float fadeABlanco = 6.0f;
    public float esperaAntesDeCreditos = 1.0f;
    public float fadeInCreditos = 2.5f;
    public float duracionCreditosVisible = 7f;
    [Tooltip("Si Bella todavía está sonando después de esos segundos, espera a que termine antes de cerrar.")]
    public bool esperarFinDeMusica = true;
    public float fadeOutFinalCreditos = 1.5f;
    public float fadeBlancoANegro = 2.0f;

    private float _tiempoInicio;

    private void Start()
    {
        PrepararEstadoInicial();
        StartCoroutine(EjecutarCinematica());
    }

    private void PrepararEstadoInicial()
    {
        // Pantalla arranca en negro opaco
        if (pantallaFade != null)
        {
            pantallaFade.gameObject.SetActive(true);
            pantallaFade.color = new Color(0f, 0f, 0f, 1f);
        }

        // Título y Créditos arrancan invisibles
        if (tituloCanvasGroup != null)
        {
            tituloCanvasGroup.alpha = 0f;
            tituloCanvasGroup.gameObject.SetActive(false);
        }

        if (creditosCanvasGroup != null)
        {
            creditosCanvasGroup.alpha = 0f;
            creditosCanvasGroup.gameObject.SetActive(false);
        }
    }

    private IEnumerator EjecutarCinematica()
    {
        _tiempoInicio = Time.time;

        // 1. Pantalla en negro. Empieza la canción.
        if (musicaAudioSource != null) musicaAudioSource.Play();
        LogTiempo("Inicio - Música arrancando en negro");
        yield return new WaitForSeconds(esperaInicialEnNegro);

        // 2. Título con animación en loop aparece en fade.
        LogTiempo("Fade-in Título");
        if (tituloCanvasGroup != null)
        {
            tituloCanvasGroup.gameObject.SetActive(true);
            yield return StartCoroutine(FadeCanvasGroup(tituloCanvasGroup, 0f, 1f, fadeInTitulo));
        }

        yield return new WaitForSeconds(duracionTituloVisible);

        // 3. La pantalla en negro desaparece en fade junto con el título.
        LogTiempo("Fade-out Negro + Título (se revela el piso)");
        if (tituloCanvasGroup != null)
        {
            StartCoroutine(FadeCanvasGroup(tituloCanvasGroup, 1f, 0f, fadeOutNegroYTitulo, desactivarAlFinal: true));
        }
        yield return StartCoroutine(FadeImagen(pantallaFade, Color.black, 1f, 0f, fadeOutNegroYTitulo));

        // Sobrevuelo solo antes del primer diálogo
        yield return new WaitForSeconds(sobrevueloAntesDeDialogos);

        // --- BLOQUE DE DIÁLOGOS ---

        // Línea 1: "Bienvenido a ninguna parte."
        LogTiempo("Línea 1: Bienvenido a ninguna parte.");
        DialogueManager.Instance.EmpezarDialogo(dialogoCinematica);
        yield return EsperarEscrituraYPausa(t01_Bienvenido);

        // Línea 2: "Un lugar para ser feliz."
        yield return AvanzarLinea("Línea 2: Un lugar para ser feliz.", t02_LugarFeliz);

        // Línea 3: "Un lugar para ser estúpido."
        yield return AvanzarLinea("Línea 3: Un lugar para ser estúpido.", t03_LugarEstupido);

        // Línea 4: "Porque no siempre tenés que pensar."
        yield return AvanzarLinea("Línea 4: Porque no siempre tenés que pensar.", t04_NoSiemprePensar);

        // Línea 5: "Hay que volverse más estúpido."
        yield return AvanzarLinea("Línea 5: Hay que volverse más estúpido.", t05_VolverseEstupido);

        // Línea 6: "Más feliz."
        yield return AvanzarLinea("Línea 6: Más feliz.", t06_MasFeliz);

        // Línea 7: "Como él…" -> Aparece el pájaro y la cámara empieza a seguirlo
        if (pajaro != null) pajaro.Aparecer();
        if (camara != null && pajaro != null) camara.EmpezarASeguir(pajaro.transform);
        yield return AvanzarLinea("Línea 7: Como él… (Aparece pájaro)", t07_ComoEl_AparecePajaro);

        // Línea 8: "Más estúpido." -> Se aleja y cambia de material
        if (pajaro != null)
        {
            pajaro.AlejarUnPaso();
            pajaro.CambiarMaterial();
        }
        yield return AvanzarLinea("Línea 8: Más estúpido. (Cambia material)", t08_MasEstupido_Material);

        // Línea 9: "Más primitivo." -> Se aleja y cambia a 1 primitiva
        if (pajaro != null)
        {
            pajaro.AlejarUnPaso();
            pajaro.CambiarAPrimitivaUnica();
        }
        yield return AvanzarLinea("Línea 9: Más primitivo. (1ra primitiva)", t09_MasPrimitivo_Mesh);

        // Línea 10: "Tu belleza no es que tanto te esforzas en ser inteligente." -> Se aleja y arranca bucle
        if (pajaro != null)
        {
            pajaro.AlejarUnPaso();
            pajaro.IniciarBuclePrimitivas();
        }
        yield return AvanzarLinea("Línea 10: Tu belleza... (Bucle de primitivas)", t10_TuBelleza_Bucle);

        // Línea 11: "Porque ya sos inteligente." -> Se aleja
        if (pajaro != null) pajaro.AlejarUnPaso();
        yield return AvanzarLinea("Línea 11: Porque ya sos inteligente.", t11_YaSosInteligente);

        // Línea 12: "Lo que te falta…" -> Se aleja
        if (pajaro != null) pajaro.AlejarUnPaso();
        yield return AvanzarLinea("Línea 12: Lo que te falta…", t12_LoQueTeFalta);

        // El bucle vuelve a ser el pájaro inicial
        LogTiempo("Vuelve al pájaro inicial");
        if (pajaro != null) pajaro.VolverAlPajaroInicial();
        yield return new WaitForSeconds(pausaVueltaAlPajaroInicial);

        // Línea 13: "...Es ser feliz." -> El pájaro vuela al cielo mientras funde a blanco
        LogTiempo("Línea 13: ...Es ser feliz. (Escape al cielo + Fade a blanco)");
        DialogueManager.Instance.MostrarSiguienteOracion();
        if (pajaro != null) pajaro.VolarAlCielo();

        yield return EsperarEscrituraYPausa(t13_EsSerFeliz_AntesDeBlanco);

        // Fundido a blanco mientras el pájaro se pierde en el cielo
        yield return StartCoroutine(FadeImagen(pantallaFade, Color.white, 0f, 1f, fadeABlanco));

        // Cerramos el panel de diálogo ya tapado por el blanco
        DialogueManager.Instance.TerminarDialogo();

        yield return new WaitForSeconds(esperaAntesDeCreditos);

        // Créditos con animación en loop aparecen
        LogTiempo("Fade-in Créditos");
        if (creditosCanvasGroup != null)
        {
            creditosCanvasGroup.gameObject.SetActive(true);
            yield return StartCoroutine(FadeCanvasGroup(creditosCanvasGroup, 0f, 1f, fadeInCreditos));
        }

        LogTiempo("Fin de secuencia (créditos en loop)");

        // Espera mínima para leer los créditos
        yield return new WaitForSeconds(duracionCreditosVisible);

        // Si la canción todavía no terminó, espera hasta el final exacto de Bella
        if (esperarFinDeMusica && musicaAudioSource != null)
        {
            yield return new WaitWhile(() => musicaAudioSource.isPlaying);
        }

        // Desvanece los créditos y pasa la pantalla de blanco a negro al mismo tiempo
        if (creditosCanvasGroup != null && fadeOutFinalCreditos > 0f)
        {
            StartCoroutine(FadeCanvasGroup(creditosCanvasGroup, 1f, 0f, fadeOutFinalCreditos));
        }
        yield return StartCoroutine(FadeColorImagen(pantallaFade, Color.white, Color.black, fadeBlancoANegro));

        // Medio segundo en negro para que no corte seco la ventana
        yield return new WaitForSeconds(0.5f);

        LogTiempo("Cerrando aplicación...");
        CerrarPrograma();
    }

    // --- HELPERS ---

    private IEnumerator AvanzarLinea(string etiquetaLog, float pausaDespuesDeEscribir)
    {
        LogTiempo(etiquetaLog);
        DialogueManager.Instance.MostrarSiguienteOracion();
        yield return EsperarEscrituraYPausa(pausaDespuesDeEscribir);
    }

    private IEnumerator EsperarEscrituraYPausa(float pausaExtra)
    {
        yield return new WaitWhile(() => DialogueManager.Instance.EstaEscribiendo);
        yield return new WaitForSeconds(pausaExtra);
    }

    private IEnumerator FadeImagen(Image img, Color colorBase, float alphaInicio, float alphaFin, float duracion)
    {
        if (img == null) yield break;

        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(alphaInicio, alphaFin, t / duracion);
            img.color = new Color(colorBase.r, colorBase.g, colorBase.b, a);
            yield return null;
        }
        img.color = new Color(colorBase.r, colorBase.g, colorBase.b, alphaFin);
    }

    private IEnumerator FadeColorImagen(Image img, Color colorInicio, Color colorFin, float duracion)
    {
        if (img == null) yield break;

        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            img.color = Color.Lerp(colorInicio, colorFin, t / duracion);
            yield return null;
        }
        img.color = colorFin;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float alphaInicio, float alphaFin, float duracion, bool desactivarAlFinal = false)
    {
        if (cg == null) yield break;

        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(alphaInicio, alphaFin, t / duracion);
            yield return null;
        }
        cg.alpha = alphaFin;

        if (desactivarAlFinal) cg.gameObject.SetActive(false);
    }

    private void LogTiempo(string mensaje)
    {
        float transcurrido = Time.time - _tiempoInicio;
        int min = Mathf.FloorToInt(transcurrido / 60f);
        float seg = transcurrido % 60f;
        Debug.Log($"[Cinemática {min:00}:{seg:00.0}] {mensaje}");
    }

    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CerrarPrograma();
        }
    }

    private void CerrarPrograma()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}