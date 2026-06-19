using System;
using UnityEngine;
using UnityEngine.UI;
using Enemy.Core;

public class VisibilityMeter : MonoBehaviour
{
    [SerializeField] private GameObject visibilityMeter;
    private Image _visibilityMeterFill;
    private Image _visibilityMeterIcon;

    private void Awake()
    {
        // Buscamos los componentes necesarios
        _visibilityMeterFill = visibilityMeter.transform.Find("Fill").GetComponent<Image>();
        _visibilityMeterIcon = visibilityMeter.transform.Find("UI").GetComponent<Image>();
        
        // Inicializamos oculto
        UpdateVisibilityUI(0f);
    }

    private void OnEnable()
    {
        // Nos suscribimos al evento estático de los enemigos tímidos
        EnemyEvents.OnShyVisibilityChanged += UpdateVisibilityUI;
    }

    private void OnDisable()
    {
        EnemyEvents.OnShyVisibilityChanged -= UpdateVisibilityUI;
    }

    private void UpdateVisibilityUI(float progress)
    {
        if (progress > 0)
        {
            // Si hay progreso, activamos y actualizamos
            if (!visibilityMeter.activeSelf) visibilityMeter.SetActive(true);
            
            _visibilityMeterFill.enabled = true;
            _visibilityMeterIcon.enabled = true;
            _visibilityMeterFill.fillAmount = progress;
        }
        else
        {
            // Si no hay progreso, ocultamos el medidor
            _visibilityMeterFill.fillAmount = 0;
            _visibilityMeterFill.enabled = false;
            _visibilityMeterIcon.enabled = false;
            
            if (visibilityMeter.activeSelf) visibilityMeter.SetActive(false);
        }
    }
}
