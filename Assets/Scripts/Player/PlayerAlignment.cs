using System;
using System.Collections;
using UnityEngine;

public class PlayerAlignment : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Coroutine rutinaMiradaActiva;
    private Camera camaraPrincipal;
    private float fovPorDefecto;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        camaraPrincipal = Camera.main;
        if (camaraPrincipal != null) fovPorDefecto = camaraPrincipal.fieldOfView;
    }

    #region Corutina Alinear
    public void Alinear(Transform puntoDestino, Transform npc, Transform puntoMirada, Action alTerminar)
    {
        StartCoroutine(RutinaAlinear(puntoDestino, npc, puntoMirada, alTerminar));
    }

    public IEnumerator RutinaAlinear(Transform destino, Transform npc, Transform puntoMirada, Action alTerminar)
    {
        if (playerMovement != null) playerMovement.enabled = false;

        float duracion = 0.4f;
        float tiempo = 0f;

        Vector3 posInicial = transform.position;
        Quaternion rotInicial = transform.rotation;
        float pitchInicial = playerMovement != null ? playerMovement.GetXRotation() : 0f;

        Vector3 destinoPlano = new Vector3(destino.position.x, posInicial.y, destino.position.z);

        Vector3 dirNpc = npc.position - destino.position;
        dirNpc.y = 0;
        Quaternion rotFinal = Quaternion.LookRotation(dirNpc);

        float pitchFinal = 0f;
        if (puntoMirada != null && playerMovement != null && playerMovement.cameraTransform != null)
        {
            Vector3 posAFuturo = new Vector3(destinoPlano.x, playerMovement.cameraTransform.position.y, destinoPlano.z);
            Vector3 diferencia = puntoMirada.position - posAFuturo;
            float distanciaXZ = new Vector2(diferencia.x, diferencia.z).magnitude;
            pitchFinal = Mathf.Atan2(-diferencia.y, distanciaXZ) * Mathf.Rad2Deg;
        }

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracion);
            transform.position = Vector3.Lerp(posInicial, destinoPlano, t);
            transform.rotation = Quaternion.Slerp(rotInicial, rotFinal, t);
            if (playerMovement != null)
            {
                float pitchActual = Mathf.Lerp(pitchInicial, pitchFinal, t);
                playerMovement.SetXRotation(pitchActual);
            }
            yield return null;
        }

        transform.position = destinoPlano;
        transform.rotation = rotFinal;

        if (playerMovement != null)
        {
            playerMovement.SetXRotation(pitchFinal);
            playerMovement.enabled = true;
        }

        if (camaraPrincipal != null) camaraPrincipal.fieldOfView = fovPorDefecto;
        alTerminar?.Invoke();
    }
    #endregion

    #region Alinear Solo Rotacion
    public void AlinearSoloRotacion(Transform npc, Transform puntoMirada, Action alTerminar)
    {
        StartCoroutine(RutinaAlinearSoloRotacion(npc, puntoMirada, alTerminar));
    }

    private IEnumerator RutinaAlinearSoloRotacion(Transform npc, Transform puntoMirada, Action alTerminar)
    {
        if (playerMovement != null) playerMovement.enabled = false;

        float duracion = 0.4f;
        float tiempo = 0f;
        Quaternion rotInicial = transform.rotation;
        float pitchInicial = playerMovement != null ? playerMovement.GetXRotation() : 0f;

        Vector3 dirNpc = npc.position - transform.position;
        dirNpc.y = 0;
        Quaternion rotFinal = Quaternion.LookRotation(dirNpc);

        float pitchFinal = 0f;
        if (puntoMirada != null && playerMovement != null && playerMovement.cameraTransform != null)
        {
            Vector3 diferencia = puntoMirada.position - playerMovement.cameraTransform.position;
            float distanciaXZ = new Vector2(diferencia.x, diferencia.z).magnitude;
            pitchFinal = Mathf.Atan2(-diferencia.y, distanciaXZ) * Mathf.Rad2Deg;
        }

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracion);

            // Solo rotación, se omite transform.position
            transform.rotation = Quaternion.Slerp(rotInicial, rotFinal, t);
            if (playerMovement != null)
                playerMovement.SetXRotation(Mathf.Lerp(pitchInicial, pitchFinal, t));

            yield return null;
        }

        transform.rotation = rotFinal;
        if (playerMovement != null)
        {
            playerMovement.SetXRotation(pitchFinal);
            playerMovement.enabled = true;
        }

        alTerminar?.Invoke();
    }
    #endregion

    // ... (Mantén tus métodos existentes de CambiarMirada y RestaurarFOV debajo)
    #region Métodos de Mirada y FOV
    public void CambiarMirada(Transform puntoMirada, bool hacerZoom, float nivelDeZoom)
    {
        if (rutinaMiradaActiva != null) StopCoroutine(rutinaMiradaActiva);
        rutinaMiradaActiva = StartCoroutine(RutinaGirarMirada(puntoMirada, hacerZoom, nivelDeZoom));
    }

    private IEnumerator RutinaGirarMirada(Transform puntoMirada, bool hacerZoom, float nivelDeZoom)
    {
        float duracion = 0.3f;
        float tiempo = 0f;

        Quaternion rotInicialCuerpo = transform.rotation;
        float pitchInicial = playerMovement != null ? playerMovement.GetXRotation() : 0f;
        float fovInicial = camaraPrincipal != null ? camaraPrincipal.fieldOfView : 60f;

        Vector3 dirCuerpo = puntoMirada.position - transform.position;
        dirCuerpo.y = 0;
        Quaternion rotFinalCuerpo = Quaternion.LookRotation(dirCuerpo);

        float pitchFinal = 0f;
        float fovFinal = fovPorDefecto;

        if (playerMovement != null && playerMovement.cameraTransform != null)
        {
            Vector3 diferencia = puntoMirada.position - playerMovement.cameraTransform.position;
            float distancia = diferencia.magnitude;
            float distanciaXZ = new Vector2(diferencia.x, diferencia.z).magnitude;
            pitchFinal = Mathf.Atan2(-diferencia.y, distanciaXZ) * Mathf.Rad2Deg;

            if (hacerZoom && camaraPrincipal != null)
            {
                float alturaEncuadre = nivelDeZoom > 0f ? nivelDeZoom : 1.2f;
                fovFinal = 2f * Mathf.Atan(alturaEncuadre / (2f * distancia)) * Mathf.Rad2Deg;
                fovFinal = Mathf.Clamp(fovFinal, 15f, fovPorDefecto);
            }
        }

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracion);

            transform.rotation = Quaternion.Slerp(rotInicialCuerpo, rotFinalCuerpo, t);

            if (playerMovement != null)
                playerMovement.SetXRotation(Mathf.Lerp(pitchInicial, pitchFinal, t));

            if (camaraPrincipal != null)
                camaraPrincipal.fieldOfView = Mathf.Lerp(fovInicial, fovFinal, t);

            yield return null;
        }

        transform.rotation = rotFinalCuerpo;
        if (playerMovement != null) playerMovement.SetXRotation(pitchFinal);
        if (camaraPrincipal != null) camaraPrincipal.fieldOfView = fovFinal;
    }

    public void RestaurarFOV()
    {
        if (camaraPrincipal != null) camaraPrincipal.fieldOfView = fovPorDefecto;
    }
    #endregion
}