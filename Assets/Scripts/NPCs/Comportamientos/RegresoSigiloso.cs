using UnityEngine;
using System.Collections;

public class RegresoSigiloso : MonoBehaviour
{
    [Header("Configuracion de Regreso")]
    public float tiempoDeEspera = 2f;

    private Vector3 posicionOriginal;
    private bool regresando = false;

    private void Awake()
    {
        posicionOriginal = transform.position;
    }

    public void IniciarRegreso()
    {
        if (!regresando)
        {
            StartCoroutine(RutinaRegreso());
        }
    }

    private IEnumerator RutinaRegreso()
    {
        regresando = true;

        yield return new WaitUntil(() => !DialogueManager.Instance.EstaHablando());

        yield return new WaitForSeconds(tiempoDeEspera);

        while (EstaEnPantalla())
        {
            yield return null;
        }

        transform.position = posicionOriginal;
        regresando = false;
    }

    private bool EstaEnPantalla()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds);
    }

}
