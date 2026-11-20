using UnityEngine;

public class PisoInfinito : MonoBehaviour
{
    public Vector2 velocidadScroll = new Vector2(0f, 5f); // Probá positivo o negativo según hacia dónde miran las rayas
    private Renderer _rend;
    private Vector2 _offset;

    private void Awake()
    {
        _rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        _offset += velocidadScroll * Time.deltaTime;
        // En URP Unlit/Lit, la textura principal se llama "_BaseMap"
        _rend.material.SetTextureOffset("_BaseMap", _offset);
    }
}