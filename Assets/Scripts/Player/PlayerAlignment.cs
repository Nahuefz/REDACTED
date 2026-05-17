using System;
using System.Collections;
using UnityEngine;


public class PlayerAlignment : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    public void Alinear(Transform puntoDestino, Transform npc, Transform puntoMirada, Action alTerminar)
    {
        StartCoroutine(RutinaAlinear(puntoDestino, npc, puntoMirada, alTerminar));
    }

    public IEnumerator RutinaAlinear(Transform destino, Transform npc, Transform puntoMirada, Action alTerminar)
    {
        if (playerMovement != null) playerMovement.enabled = false;

        float duracion = 0.4f; //Tiempo de la transicion.
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
            pitchFinal = Mathf.Atan2(-diferencia.y, distanciaXZ) * Mathf.Rad2Deg;//Calculo trigonometrico, waos!
        }

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracion);
            //Para mover el cuerpo
            transform.position = Vector3.Lerp(posInicial, destinoPlano, t);
            transform.rotation = Quaternion.Slerp(rotInicial, rotFinal, t);
            //Para mover el cuello
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

        alTerminar?.Invoke();
    }
}
