using UnityEngine;
using System.Collections;

public class DirectorInicio : MonoBehaviour
{
    public static DirectorInicio Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [Header("Anclas de Animacion")]
    public Transform puntoSentado;
    public Transform anclaMiradaMonitor;

    private Vector3 posCamaraParado;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement != null && playerMovement.cameraTransform != null)
        {
            posCamaraParado = playerMovement.cameraTransform.localPosition;
            SentarJugador();
        }
    }

    private void SentarJugador()
    {
        playerMovement.enabled = false;

        if (puntoSentado != null) playerMovement.cameraTransform.position = puntoSentado.position;

        if (anclaMiradaMonitor != null) playerMovement.cameraTransform.LookAt(anclaMiradaMonitor);
        else if (puntoSentado != null) playerMovement.cameraTransform.rotation = puntoSentado.rotation;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void IniciarSecuenciaLevantarse()
    {
        StartCoroutine(RutinaLevantarse());
    }

    private IEnumerator RutinaLevantarse()
    {
        float duracion = 1.2f;
        float tiempo = 0f;

        Vector3 posInicialCamara = playerMovement.cameraTransform.localPosition;
        Quaternion rotInicialCamara = playerMovement.cameraTransform.localRotation;

        Quaternion rotFinalCamara = Quaternion.identity;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracion);

            playerMovement.cameraTransform.localPosition = Vector3.Lerp(posInicialCamara, posCamaraParado, t);
            playerMovement.cameraTransform.localRotation = Quaternion.Slerp(rotInicialCamara, rotFinalCamara, t);

            yield return null;
        }

        playerMovement.cameraTransform.localPosition = posCamaraParado;
        playerMovement.cameraTransform.localRotation = rotFinalCamara;

        playerMovement.SetXRotation(0f);
        playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
