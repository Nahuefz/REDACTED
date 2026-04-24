using System;
using UnityEngine;

public class InteractRay : MonoBehaviour
{ 
    [Header("INTERACT")]
    [Space(2)]
    [SerializeField][Range(0f, 100f)] private float rayMaxDistance = 25f;
    [SerializeField] private LayerMask interactMask;
    private IOutlined lastTarget;
    [SerializeField] private GameObject lastTargetObj;
    [Space(5)]
    [Header("Camera")]
    [SerializeField] private Transform _camera;

    public IInteractable CurrentInteractable { get; private set; }

    private void Awake()
    {
       if(_camera == null) _camera = Camera.main.transform;
    }

    private void Update()
    {
        CastInteractiveRay();
    }
    void CastInteractiveRay()
    {
        Ray interactRay = new Ray(_camera.position, _camera.forward);
        RaycastHit raycastHit;

        if (Physics.Raycast(interactRay, out raycastHit, rayMaxDistance, interactMask))
        {
            //Debug.DrawRay(interactRay.origin, interactRay.direction, Color.red);
            //Debug.Log($"<b>Raycast:</b> <color=yellow>{raycastHit.transform.name}</color>");
            //COMENTADO PORQUE ANDA!
            CurrentInteractable = raycastHit.collider.GetComponentInParent<IInteractable>();
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
            CurrentInteractable = null;
            
            if (lastTarget != null)
            {
                if (lastTargetObj != null) lastTarget.EraseOutline(lastTargetObj);
                
                lastTarget = null;
                lastTargetObj = null;
            }
        }
    }
}
