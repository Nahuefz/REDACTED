using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class MovingPlatformBehaviour : MonoBehaviour
{
    [SerializeField] private float transparentSpeed = 1f;
    [SerializeField] private float reappearDelay = 2f;
    
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private bool _isFading = false;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !_isFading)
        {
            StartCoroutine(FadeDisableAndReset());
        }
    }

    private IEnumerator FadeDisableAndReset()
    {
        _isFading = true;
        Color currentColor = _meshRenderer.material.color;
        
        while (currentColor.a > 0.05f)
        {
            float newAlpha = math.lerp(currentColor.a, 0f, Time.deltaTime * transparentSpeed);
            currentColor.a = newAlpha;
            _meshRenderer.material.color = currentColor;
            yield return null;
        }
        
        currentColor.a = 0f;
        _meshRenderer.material.color = currentColor;
        if (_collider != null) _collider.enabled = false;
        yield return new WaitForSeconds(reappearDelay);
        
        currentColor.a = 1f;
        _meshRenderer.material.color = currentColor;
        if (_collider != null) _collider.enabled = true;
        
        _isFading = false;
    }
}
