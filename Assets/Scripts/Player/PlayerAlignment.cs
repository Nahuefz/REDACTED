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
    public void Alinear(Transform puntoDestino, Transform npc, Action alTerminar)
    {
        StartCoroutine(RutinaAlinear(puntoDestino, npc, alTerminar));
    }

    public IEnumerator RutinaAlinear(Transform destino, Transform npc, Action alTerminar)
    {
        if (playerMovement != null) playerMovement.enabled = false;

        float duracion = 0.4f; //Tiempo de la transicion.
        float tiempo = 0f;

        Vector3 posInicial = transform.position;
        Quaternion rotInicial = transform.rotation;

        Vector3 destinoPlano = new Vector3(destino.position.x, posInicial.y, destino.position.z);

        Vector3 dirNpc = npc.position - destino.position;
        dirNpc.y = 0;
        Quaternion rotFinal = Quaternion.LookRotation(dirNpc);

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracion);

            transform.position = Vector3.Lerp(posInicial, destinoPlano, t);
            transform.rotation = Quaternion.Slerp(rotInicial, rotFinal, t);

            yield return null;
        }

        transform.position = destinoPlano;
        transform.rotation = rotFinal;

        if (playerMovement != null)
        {
            playerMovement.LookAtFront();
            playerMovement.enabled = true;
        }

        alTerminar?.Invoke();
    }
}
