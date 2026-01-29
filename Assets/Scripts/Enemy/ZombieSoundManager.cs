using UnityEngine;

public class ZombieSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] zombieAggressive;
    [SerializeField] private AudioClip[] zombieDeath;
    [SerializeField] private AudioClip[] zombieGrowl;
    [SerializeField] private AudioClip[] zombieChase;
    [SerializeField] private AudioClip zombieGrunt;
    [SerializeField] private AudioClip zombieHiss;
    [SerializeField] private AudioClip zombieMoan;

    void OnEnable()
    {
        GameEventManager.Instance.OnEnemyHit += HandleEnemyHit;
    }

    void OnDisable()
    {
        GameEventManager.Instance.OnEnemyHit -= HandleEnemyHit;
    }

    private void HandleEnemyHit(GameObject enemy)
    {
        if (enemy == this.gameObject)
        {
            var rand = Random.Range(0, 100);
            if (rand < 50)
                PlayAggressiveSound();
        }
    }

    public void PlayAggressiveSound()
    {
        AudioManager.Instance.PlayRandomSound(zombieAggressive, transform.position);
    }

    public void PlayDeathSound()
    {
        AudioManager.Instance.PlayRandomSound(zombieDeath, transform.position);
    }

    public void PlayGrowlSound()
    {
        AudioManager.Instance.PlayRandomSound(zombieGrowl, transform.position);
    }

    public void PlayChaseSound()
    {
        AudioManager.Instance.PlayRandomSound(zombieChase, transform.position);
    }

    public void PlayGruntSound()
    {
        AudioManager.Instance.PlaySound(zombieGrunt, transform.position);
    }

    public void PlayHissSound()
    {
        AudioManager.Instance.PlaySound(zombieHiss, transform.position);
    }

    public void PlayMoanSound()
    {
        AudioManager.Instance.PlaySound(zombieMoan, transform.position);
    }
}