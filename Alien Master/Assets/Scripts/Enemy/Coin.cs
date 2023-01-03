using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.AI;

public class Coin : MonoBehaviour
{

    [SerializeField] CoinData stats;
    public bool staticCoin;
    float rotateAmount;

    private void OnEnable()
    {
        rotateAmount = Random.Range(-90, 90);
        stats.timeToFollow = stats.timeToFollowAmount;
        stats.magnetSpeed = 0;
        Invoke("JumpCoin",0.02f);
    }

    private void Update()
    {
        stats.timeToFollow -= Time.deltaTime;
        if (!staticCoin)
        {
            transform.Rotate(rotateAmount * Time.deltaTime * stats.rotateSpeed, 0, rotateAmount * Time.deltaTime * stats.rotateSpeed);
            if (stats.timeToFollow <= 0)
            {
                StartCoroutine(MoveToPlayer());
            }
           
        }
        
    }

    public void JumpCoin()
    {

        Vector3 jumpDest = new Vector3(Random.Range(transform.position.x - stats.xAxisForce, transform.position.x + stats.xAxisForce), 0, Random.Range(transform.position.z - stats.zAxisForce, transform.position.z + stats.zAxisForce));

        if (!staticCoin)
        {
            transform.DOScale(Random.Range(1.2f, 1.9f), 0.5f);
            transform.DOJump(jumpDest, stats.jumpPower, 1, Random.Range(0.5f, 1f)).SetDelay(0.05f);
        }  
    }

    IEnumerator MoveToPlayer()
    {
        stats.magnetSpeed = stats.magnetSpeedAmount;
        transform.position = Vector3.MoveTowards(transform.position, PlayerAttacking.Instance.transform.position + new Vector3(0, 1f, 0), stats.magnetSpeed * Time.deltaTime);
        yield return new WaitForEndOfFrame();
        stats.magnetSpeed += 15;
        yield return new WaitForEndOfFrame();
        stats.magnetSpeed += 5;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerAttacking.Instance.lootEffect.gameObject.SetActive(true);
            PlayerAttacking.Instance.lootEffect.transform.position = other.gameObject.transform.position + new Vector3(0,1f,0);
            PlayerAttacking.Instance.lootEffect.Play();
            GameManager.Instance.totalCoin += stats.coinAmount;
            PlayerProjectilePool.Instance.OnReleaseCoin(this.gameObject);
        }
    }


}
