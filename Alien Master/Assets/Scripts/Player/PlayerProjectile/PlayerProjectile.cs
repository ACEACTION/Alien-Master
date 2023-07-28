using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public PlayerProjectileStats stats;
    [SerializeField] Vector3 offset;
    [SerializeField] bool isMelee;
    public Vector3 direction;
    


    private void Update()
    {
        if(!isMelee)
            transform.position += direction * Time.deltaTime * stats.speed;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {

            if (!isMelee)
            {
                CameraShake.Instance.ShakeCamera(0.2f, 1f, 0.5f);
                PlayerProjectilePool.Instance.hitEffect = stats.hitEffect;
                //var hit = PlayerProjectilePool.Instance.hitEffectPool.Get();
                //hit.transform.position = transform.position;
                PlayerProjectilePool.Instance.onReleaseProjectile(this);
                PlayerAttacking.Instance.canDealDmg = true;
            }
        }
    }    

    IEnumerator EffectDestroyer()
    {
        yield return new WaitForSeconds(1f);
        PlayerProjectilePool.Instance.onReleaseProjectile(this);

    }
}
