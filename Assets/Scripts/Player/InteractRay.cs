using System;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractRay : MonoBehaviour
{ 
    [Header("INTERACT")]
    [Space(2)]
    [SerializeField][Range(0f, 100f)] private float rayMaxDistance = 25f;
    [SerializeField] private LayerMask interactMask;
    private IOutlined lastTarget;
    [SerializeField] private GameObject lastTargetObj;
    [FormerlySerializedAs("Camera")]
    [FormerlySerializedAs("_camera")]
    [Space(5)]
    [Header("Camera")]
    [SerializeField] private Transform playerCamera;

    public IInteractable CurrentInteractable { get; private set; } //PARA EL CONTORNO
    public static event Action<bool> OnInteractSeen;

    private void Awake()
    {
       if(playerCamera == null) playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        CastInteractiveRay();
        Debug.DrawRay(playerCamera.position, playerCamera.forward * rayMaxDistance, Color.red);

        // Intentamos interactuar solo si hay algo válido
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentInteractable != null)
            {
                CurrentInteractable.Interact(this.gameObject);
            }
            else
            {
                Debug.Log("No estoy mirando nada interactuable");
            }
        }
    }
    void CastInteractiveRay()
    {
        Ray interactRay = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit raycastHit;

        if (Physics.Raycast(interactRay, out raycastHit, rayMaxDistance, interactMask))
        {
            // Usamos GetComponentInParent para encontrar el script aunque el collider esté en un hijo
            IInteractable foundInteractable = raycastHit.collider.GetComponentInParent<IInteractable>();
            
            // Si el objeto que estamos mirando ha cambiado
            if (foundInteractable != CurrentInteractable)
            {
                CurrentInteractable = foundInteractable;
                OnInteractSeen?.Invoke(CurrentInteractable != null);
            }
            
            IOutlined currentTarget = raycastHit.collider.GetComponentInParent<IOutlined>();
            GameObject currentHitObject = raycastHit.collider.gameObject;
            
            if (currentHitObject != lastTargetObj)
            {
                if (lastTarget != null && lastTargetObj != null)
                {
                    lastTarget.EraseOutline(lastTargetObj);
                }
                lastTarget = currentTarget;
                lastTargetObj = currentHitObject;
                if (lastTarget != null)
                {
                    lastTarget.DrawOutline(lastTargetObj);
                }
            }
        }
        else
        {
            // Si no golpeamos nada, reseteamos el interactuable actual
            if (CurrentInteractable != null)
            {
                CurrentInteractable = null;
                OnInteractSeen?.Invoke(false);
            }

            if (lastTarget != null)
            {
                if (lastTargetObj != null) lastTarget.EraseOutline(lastTargetObj);
                
                lastTarget = null;
                lastTargetObj = null;
            }
        }
    }
    
    
}
