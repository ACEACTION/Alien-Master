using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float currentHp;
    Vector3 pushBackDir;

    // references    
    [SerializeField] Rigidbody rb;
    [SerializeField] int coinAmount;
    [SerializeField] bool coinGenerator = true;
    [SerializeField] Collider enemyCol;
    [SerializeField] GameObject enemyModel;
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject fowObj;
    [SerializeField] GameObject gunObj;
    [SerializeField] ParticleSystem bloodEff;

    private void Awake()
    {
        currentHp = enemy.data.maxHp;
        bloodEff.Stop();

        SetRigidbodyState(true);
        SetColliderState(false);

        // parent components
        enemyCol.enabled = true;
        rb.isKinematic = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            CameraShake.Instance.ShakeCamera(.2f, 1, 1f);
    }

    void SetRigidbodyState(bool state)
    {
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidBodies)
            rb.isKinematic = state;
    }

    void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
            col.enabled = state;
    }

    //public void TakeDamage(float dmg)
    //{
    //    MinusHp(dmg);
    //}

    public void MinusHp(float dmg)
    {
        currentHp -= dmg;
        PlayerAttacking.Instance.canDealDmg = false;
        // spawn blood
        HitBlood();

        Alert.Instance.EnemyAlertToNearEnemies(transform.position, enemy);
        if (currentHp <= 0)
        {
            DieProcess();
            
            // I dont need enemy pushed when explosed so I call below method when enemy taked dmg
            PushBackEnemy();            
        }
    }

    void HitBlood()
    {
        //var blood = Instantiate(enemy.data.bloodEff, transform.position, Quaternion.identity);
        //Destroy(blood, 3);

        //bloodEff.Play();

        EnemyHitBlood blood = EnemyHitBloodPool.Instance.pool.Get();
        //blood.transform.SetParent(transform);
        //blood.transform.localPosition = new Vector3(0, 1, 0);
        blood.transform.position = transform.position;
        blood.bloodEff.Play();

    }

    void PushBackEnemy()
    {
        pushBackDir = (PlayerAttacking.Instance.gameObject.transform.position - transform.position);
        rb.AddForce(-pushBackDir * PlayerAttacking.Instance.pushAmount);
    }

    void DieProcess()
    {

        AudioSourceManager.Instance.PlayEnemyBloodsSfx();

        // remove from enemies list
        //EnemyManager.enemiesList.Remove(enemy);
        enemy.RemoveFromManagerList();

        // add to dead enemies list
        //EnemyManager.deadEnemies.Add(gameObject);

        // disable game objects
        fowObj.SetActive(false);

        Invoke("ChangeLayer", 0);
        //ChangeLayer();

        //GunProcess();

        // active ragdoll
        SetRigidbodyState(false);
        SetColliderState(true);

        // deactive ragdoll after while time
        StartCoroutine(SetRagdollStateWithDelay(true, false));

        enemy.DieProcess();
        DisableComponents();

        // because enemy doesnt push and we must to disable with delay 
        StartCoroutine(DisableColWithDelay());

        GameManager.Instance.DoSlowMotion();
        
        MakeCoin();

    }



    void MakeCoin()
    {
        if (coinGenerator)
        {
            coinGenerator = false;
            for (int i = 0; i < enemy.data.coinCount; i++)
            {
                var coin = PlayerProjectilePool.Instance.coinPool.Get();
                coin.transform.position = transform.position;

            }
           // AudioSourceManager.Instance.PlayCoinSfx();

        }
    }

    IEnumerator SetRagdollStateWithDelay(bool rbState, bool colState)
    {
        yield return new WaitForSeconds(4f);
        SetRigidbodyState(rbState);
        SetColliderState(colState);
    }

    void ChangeLayer()
    {
        string deadEnemyTag = "Dead Enemy";
        int layer = LayerMask.NameToLayer("Dead Enemy");
        Transform[] enemyObjs = GetComponentsInChildren<Transform>();
        foreach (Transform obj in enemyObjs)
        {
            obj.gameObject.layer = layer;
            obj.gameObject.tag = deadEnemyTag;
        }
        GunProcess();
    }

    void GunProcess()
    {
        gunObj.transform.SetParent(null);
        gunObj.GetComponent<Rigidbody>().isKinematic = false;
        gunObj.GetComponent<Collider>().enabled = true;
    }

    void DisableComponents()
    {
        enemy.agent.enabled = false;
        enemy.anim.enabled = false;
        enemy.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player Weapon") && PlayerAttacking.Instance.canDealDmg)
        {
            //MinusHp(PlayerAttacking.Instance.damage);
            //PlayerAttacking.Instance.canDealDmg = false;
        }

        if (collision.gameObject.CompareTag("Player Weapon"))
        {
            //PlayerProjectilePool.Instance.projectilePool.Release()
            PlayerProjectilePool.Instance.onReleaseProjectile(collision.gameObject.GetComponent<PlayerProjectile>());

        }

    }

    IEnumerator DisableColWithDelay()
    {
        yield return new WaitForSeconds(.5f);
        enemyCol.enabled = false;
    }

    public void Exploded()
    {
        HitBlood();
        DieProcess();
        rb.constraints = RigidbodyConstraints.None;
    }
}
