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
        //OnInteractSeen?.Invoke(CurrentInteractable != null); ponerlo en algun
    }
    void CastInteractiveRay()
    {
        Ray interactRay = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit raycastHit;

        if (Physics.Raycast(interactRay, out raycastHit, rayMaxDistance, interactMask))
        {
            //IInteractable foundInteractable = raycastHit.collider.GetComponent<IInteractable>(); //var local para el delegate del ui
            
            CurrentInteractable = raycastHit.collider.GetComponentInParent<IInteractable>();
            IOutlined currentTarget = raycastHit.collider.GetComponentInParent<IOutlined>();
            GameObject currentHitObject = raycastHit.collider.gameObject;

            if (CurrentInteractable != null)
            {
                OnInteractSeen(true);
            }
            
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
            if (CurrentInteractable != null)
            {
                CurrentInteractable = null;
                OnInteractSeen(false);
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
