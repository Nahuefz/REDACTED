using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AnimadorSpritesUI : MonoBehaviour
{
    public Sprite[] frames;
    [Tooltip("Cuadros por segundo (entre 6 y 12 suele quedar bien para estilo dibujado/stop-motion).")]
    public float fps = 8f;

    private Image _imagen;
    private float _timer;
    private int _indiceActual;

    private void Awake()
    {
        _imagen = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _timer = 0f;
        _indiceActual = 0;
        if (frames != null && frames.Length > 0 && frames[0] != null)
        {
            _imagen.sprite = frames[0];
        }
    }

    private void Update()
    {
        if (frames == null || frames.Length <= 1 || fps <= 0f) return;

        _timer += Time.deltaTime;
        float duracionFrame = 1f / fps;

        if (_timer >= duracionFrame)
        {
            _timer -= duracionFrame;
            _indiceActual = (_indiceActual + 1) % frames.Length;
            _imagen.sprite = frames[_indiceActual];
        }
    }
}