using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    public InputManager inputManager;
    public Vector3 moveDirection;
    public Transform cameraObject;
    Rigidbody playerRigidbody;

    public bool isGrounded;
    public bool isFalling;

    public float inAirTimer;
    public float leapingVelocity = 3f;
    public float fallingVelocity = 33f;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;

    public float movementSpeed = 7;
    public float rotationSpeed = 15;

    public bool isClimbing = false;
    public float climbSpeed = 3f;

    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootForce = 700f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void HandleAllMovement()
    {
        if (isClimbing)
        {
            HandleClimbing();
            return;
        }

        HandleFallingAndLanding();
        TryStepUp();
        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = new Vector3(movementVelocity.x, playerRigidbody.velocity.y, movementVelocity.z);
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y += rayCastHeightOffset;

        if (!isGrounded)
        {
            if (!playerManager.isInteracting && !isFalling)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
                isFalling = true;
            }

            inAirTimer += Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.3f, -Vector3.up, out hit, 1f, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            playerRigidbody.useGravity = false;
            playerRigidbody.velocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            playerRigidbody.useGravity = true;
        }
    }

    private void HandleClimbing()
    {
        if (isClimbing)
        {
            float vertical = inputManager.verticalInput;
            Vector3 climbDirection = new Vector3(0, vertical * climbSpeed, 0);
            playerRigidbody.velocity = climbDirection;

            if (vertical != 0)
            {
                animatorManager.PlayTargetAnimation("Climb", true);
            }
        }
    }

    private void TryStepUp()
    {
        RaycastHit lowerHit;
        RaycastHit upperHit;

        Vector3 origin = transform.position + Vector3.up * 0.1f;
        Vector3 forward = transform.forward;

        if (Physics.Raycast(origin, forward, out lowerHit, 0.5f))
        {
            Vector3 upperOrigin = transform.position + Vector3.up * 0.5f;
            if (!Physics.Raycast(upperOrigin, forward, out upperHit, 0.5f))
            {
                // יש מדרגה שאפשר לטפס עליה
                playerRigidbody.position += new Vector3(0, 0.1f, 0); // דחיפה עדינה למעלה
            }
        }
    }
    
    public void FireProjectile()
    {
        if (projectilePrefab == null || shootPoint == null) return;

        GameObject projectile = GameObject.Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(transform.forward));
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
        }

        Debug.Log("Player fired fireball");
    }
}
