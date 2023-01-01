using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    // sfx
    [SerializeField] AudioClip[] enemyBloodsSfx;
    [SerializeField] AudioClip[] gunDrawSfx;
    [SerializeField] AudioClip lootSound;
    [SerializeField] AudioClip shotgunSfx;
    [SerializeField] AudioClip loseSfx;
    [SerializeField] AudioClip laserSfx;
    [SerializeField] AudioClip pistolSfx;

    // audio source
    [SerializeField] AudioSource audioScr;

    // singleton
    public static AudioSourceManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void PlayEnemyBloodsSfx()
    {
        audioScr.PlayOneShot(enemyBloodsSfx[Random.Range(0, enemyBloodsSfx.Length)]);
    }

    public void PlayDrawGunSfx(int index)
    {
        audioScr.PlayOneShot(gunDrawSfx[index]);
    }
    public void PlayShotGunSfx()
    {
        audioScr.PlayOneShot(shotgunSfx);
    }

    public void PlayCoinSfx()
    {
        audioScr.PlayOneShot(lootSound);
    }
        public void PlayLoseSfx()
    {
        audioScr.PlayOneShot(loseSfx);
    }

    public void PlayLaserGunSfx()
    {
        audioScr.PlayOneShot(laserSfx);
    }

    public void PlayPistolSfx()
    {
        audioScr.PlayOneShot(pistolSfx);
    }


}
