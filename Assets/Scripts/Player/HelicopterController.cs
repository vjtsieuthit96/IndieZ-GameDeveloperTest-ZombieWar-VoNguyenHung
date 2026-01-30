using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    [SerializeField] private Transform landingSpot;
    [SerializeField] private float flyHeight = 40f;
    [SerializeField] private float flySpeed = 5f;
    [SerializeField] private Animator helicopterAnimator;
    [SerializeField] private AudioSource audioSource;

    private int isFlyingHash = Animator.StringToHash("isFlying");
    private bool isTakingOff = false;
    private float flightTimer = 0f;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool hasArrived = false;
    private bool isLanding = false;
    private void OnEnable()
    {
        GameEventManager.Instance.OnHelicopterTakeOff += HandleHelicopterTakeOff;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnHelicopterTakeOff -= HandleHelicopterTakeOff;
    }

    private void HandleHelicopterTakeOff()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.up * flyHeight;
        isTakingOff = true;
        flightTimer = 0f;
        hasArrived = false;
        isLanding = false;
        helicopterAnimator.SetBool(isFlyingHash, true);
        if (audioSource != null) audioSource.Play();
    }   

    private void Update()
    {
        if (isTakingOff && !isLanding)
        {
            flightTimer += Time.deltaTime;
           
            if (transform.position.y < targetPos.y)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y + flySpeed * Time.deltaTime,
                    transform.position.z
                );
            }
                       
            if (flightTimer >= 30f)
            {
                if (!audioSource.isPlaying && !isLanding) audioSource.Play();
                isLanding = true;
            }
        }

        if (isLanding)
        {            
            transform.position = new Vector3(
                transform.position.x,
                Mathf.MoveTowards(transform.position.y, landingSpot.position.y, flySpeed * Time.deltaTime),
                transform.position.z
            );

            if (!hasArrived && Mathf.Abs(transform.position.y - landingSpot.position.y) < 0.1f)
            {
                helicopterAnimator.SetBool(isFlyingHash, false);
                isTakingOff = false;
                isLanding = false;
                hasArrived = true;
                GameEventManager.Instance.InvokeHelicopterArrived();
                Debug.Log("Helicopter Arrived Event Fired!");
            }
        }
    }
}