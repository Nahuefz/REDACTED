using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Enemy.Core;

public class VisibilityMeter : MonoBehaviour
{
    [SerializeField] private GameObject visibilityMeter;
    [SerializeField] private float fadeInDuration = 0.15f;
    [SerializeField] private float fadeOutDuration = 0.15f;

    private Image _visibilityMeterFill;
    private Image _visibilityMeterIcon;
    private Coroutine _fadeCoroutine;
    private bool _isFadingOut;

    private void Awake()
    {
        // Buscamos los componentes necesarios
        _visibilityMeterFill = visibilityMeter.transform.Find("Fill").GetComponent<Image>();
        _visibilityMeterIcon = visibilityMeter.transform.Find("UI").GetComponent<Image>();
        
        // Inicializamos oculto
        _visibilityMeterFill.fillAmount = 0f;
        SetImagesAlpha(0f);
        _visibilityMeterFill.enabled = false;
        _visibilityMeterIcon.enabled = false;

        if (visibilityMeter.activeSelf) visibilityMeter.SetActive(false);
    }

    private void OnEnable()
    {
        // Nos suscribimos al evento estático de los enemigos tímidos
        EnemyEvents.OnScaredVisibilityChanged += UpdateVisibilityUI;
    }

    private void OnDisable()
    {
        EnemyEvents.OnScaredVisibilityChanged -= UpdateVisibilityUI;
        StopFade();
    }

    private void UpdateVisibilityUI(float progress)
    {
        if (progress > 0)
        {
            bool wasHidden = !visibilityMeter.activeSelf;

            if (_isFadingOut)
            {
                StopFade();
                SetImagesAlpha(1f);
            }

            // Si hay progreso, activamos y actualizamos
            if (!visibilityMeter.activeSelf) visibilityMeter.SetActive(true);
            
            _visibilityMeterFill.enabled = true;
            _visibilityMeterIcon.enabled = true;
            _visibilityMeterFill.fillAmount = progress;

            if (wasHidden)
            {
                StartFadeIn();
            }
        }
        else
        {
            // Si no hay progreso, ocultamos el medidor
            if (visibilityMeter.activeSelf && !_isFadingOut)
            {
                StartFadeOut();
            }
        }
    }

    private void StartFadeIn()
    {
        StopFade();
        _isFadingOut = false;
        SetImagesAlpha(0f);
        _fadeCoroutine = StartCoroutine(FadeImages(0f, 1f, fadeInDuration, false));
    }

    private void StartFadeOut()
    {
        StopFade();
        _isFadingOut = true;
        _fadeCoroutine = StartCoroutine(FadeImages(GetCurrentAlpha(), 0f, fadeOutDuration, true));
    }

    private void StopFade()
    {
        if (_fadeCoroutine == null) return;

        StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = null;
        _isFadingOut = false;
    }

    private IEnumerator FadeImages(float fromAlpha, float toAlpha, float duration, bool hideOnComplete)
    {
        if (duration <= 0f)
        {
            SetImagesAlpha(toAlpha);
            CompleteFade(hideOnComplete);
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            SetImagesAlpha(Mathf.Lerp(fromAlpha, toAlpha, progress));
            yield return null;
        }

        SetImagesAlpha(toAlpha);
        CompleteFade(hideOnComplete);
    }

    private void CompleteFade(bool hideOnComplete)
    {
        _fadeCoroutine = null;
        _isFadingOut = false;

        if (!hideOnComplete) return;

        _visibilityMeterFill.enabled = false;
        _visibilityMeterIcon.enabled = false;
        _visibilityMeterFill.fillAmount = 0f;

        if (visibilityMeter.activeSelf) visibilityMeter.SetActive(false);
    }

    private float GetCurrentAlpha()
    {
        if (_visibilityMeterFill == null) return 0f;

        return _visibilityMeterFill.color.a;
    }

    private void SetImagesAlpha(float alpha)
    {
        SetImageAlpha(_visibilityMeterFill, alpha);
        SetImageAlpha(_visibilityMeterIcon, alpha);
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image == null) return;

        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
