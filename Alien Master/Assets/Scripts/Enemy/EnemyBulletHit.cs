using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletHit : MonoBehaviour
{
    
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.5f);
        EnemyHitPool.Instance.OnReleaseHit(gameObject);
    }

}
