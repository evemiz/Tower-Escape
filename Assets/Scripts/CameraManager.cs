using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public bool isCameraLocked = false;
    InputManager inputManager;

    public Transform targetTransform;   // The object the camera will follow
    public Transform cameraPivot;       // The object the camera uses to pivot (Look up and down)
    public Transform cameraTransform;   // The transform of the actual camera object in the scene
    public LayerMask collisionLayers;   // The layers we want our camera to collide with
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public float cameraCollisionsOffset = 0.2f;      // How much the camera will jump off of objects it colliding with
    public float minimumCollisionOffset = 0.2f;
    public float maximumCollisionOffset = 0.2f;
    public float cameraCollisionRadius = 2;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float lookAngle;     // Camera looking up and down
    public float pivotAngle;    // Camera looking left and right
    public float minimumPivotAngle = -18;
    public float maximumPivotAngle = 35;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        defaultPosition = cameraTransform.localPosition.z;

        lookAngle = transform.eulerAngles.y;
        pivotAngle = cameraPivot.localEulerAngles.x;

    }

    public void HandleAllCameraMovement()
    {
        if (isCameraLocked)
            return;
            
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }
    
    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle + (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    // private void RotateCamera()
    // {
    //     Vector3 direction = targetTransform.forward;
    //     if (direction.sqrMagnitude < 0.001f) return;

    //     direction.y = 0;
    //     if (direction == Vector3.zero) direction = transform.forward;

    //     Quaternion targetRotation = Quaternion.LookRotation(direction);
    //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * cameraLookSpeed);

    //     // הפיבוט מאופס כך שהמצלמה תמיד בגובה אחיד ולא נוטה למעלה/למטה
    //     cameraPivot.localRotation = Quaternion.identity;
    // }
    

    private void HandleCameraCollisions()
    {
        float targetPosiotion = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosiotion), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosiotion = -(distance - cameraCollisionsOffset);
        }

        if (Mathf.Abs(targetPosiotion) < minimumCollisionOffset)
        {
            targetPosiotion = targetPosiotion - minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosiotion, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
