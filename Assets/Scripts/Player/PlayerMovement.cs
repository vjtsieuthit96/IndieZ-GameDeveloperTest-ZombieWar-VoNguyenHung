using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 12f;
    [SerializeField] private float rotationSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;

    private Vector3 targetDirection;
    private float currentSpeed;
    private float verticalVelocity;

    private int speedHash = Animator.StringToHash("Speed");
    private int dieHash = Animator.StringToHash("Die");
    private bool isDead = false;

    private void OnEnable()
    {
        controller.enabled = true;
        isDead = false;

        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnMoveJoystick += OnMove;
            GameEventManager.Instance.OnMoveRelease += OnMoveRelease;
            GameEventManager.Instance.OnPlayerDied += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnMoveJoystick -= OnMove;
            GameEventManager.Instance.OnMoveRelease -= OnMoveRelease;
            GameEventManager.Instance.OnPlayerDied -= OnPlayerDied;
        }
    }

    private void OnMove(Vector2 input)
    {        
        targetDirection = new Vector3(input.x, 0, input.y).normalized;
    }

    private void OnMoveRelease()
    {
        targetDirection = Vector3.zero;
    }

    private void OnPlayerDied()
    {
        isDead = true;
        currentSpeed = 0f;
        animator.SetFloat(speedHash, 0f);
        animator.SetBool(dieHash, true);
    }

    private void Update()
    {
        if (isDead)
        {
            controller.enabled = false;
            return;
        }

        float targetSpeed = targetDirection.magnitude * maxSpeed;
        float lerpRate = targetSpeed > currentSpeed ? acceleration : deceleration;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * lerpRate);

        if (Mathf.Abs(currentSpeed) < 0.25f)
        {
            currentSpeed = 0f;
        }

        // Gravity
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
       
        Vector3 moveDirection = targetDirection * currentSpeed + Vector3.up * verticalVelocity;
        controller.Move(moveDirection * Time.deltaTime);
       
        Vector3 flatMove = new Vector3(moveDirection.x, 0, moveDirection.z);
        if (flatMove.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatMove);
            controller.transform.rotation = Quaternion.Slerp(
                controller.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        animator.SetFloat(speedHash, currentSpeed);
    }
}