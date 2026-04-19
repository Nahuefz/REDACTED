using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private Transform mainCameraTransform;
    
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(mainCameraTransform.position.x,
            transform.position.y,
            mainCameraTransform.position.z);
        transform.LookAt(targetPosition);
    }
}
