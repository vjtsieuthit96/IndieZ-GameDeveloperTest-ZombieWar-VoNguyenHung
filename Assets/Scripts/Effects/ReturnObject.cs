using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ReturnObject : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckParticleAlive());
    }

    private IEnumerator CheckParticleAlive()
    {        
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (ps != null && !ps.IsAlive(true))
            {
                ObjectPoolManager.ReturnObjectToPool(this.gameObject);
                yield break; 
            }
        }
    }
}