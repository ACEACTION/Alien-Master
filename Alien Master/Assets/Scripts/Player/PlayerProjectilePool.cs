using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PlayerProjectilePool : MonoBehaviour
{
    public PlayerProjectile ProjectilePrefab;
    public GameObject coinPrefab;
    public GameObject hitEffect;
    public GameObject lootEffectPrefab;
    public ObjectPool<PlayerProjectile> projectilePool;
    public ObjectPool<GameObject> hitEffectPool;
    public ObjectPool<GameObject> coinPool;
    public ObjectPool<GameObject> lootEffectPool;




    public static PlayerProjectilePool Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        projectilePool = new ObjectPool<PlayerProjectile>(SpawnBullet, OnGetProjectile, OnProjectileRelease, OnDestroyProjectile, false, 10, 50);
        hitEffectPool = new ObjectPool<GameObject>(SpawnHitEffect, OnGetHitEffect, OnHitEffectRelease, OnDestroyHitEffect, false, 10, 50);
        coinPool = new ObjectPool<GameObject>(SpawnCoin, OnGetCoin, OnCoinRelease, OnDestroyCoint, false, 10, 50);
        lootEffectPool = new ObjectPool<GameObject>(SpawnLootEffect, OnGetLootEffect, OnLootEffectRelease, OnDestroyLootEffect,false, 10, 50);
    }

    //projectile
    private void OnDestroyProjectile(PlayerProjectile obj)
    {
        Destroy(obj);
    }

    private void OnProjectileRelease(PlayerProjectile obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnGetProjectile(PlayerProjectile obj)
    {
        obj.gameObject.SetActive(true);
    }

    private PlayerProjectile SpawnBullet()
    {
        PlayerProjectile proj = Instantiate(ProjectilePrefab);
        return proj;
    }

    public void onReleaseProjectile(PlayerProjectile bullet)
    {
        projectilePool.Release(bullet);
    }

    //Hit effect
    private void OnDestroyHitEffect(GameObject obj)
    {
        Destroy(obj);
    }

    private void OnHitEffectRelease(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnGetHitEffect(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private GameObject SpawnHitEffect()
    {
        GameObject hit = Instantiate(hitEffect);

        return hit;
    }

    public void OnReleaseHitEffect(GameObject hit)
    {
        hitEffectPool.Release(hit);
    }

    //coin 

    private void OnDestroyCoint(GameObject obj)
    {
        Destroy(obj);
    }

    private void OnCoinRelease(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnGetCoin(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private GameObject SpawnCoin()
    {
        GameObject coin = Instantiate(coinPrefab);

        return coin;
    }

    public void OnReleaseCoin(GameObject coin)
    {
        coinPool.Release(coin);
    }

    //loot effect
    private void OnDestroyLootEffect(GameObject obj)
    {
        Destroy(obj);
    }

    private void OnLootEffectRelease(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnGetLootEffect(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private GameObject SpawnLootEffect()
    {
        GameObject looteffect = Instantiate(lootEffectPrefab);

        return looteffect;
    }

    public void OnReleaseLootEffect(GameObject looteffect)
    {
        lootEffectPool.Release(looteffect);
    }
}
