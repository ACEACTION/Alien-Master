using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Projectile_stats")]
public class PlayerProjectileStats : ScriptableObject
{
    public GameObject hitEffect;
    public GameObject muzzleEffect;

    public int damage;
    public int defaultDamage;
    public int speed;
    public float range;
    public int pushAmount;

}
