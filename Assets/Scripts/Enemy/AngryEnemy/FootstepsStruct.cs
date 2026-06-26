using System;
using UnityEngine;

[Serializable]
public struct FootstepsStruct
{
    [SerializeField] private float stepInterval /*0.6f*/; // Tiempo en segundos entre cada pisada
    [SerializeField] private float maxShakeRadius /*= 25f*/; // Distancia máxima para empezar a sentir el temblor
    [SerializeField] private float maxShakeForce /*= 0.25f*/; // Fuerza máxima en la cara del jugador
    [SerializeField] private float shakeDuration /*= 0.15f*/;
    
    public float StepInterval => stepInterval;
    public float MaxShakeRadius => maxShakeRadius;
    public float MaxShakeForce => maxShakeForce;
    public float ShakeDuration => shakeDuration;
}
