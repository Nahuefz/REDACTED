using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionFade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeTime = 1f;

    private void Start()
    {
        if (fadeImage != null)
        {
            // Nos aseguramos que la imagen esté activa y bloquee clics al inicio
            fadeImage.gameObject.SetActive(true);
            fadeImage.raycastTarget = true;
            
            // Empezamos el fade para mostrar la escena (de negro a transparente)
            StartCoroutine(Fade(1f, 0f));
        }
    }

    public IEnumerator FadeToBlack()
    {
        if (fadeImage == null) yield break;

        // Bloqueamos interacciones y hacemos el fade a negro
        fadeImage.raycastTarget = true;
        yield return StartCoroutine(Fade(0f, 1f));
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        float timer = 0;
        Color color = fadeImage.color;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeTime);
            fadeImage.color = color;
            yield return null;
        }
        
        color.a = targetAlpha;
        fadeImage.color = color;

        // Si terminamos en transparente, liberamos los clics
        if (targetAlpha <= 0)
        {
            fadeImage.raycastTarget = false;
        }
    }
}
