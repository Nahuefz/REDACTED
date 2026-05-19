using UnityEngine;

[System.Serializable]
public struct ActorDialogo
{
    public string nombreEnElScript;
    public Transform anclaMirada;
    public bool hacerZoom;

    [Tooltip("Mas chico = mas zoom. (1.2 es plano pecho)")]
    public float nivelDeZoom;
}
