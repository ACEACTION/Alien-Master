using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public enum EnemyState
{
    idle,
    wondering,
    attacking,
    patrolling,
    findingPlayer,
    backToPrevEnemyState,
    alert,
    alertPatrolling,
    dead
}

public class Enemy : MonoBehaviour
{

    [Header("General")]
    [SerializeField] EnemyState enemyState;
    EnemyState previousState;

    [Header("Movement")]
    [SerializeField] Transform[] patrollingPoints;
    [SerializeField] bool idleRot;
    int patrollingPointsIndex;
    float idleRotCd;
    Vector3 dstPos;
    Vector3 originPos;
    float patrollingStandingCd;

    [Header("Wondering")]
    float wonderingCd;

    [Header("Attack")]    
    [SerializeField] ParticleSystem muzzleEfx;
    float attackingCd;

    [Header("Finding Player")]
    Vector3 playerPos;
    float findingPlayerCd;

    [Header("Alert")]
    float alertPatrollingCd;
    Vector3 alertPos;
    float alertStandingCd;
    bool alertToOtherEnemy;


    [Header("References")]
    public EnemyData data;
    public NavMeshAgent agent;    
    public FieldOfView fow;
    public Animator anim;
    [SerializeField] GameObject wonderingMark;

    private void Awake()
    {
        muzzleEfx.Stop();        
    }
    private void Start()
    {       
        EnemyManager.enemiesList.Add(this);
        Init();
    }

    void Init()
    {
        alertStandingCd = data.maxAlertStandingTime;
        idleRotCd = data.maxIdleRotTime;
        wonderingCd = data.maxWonderingTime;
        attackingCd = data.maxAtckCd;
        findingPlayerCd = data.maxFindingPlayerTime;
        alertPatrollingCd = data.maxAlertPatrollingTime;
        patrollingStandingCd = data.maxPatrollingStandingCd;

        if (enemyState == EnemyState.patrolling) SetAgentDst(patrollingPoints[0].position);
    }

    private void Update()
    {
        Move();
        CheckState();
    }

    void CheckState()
    {
        SetEnemyWonderingState();

        switch (enemyState)
        {
            case EnemyState.idle:
                SetIdle();
                break;
            case EnemyState.patrolling:
                SetPatrolling();
                break;
            case EnemyState.wondering:
                SetWondering();
                break;
            case EnemyState.attacking:                
                SetAttacking();
                break;
            case EnemyState.findingPlayer:
                FindingPlayer();
                break;
            case EnemyState.backToPrevEnemyState:
                BackToPrevState();
                break;
            case EnemyState.alert:
                GoToAlert();
                break;
            case EnemyState.alertPatrolling:
                AlertPatrolling();
                break;
        }
    }    
    
    void Move()
    {        
        agent.SetDestination(dstPos);
    }

    void SetIdle()
    {
        IdleProcess();
        SetEnemyPreviousState();
    }

    private void IdleProcess()
    {
        SetWalkAnimationState(false);
        SetFiringAniamtionState(false);

        SetAgentDst(transform.position);

        if (idleRot)
        {
            RandomIdleRotation();
        }
    }
    
    void SetPatrolling()
    {
        PatrollingProcess();
        SetEnemyPreviousState();
    }

    private void PatrollingProcess()
    {
        SetWalkAnimationState(true);
        if (ArriveToPoint())
        {
            patrollingStandingCd -= Time.deltaTime;
            IdleProcess();
            if (patrollingStandingCd <= 0)
            {                
                patrollingStandingCd = data.maxPatrollingStandingCd;
                CheckPatrollingPointIndex();
                SetAgentDst(patrollingPoints[patrollingPointsIndex].position);
            }
        }
    }

    void CheckPatrollingPointIndex()
    {
        patrollingPointsIndex++;
        if (patrollingPointsIndex >= patrollingPoints.Length)
            patrollingPointsIndex = 0;
    }

    void SetEnemyWonderingState()
    {
        if (IsPlayer() && enemyState != EnemyState.attacking)
        {
            SetEnemyState(EnemyState.wondering);
        }        
    }

    void SetWondering()
    {
        SetWalkAnimationState(false);
        SetWonderingMarkState(true);
        SetAgentDst(transform.position);

        if (IsPlayer())
        {
            LookAtPlayer();
            playerPos = fow.player.position;
        }

        wonderingCd -= Time.deltaTime;
        if (wonderingCd < 0)
        {
            if (IsPlayer())
            {
                SetEnemyState(EnemyState.attacking);
                SetWonderingMarkState(false);
                alertToOtherEnemy = true;
            }
            else
            {
                findingPlayerCd = data.maxFindingPlayerTime;
                SetEnemyState(EnemyState.findingPlayer);
            }

            wonderingCd = data.maxWonderingTime;
        }

        //if (IsPlayer())
        //{
        //    SetWalkAnimationState(false);
        //    SetWonderingMarkState(true);
        //    SetAgentDst(transform.position);
        //    LookAtPlayer();

        //    wonderingCd -= Time.deltaTime;
        //    if (wonderingCd < 0)
        //    {                
        //        SetWonderingMarkState(false);
        //        SetEnemyState(EnemyState.attacking);
        //        wonderingCd = data.maxWonderingTime;
        //        alertToOtherEnemy = true;
        //    }
        //    else
        //    {
        //        playerPos = fow.player.position;
        //        //SetEnemyState(EnemyState.findingPlayer);
        //    }
        //}
    }

    void SetAttacking()
    {
        if (IsPlayer())
        {
            if (alertToOtherEnemy)
            {
                alertToOtherEnemy = false;
                Alert.Instance.EnemyAlertToNearEnemies(GetRandomPos(fow.player.position), this);
            }

            SetFiringAniamtionState(true);
            LookAtPlayer();
            SetAgentDst(transform.position);
            playerPos = fow.player.position;

            attackingCd -= Time.deltaTime;
            if (attackingCd < 0)
            {
                attackingCd = data.maxAtckCd;
                DmgToPlayerProcess();
            }
        }
        else
        {               
            SetFiringAniamtionState(false);
            SetEnemyState(EnemyState.findingPlayer);
            findingPlayerCd = data.maxFindingPlayerTime;
        }
    }    

    private void DmgToPlayerProcess()
    {
        PlayerHealth.Instance.TakeDamage(data.damage);
        MakeHitBlood();
    }

    private void MakeHitBlood()
    {
        muzzleEfx.Play();

        PlayerHealth.Instance.takeDmg = true;
        GameObject hit = EnemyHitPool.Instance.pool.Get();
        hit.transform.position = transform.position;
        hit.transform.LookAt(fow.player.transform.position);
        hit.transform.position = fow.player.position + new Vector3(0, 1, 0);
    }

    void FindingPlayer()
    {
        SetAgentDst(playerPos);
        SetWonderingMarkState(true);
        SetWalkAnimationState(true);

        if (ArriveToPoint())
        {
            idleRot = true;
            IdleProcess();
        }

        findingPlayerCd -= Time.deltaTime;
        if (findingPlayerCd < 0)
        {
            findingPlayerCd = data.maxFindingPlayerTime;
            SetEnemyState(EnemyState.backToPrevEnemyState);
        }
    }

    void BackToPrevState()
    {
        SetAgentDst(originPos);
        SetWalkAnimationState(true);
        SetWonderingMarkState(false);

        if (ArriveToPoint())
            ResetEnemyState();
    }

    void GoToAlert()
    {
        SetAgentDst(alertPos);
        SetWonderingMarkState(true);

        if (ArriveToPoint())
        {
            SetAgentDst(transform.position);
            SetWalkAnimationState(false);
            idleRot = true;
            IdleProcess();
        }
        else
        {
            SetWalkAnimationState(true);
        }

        CheckSuspicion();

        alertStandingCd -= Time.deltaTime;
        if (alertStandingCd <= 0)
        {
            alertStandingCd = data.maxAlertStandingTime;
            SetEnemyState(EnemyState.backToPrevEnemyState);
        }       

    }

    void CheckSuspicion()
    {
        if (IsDeadEnemy())
        {
            Alert.Instance.EnemyAlertPatrolling(this);
        }
    }

    public void AlertPatrolling()
    {
        SetWonderingMarkState(true);
        PatrollingProcess();
        idleRot = true;

        CheckSuspicion();

        alertPatrollingCd -= Time.deltaTime;
        if (alertPatrollingCd <= 0)
        {
            alertPatrollingCd = data.maxAlertPatrollingTime;
            SetEnemyState(EnemyState.backToPrevEnemyState);
        }
    }



    public void SetAlert(Vector3 alertPos)
    {
        if (enemyState == EnemyState.attacking) return;
        this.alertPos = GetRandomPos(alertPos);
        SetEnemyState(EnemyState.alert);
    }

    
    Vector3 GetRandomPos(Vector3 center)
    {
        Vector3 randomVector = new Vector3(
            Random.Range(center.x - data.randomPosOffset, center.x + data.randomPosOffset), 
            center.y,
            Random.Range(center.z - data.randomPosOffset, center.z + data.randomPosOffset));

        return (randomVector);
    }

    void ResetEnemyState() => enemyState = previousState;    
    void SetEnemyPreviousState()
    {
        previousState = enemyState;        
        originPos = transform.position;
    }
    void RandomIdleRotation()
    {
        idleRotCd -= Time.deltaTime;
        if (idleRotCd < 0)
        {
            transform.DORotate(new Vector3(0, Random.Range(-180, 180), 0), 2f);
            idleRotCd = data.maxIdleRotTime;
        }
    }

    // deactive some elements in this sc
    public void DieProcess()
    {
        SetWonderingMarkState(false);
    }

    bool ArriveToPoint() 
    { 
        return Vector3.Distance(transform.position, dstPos) <= 2f;        
    }    
    bool IsPlayer() { return fow.player; }
    private void LookAtPlayer()
    {
        transform.DOLookAt(
            new Vector3(fow.player.position.x, 0, fow.player.position.z), .5f);
    }
    public void SetAlertPatrollingState() => SetEnemyState(EnemyState.alertPatrolling);
    void SetAgentDst(Vector3 pos) => dstPos = pos;
    void SetWalkAnimationState(bool state) => anim.SetBool("walk", state);    
    void SetFiringAniamtionState(bool state) => anim.SetBool("firing", state);    
    void SetEnemyState(EnemyState state) => enemyState = state;    
    void SetWonderingMarkState(bool state) => wonderingMark.SetActive(state);
    bool IsDeadEnemy()
    {
        return fow.deadEnemy && !IsPlayer();
    }

    public void RemoveFromManagerList()
    {
        EnemyManager.enemiesList.Remove(this);
    }

}
