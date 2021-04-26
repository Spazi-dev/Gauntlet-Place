using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraAdaptive : MonoBehaviour
{

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private float followSpeed = 3.0f;
    private Vector3 cameraRelativePosition;

    void Awake()
    {
       cameraRelativePosition = new Vector3(
           targetTransform.position.x + transform.position.x, 
           targetTransform.position.y + transform.position.y, 
           targetTransform.position.z + transform.position.z);
    }
    public void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position + cameraRelativePosition, followSpeed * Time.deltaTime);
    }

}
