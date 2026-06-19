using UnityEngine;
using UnityEngine.InputSystem;

public class DisparoPuzzle2 : MonoBehaviour
{
    private PlayerInputs inputActions;

    [Header("Cofiguracion del Raycast")]
    public Transform cameraTransform;
    public float rangoDelRaycast = 10f;

    private void Awake()
    {
        inputActions = new PlayerInputs();
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Shoot.performed += EjecutarAcusacion;
    }

    private void OnDisable()
    {
        inputActions.Player.Shoot.performed -= EjecutarAcusacion;
        inputActions.Player.Disable();
    }

    private void EjecutarAcusacion(InputAction.CallbackContext context)
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.EstaHablando()) return;

        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rangoDelRaycast))
        {
            SurrealPuzzleNPC npc = hit.collider.GetComponent<SurrealPuzzleNPC>();

            if (npc != null)
            {
                Debug.Log("Acusacion lanzada al NPC " + npc.miID);
                npc.RecibirAtaqueMortal();
            }
        }
    }

    private void OnDestroy()
    {
        if (inputActions != null)
        {
            inputActions.Disable();
            inputActions.Dispose();
        }
    }
}
