using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHitPool : MonoBehaviour
{
    [SerializeField] GameObject hitPrefab;
    public ObjectPool<GameObject> pool;

    public static EnemyHitPool Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
        }                
    }

    void Start()
    {        
        pool = new ObjectPool<GameObject>(CreateHit, OnGet, OnRelease, OnDestoryHit, false, 10, 50);        
    }

    private void OnDestoryHit(GameObject obj)
    {
        Destroy(obj);
    }

    private void OnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    private GameObject CreateHit()
    {
        GameObject hit = Instantiate(hitPrefab);

        return hit;
    }

    public void OnReleaseHit(GameObject bullet)
    {
        pool.Release(bullet);
    }
}
