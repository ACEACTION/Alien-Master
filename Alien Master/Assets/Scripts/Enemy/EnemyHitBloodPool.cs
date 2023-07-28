using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHitBloodPool : MonoBehaviour
{
    [SerializeField] EnemyHitBlood hitPrefab;
    public ObjectPool<EnemyHitBlood> pool;

    public static EnemyHitBloodPool Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        pool = new ObjectPool<EnemyHitBlood>(CreateBlood, OnGet, OnRelease, OnDestoryBlood, false, 10, 50);
    }

    private void OnDestoryBlood(EnemyHitBlood obj)
    {
        Destroy(obj);
    }

    private void OnRelease(EnemyHitBlood obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnGet(EnemyHitBlood obj)
    {
        obj.gameObject.SetActive(true);
    }

    private EnemyHitBlood CreateBlood()
    {
        EnemyHitBlood blood = Instantiate(hitPrefab);

        return blood;
    }

    public void OnReleaseBlood(EnemyHitBlood blood)
    {
        pool.Release(blood);
    }
}
