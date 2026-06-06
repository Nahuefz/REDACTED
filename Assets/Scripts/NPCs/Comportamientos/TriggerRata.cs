using UnityEngine;

public class TriggerRata : MonoBehaviour
{
    [Header("Conexiones")]
    public RataArchivo laRata;
    public Transform puntoDestino;

    [Header("Configuracion")]
    [Tooltip("Tildá esto SOLO en el trigger que la manda al rincón final.")]
    public bool esElUltimoPunto = false;

    private bool yaSeUso = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaSeUso)
        {
            yaSeUso = true;

            if (laRata != null && puntoDestino != null)
            {
                laRata.EscapeTo(puntoDestino, esElUltimoPunto);
            } 
        }
    }
}
