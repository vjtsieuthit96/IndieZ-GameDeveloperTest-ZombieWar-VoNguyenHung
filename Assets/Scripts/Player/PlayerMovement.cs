using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 12f;
    [SerializeField] private float rotationSpeed = 6f;
    [SerializeField] private CharacterController controller;

    [SerializeField] private float gravity = -9.81f;
    private float verticalVelocity;                    

    private Vector3 targetDirection;
    private float currentSpeed;

    private void OnEnable()
    {
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnMoveJoystick += OnMove;
            GameEventManager.Instance.OnMoveRelease += OnMoveRelease;
        }
    }

    private void OnDisable()
    {
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnMoveJoystick -= OnMove;
            GameEventManager.Instance.OnMoveRelease -= OnMoveRelease;
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

    private void Update()
    {       
        if (targetDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
      
        float targetSpeed = targetDirection.magnitude * maxSpeed;
        float lerpRate = targetSpeed > currentSpeed ? acceleration : deceleration;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * lerpRate);

        // Gravity
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; 
        }
        
        Vector3 move = transform.forward * currentSpeed + Vector3.up * verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }
}