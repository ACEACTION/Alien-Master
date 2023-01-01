using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{    
    public float maxHp;
    public float damage;
    public float maxAtckCd;
    public float maxWonderingTime;
    public float maxIdleRotTime;
    public float maxFindingPlayerTime;
    public float maxAlertStandingTime;
    public float maxAlertPatrollingTime;
    public GameObject bloodEff;
    public int coinCount;
    public float randomPosOffset;
}
