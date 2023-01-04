using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum WeaponName
{
    sword,
    axe,
    pistol,
    shotgun,
    laser_rifle
}


public class PlayerWeaponChanger : MonoBehaviour
{
    public List<GameObject> weaponsList = new List<GameObject>();
    [SerializeField] PlayerProjectile[] PlayerProjectiles;
    public int weaponIndex;

    public TextMeshProUGUI fpsText;
    public float deltaTime;
    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();

        if (Input.GetKeyDown(KeyCode.D))
        {
            //sword chosen
            Sword();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //axe chosen
            Axe();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //pistol chosen
            Pistol();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //rifle 1 chosen

            Shotgun();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //rifle 2 chosen

            Rifle();
        }


    }

    public void setWeapon(WeaponName weaponname)
    {
        switch(weaponname)
        {
            case WeaponName.laser_rifle:
                Rifle();
                break;
            case WeaponName.pistol:
                Pistol();
                break;
            case WeaponName.shotgun:
                Pistol();
                break;
            case WeaponName.axe:
                Pistol();
                break;
            case WeaponName.sword:
                Pistol();
                break;
        }

    }

    public void Rifle()
    {
        PlayerProjectilePool.Instance.ProjectilePrefab = PlayerProjectiles[2];
        PlayerProjectilePool.Instance.projectilePool.Clear();
        AudioSourceManager.Instance.PlayDrawGunSfx(0);


        PlayerAttacking.Instance.animIndex = 0;
        weaponIndex = 4;
        WeaponChange(weaponIndex);
        PlayerAttacking.Instance.damage = PlayerAttacking.Instance.projectile[4].stats.damage;
        PlayerAttacking.Instance.pushAmount = PlayerAttacking.Instance.projectile[4].stats.pushAmount;
        PlayerAttacking.Instance.attackRange = PlayerAttacking.Instance.projectile[4].stats.range;
    }

    public void Shotgun()
    {
        PlayerProjectilePool.Instance.ProjectilePrefab = PlayerProjectiles[1];
        PlayerProjectilePool.Instance.projectilePool.Clear();
        AudioSourceManager.Instance.PlayDrawGunSfx(2);


        PlayerAttacking.Instance.animIndex = 0;
        weaponIndex = 3;
        WeaponChange(weaponIndex);
        PlayerAttacking.Instance.damage = PlayerAttacking.Instance.projectile[3].stats.damage;
        PlayerAttacking.Instance.pushAmount = PlayerAttacking.Instance.projectile[3].stats.pushAmount;
        PlayerAttacking.Instance.attackRange = PlayerAttacking.Instance.projectile[3].stats.range;
    }

    public void Pistol()
    {
        PlayerProjectilePool.Instance.ProjectilePrefab = PlayerProjectiles[0];
        PlayerProjectilePool.Instance.projectilePool.Clear();
        AudioSourceManager.Instance.PlayDrawGunSfx(1);

        PlayerAttacking.Instance.animIndex = 2;
        weaponIndex = 2;
        WeaponChange(weaponIndex);
        PlayerAttacking.Instance.damage = PlayerAttacking.Instance.projectile[2].stats.damage;
        PlayerAttacking.Instance.pushAmount = PlayerAttacking.Instance.projectile[2].stats.pushAmount;
        PlayerAttacking.Instance.attackRange = PlayerAttacking.Instance.projectile[2].stats.range;
    }

    public void Axe()
    {
        AudioSourceManager.Instance.PlayDrawGunSfx(3);

        PlayerAttacking.Instance.animIndex = 1;
        weaponIndex = 1;
        WeaponChange(weaponIndex);
        PlayerAttacking.Instance.damage = PlayerAttacking.Instance.projectile[1].stats.damage;
        PlayerAttacking.Instance.pushAmount = PlayerAttacking.Instance.projectile[1].stats.pushAmount;
        PlayerAttacking.Instance.attackRange = PlayerAttacking.Instance.projectile[1].stats.range;
    }

    public void Sword()
    {
        AudioSourceManager.Instance.PlayDrawGunSfx(4);

        PlayerAttacking.Instance.animIndex = 1;
        weaponIndex = 0;
        WeaponChange(weaponIndex);
        PlayerAttacking.Instance.damage = PlayerAttacking.Instance.projectile[0].stats.damage;
        PlayerAttacking.Instance.pushAmount = PlayerAttacking.Instance.projectile[0].stats.pushAmount;
        PlayerAttacking.Instance.attackRange = PlayerAttacking.Instance.projectile[0].stats.range;
    }

    public void WeaponChange(int index)
    {
        for (int i = 0; i < weaponsList.Count; i++)
        {
            if(i == index)
            {
                weaponsList[i].gameObject.SetActive(true);
            }
            else
            {
                weaponsList[i].gameObject.SetActive(false);

            }
        }
    }
}
