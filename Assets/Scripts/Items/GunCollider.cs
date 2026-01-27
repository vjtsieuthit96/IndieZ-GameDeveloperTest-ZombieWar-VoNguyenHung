using UnityEngine;

public class GunCollider : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private void OnEnable()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.None;
        GameEventManager.Instance.OnPlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnPlayerDied -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }
}
