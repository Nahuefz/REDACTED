using UnityEngine;

public class CamaraCinematica : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadRotacion = 2.5f;

    private bool _siguiendo = false;

    private void Update()
    {
        if (!_siguiendo || objetivo == null) return;

        Vector3 direccion = objetivo.position - transform.position;
        if (direccion.sqrMagnitude < 0.001f) return;

        Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * velocidadRotacion);
    }

    [ContextMenu("Empezar a seguir")]
    public void EmpezarASeguir(Transform nuevoObjetivo = null)
    {
        if (nuevoObjetivo != null) objetivo = nuevoObjetivo;
        _siguiendo = true;
    }

    public void DejarDeSeguir()
    {
        _siguiendo = false;
    }
}