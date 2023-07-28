using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBlood : MonoBehaviour
{
    public ParticleSystem bloodEff;

    private void OnEnable()
    {
        StartCoroutine(Release());
    }

    IEnumerator Release()
    {
        yield return new WaitForSeconds(1.5f);
        bloodEff.Stop();
        EnemyHitBloodPool.Instance.OnReleaseBlood(this);
    }

}
