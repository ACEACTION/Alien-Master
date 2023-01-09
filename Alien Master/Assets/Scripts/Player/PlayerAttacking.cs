using System.Collections;
using UnityEngine;
using DG.Tweening;


public class PlayerAttacking : MonoBehaviour
{
    [SerializeField] Animator activeAnim;
    public bool isAttacking;
    
    [SerializeField] RuntimeAnimatorController[] anims;
    [SerializeField] float attackCd;
    [SerializeField] LayerMask layermask;
    [SerializeField] float attackCdAmount;
    public ParticleSystem lootEffect;
    [SerializeField] float offset;
    [SerializeField] int shotgunBulletCounter;
    [SerializeField] float rotateSpeed;
    public PlayerProjectile[] projectile;
    [SerializeField] Transform[] bulletSpawnPoint;
    [SerializeField] PlayerWeaponChanger weaponcChanger;
    public GameObject closestEnemy;
    RaycastHit hit;
    public float pushAmount;
    public float attackRange;

    public int animIndex;
    public float damage;
    public int weaponIndex;
    public static PlayerAttacking Instance;
    public bool canDealDmg;

    Enemy targetEnemy;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        attackCd = attackCdAmount;

    }

    private void Update()
    {
        if (CheckDied())
            return;

        weaponIndex = weaponcChanger.weaponIndex;
        Attacking();

        attackCd -= Time.deltaTime;
        activeAnim.runtimeAnimatorController = anims[animIndex];
    }

    bool CheckDied() 
    {
        return PlayerHealth.Instance.died;
    }

    private void Attacking()
    {
        for (int i = 0; i < EnemyManager.enemiesList.Count; i++)
        {
            bool isHit = Physics.Raycast(transform.position + new Vector3(0, 1, 0),
                EnemyManager.enemiesList[i].transform.position - transform.position + new Vector3(0, 1f, 0),
                out hit, 200f, layermask);

            if (isHit && hit.transform.CompareTag("Enemy"))
            {
                //check if enemy is in range
                if (Vector3.Distance(EnemyManager.enemiesList[i].transform.position, transform.position) < attackRange)
                {
                    closestEnemy = EnemyManager.enemiesList[i].gameObject;
                    if (attackCd <= 0)
                    {
                        transform.DOLookAt(closestEnemy.transform.position, 0f);
                        
                        activeAnim.SetBool("Attacking", true);
                        attackCd = attackCdAmount;
                    }

                }
            }
        }
    }

    public void DisableAttack()
    {
        if (animIndex == 1)
            canDealDmg = false;

        activeAnim.SetBool("Attacking", false);

    }

     public IEnumerator Fire()
    {
        if (closestEnemy != null)
        {
            

            transform.DOLookAt(closestEnemy.transform.position, 0.05f);
            //for melee attack
            if(animIndex == 1)
                closestEnemy.GetComponent<EnemyHealth>().MinusHp(damage);

            if (animIndex == 1)
            {
                CameraShake.Instance.ShakeCamera(0.2f, 1f, 0.5f);
                canDealDmg = true;
            }

            yield return new WaitForSeconds(0.05f);
            if (weaponIndex == 2 || weaponIndex == 4)
            {
                var projectile = PlayerProjectilePool.Instance.projectilePool.Get();
                projectile.transform.position = bulletSpawnPoint[weaponIndex].position;
                projectile.direction = (closestEnemy.transform.position - transform.position).normalized;

                if (weaponIndex == 2)
                {
                    AudioSourceManager.Instance.PlayPistolSfx();
                }
                else if (weaponIndex == 4)
                {
                    AudioSourceManager.Instance.PlayLaserGunSfx();
                }


            }
            else if (weaponIndex == 3)
            {
                for (int i = 0; i < shotgunBulletCounter; i++)
                {
                    var projectile = PlayerProjectilePool.Instance.projectilePool.Get();
                    projectile.transform.position = bulletSpawnPoint[weaponIndex].position;
                    projectile.direction = ((closestEnemy.transform.position - transform.position) + new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), Random.Range(0, offset))).normalized;
                }
                AudioSourceManager.Instance.PlayShotGunSfx();
            }                   
        }
            
        PlayerMovement.Instance.moveSpeed = PlayerMovement.Instance.moveSpeedDefault;
        PlayerMovement.Instance.rotateSpeed = PlayerMovement.Instance.rotateSpeedDefault;
    }
    //private void OnDrawGizmos()
    //{
    //    for (int i = 0; i < EnemyManager.enemiesList.Count; i++)
    //    {
    //        bool isHit = Physics.Raycast(transform.position + new Vector3(0, 1, 0),
    //            (EnemyManager.enemiesList[i].transform.position - transform.position) + new Vector3(0, 1, 0),
    //            out hit, 200f, layermask);

    //        if (isHit && hit.transform.CompareTag("Enemy"))
    //        {
    //            Gizmos.color = Color.green;
    //            //check if enemy is in range
    //            if (Vector3.Distance(EnemyManager.enemiesList[i].transform.position, transform.position) < attackRange)
    //            {?
    //                Gizmos.color = Color.yellow;
    //            }
    //        }
    //        else
    //            Gizmos.color = Color.red;

    //        Gizmos.DrawLine(transform.position + new Vector3(0, 1, 0), EnemyManager.enemiesList[i].transform.position + new Vector3(0, 1, 0));
    //    }
    //}

}


