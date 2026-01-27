using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 12f;
    [SerializeField] private float rotationSpeed = 6f;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private RigBuilder rig;

    private float verticalVelocity;                   
    private Vector3 targetDirection;
    private float currentSpeed;
    private int speedHash = Animator.StringToHash("Speed");
    private int dieHash = Animator.StringToHash("Die");
    private bool isDead = false;

    private void OnEnable()
    {
        controller.enabled = true;
        isDead = false;
        //rig.enabled = true;
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
        rig.enabled = false;
        currentSpeed = 0f;
        animator.SetFloat(speedHash, 0f);
        animator.SetBool(dieHash,true);
    }

    private void Update()
    {
        if (isDead) 
        {
            controller.enabled = false;
            return;
        }

        if (targetDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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

        Vector3 move = transform.forward * currentSpeed + Vector3.up * verticalVelocity;
        controller.Move(move * Time.deltaTime);
        animator.SetFloat(speedHash, currentSpeed);
    }
   

}