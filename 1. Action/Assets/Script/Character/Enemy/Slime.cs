using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;

#if UNITY_EDITOR
using UnityEditor;      // UNITY_EDITOR라는 전처리기가 설정되어있을 때만 실행버전에 넣어라
#endif

[RequireComponent(typeof(Rigidbody))]       // 필수적으로 필요한 컴포넌트가 있을 때 자동으로 넣어주는 유니티 속성
[RequireComponent(typeof(Animator))]
public class Slime : MonoBehaviour, IHealth, IBattle
{
    /// <summary>
    /// 적이 순찰할 웨이포인트
    /// </summary>
    public WayPoints waypoints;
    public float moveSpeed = 3.0f;      // 적의 이동 속도

    Transform wayPointTarget;               // 지금 적이 이동할 목표 지점

    EnemyState state = EnemyState.Patrol;                   // 현재 적의 상태(대기 상태 or 순찰 상태)
    public float waitTime = 1.0f;       // 목적지에 도착했을 때 기달리는 시간
    float waitTimer;                    // 남아있는 기다려야 하는 시간

    // 몬스터 스탯 관련 변수 -----------------------------------------------------
    public float maxHP = 100.0f;        // 최대 HP 
    public float attackPower = 10.0f;   // 공격력
    public float defencePower = 3.0f;   // 방어력
    float hp = 100.0f;                  // 현재 HP
    float attackSpeed = 1.0f;           // 1초마다 공격
    float attackCoolTime = 1.0f;        // 쿨타임이 0 미만이 되면 공격
    IBattle attackTarget;
    ParticleSystem dieEffect;           // 죽을 때표시될 이펙트
    // -------------------------------------------------------------------------

    // 추적 관련 변수 ------------------------------------------------------
    public float sightRange = 10.0f;                // 시야 범위
    public float sightHalfAngle = 50.0f;            // 시야각의 절반
    public float closeSightRange = 2.5f;
    Transform chaseTarget;                          // 추적할 플레이어의 트랜스폼
    // --------------------------------------------------------------------

    // 아이템 드랍용 데이터 -------------------------------------------------------
    [Serializable]
    public struct ItemDropInfo            // 드랍 아이템 정보
    {
        public ItemIDCode id;             // 아이템 종류

        [Range(0.0f, 1.0f)]
        public float dropPercentage;      // 아이템 드랍 확률
    }

    public ItemDropInfo[] dropItems;      // 이 몬스터가 드랍할 아이템의 종류

    Animator anima;
    NavMeshAgent agent;
    SphereCollider badycollider;
    Rigidbody rigid;


    /// <summary>
    /// 적의 상태를 나타내기 위한 enum
    /// </summary>
    protected enum EnemyState
    {
        Wait = 0,       // 대기 상태
        Patrol,         // 순찰 상태
        Chase,          // 추적 상태
        Attack,         // 공격 상태
        Dead            // 사망 상태
    }

    /// <summary>
    /// 상태별 업데이트 함수를 가질 델리게이트
    /// </summary>
    Action StateUpdate;

    /// <summary>
    /// 이동할 목적지(웨이 포인트)를 나타내는 프로퍼티
    /// </summary>
    protected Transform WayPointTarget
    {
        get => wayPointTarget;
        set
        {
            wayPointTarget = value;
        }
    }

    /// <summary>
    /// 적의 상태를 나타내는 프로퍼티
    /// </summary>
    protected EnemyState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                //switch (state)      // 이전 상태(상태를 나가면서 해야할 일 처리
                //{
                //    case EnemyState.Wait:
                //        break;
                //    case EnemyState.Patrol:
                //        break;
                //    default:
                //        break;
                //}

                state = value;
                switch (state)      // 새로운 상태(새로운 상태로 들어가면서 해야할 일 처리
                {
                    case EnemyState.Wait:
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        waitTimer = waitTime;           // 타이머 초기화
                        anima.SetTrigger("Stop");       // Idle 애니메이션 재생
                        StateUpdate = Update_Wait;      // FixedUpdate에서 실행될 델리게이트 변경
                        break;
                    case EnemyState.Patrol:
                        agent.isStopped = false;
                        agent.SetDestination(wayPointTarget.position);
                        anima.SetTrigger("Move");       // Move 애니메이션 재생
                        StateUpdate = Update_Patrol;    // FixedUpdate에서 실행될 델리게이트 변경
                        break;
                    case EnemyState.Chase:
                        agent.isStopped = false;
                        anima.SetTrigger("Move");       // Move 애니메이션 재생
                        StateUpdate = Update_Chase;     // FixedUpdate에서 실행될 델리게이트 변경
                        break;
                    case EnemyState.Attack:
                        agent.isStopped = true;         // 이동 정지
                        agent.velocity = Vector3.zero;
                        anima.SetTrigger("Stop");       // 애니메이션 변경
                        attackCoolTime = attackSpeed;   // 공격 쿨타임 초기화
                        StateUpdate = Updata_Attack;    // FixedUpdate에서 실행될 델리게이트 변경
                        break;
                    case EnemyState.Dead:
                        agent.isStopped = true;         // 길찾기 중지
                        agent.velocity = Vector3.zero;
                        anima.SetTrigger("Die");        // 사망 애니메이션 재생
                        StartCoroutine(DeadRepresent());// 사망 연출 코루틴 실행(서서이 가라앉는 연출)

                        StateUpdate = Update_Dead;      // FixedUpdate에서 실행될 델리게이트 변경
                        break;
                    default:
                        break;
                }
            }
        }
    }


    /// <summary>
    /// 남은 대기 시간을 나타내는 프로퍼티
    /// </summary>
    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if (waypoints != null && waitTimer < 0)      // 남은 시간이 다 되면
            {
                State = EnemyState.Patrol;      // Patrol 상태로 전환
            }
        }
    }

    public float HP
    {
        get => hp;
        set
        {
            if (hp != value)
            {
                if (hp > value)
                {
                    hp = value;
                    if (State != EnemyState.Dead && hp < 0)
                    {
                        Die();
                    }
                }

                hp = Mathf.Clamp(hp, 0.0f, maxHP);

                onHealthChange?.Invoke(hp / maxHP);
            }
        }
    }

    public float MaxHP => maxHP;


    public float AttackPower => attackPower;

    public float DefencePower => defencePower;

    /// <summary>
    /// HP가 변경될 때 실행될 델리게이트
    /// </summary>
    public Action<float> onHealthChange { get; set; }

    /// <summary>
    /// 죽었을 때 실행될 델리게이트
    /// </summary>
    public Action onDie { get; set; }

    private void Awake()
    {
        anima = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        badycollider = GetComponent<SphereCollider>();
        dieEffect = GetComponentInChildren<ParticleSystem>();
        rigid = GetComponent<Rigidbody>();

        EnmeyAttackArea attackArea = GetComponentInChildren<EnmeyAttackArea>();
        attackArea.onPlayerIn += (target) =>
        {
            if (State == EnemyState.Chase)         // 추적 상태이면
            {
                attackTarget = target;
                State = EnemyState.Attack;         // 공격 상태로 변경
            }
        };

        attackArea.onPlayerOut += (target) =>
        {
            if(attackTarget == target)
            {
                attackTarget = null;        // 공격 하던 대상이 범위를 벗어나면 공격 대상을 비우기
                if (State != EnemyState.Dead)
                {
                    State = EnemyState.Chase;       // 플레이어가 공격 범위에서 벗어나면 다시 추적 상태로
                }
            }
        };
    }

    private void Start()
    {
        agent.speed = moveSpeed;
        if (waypoints != null)      // waypoints 가 없을 때를 대비한 코드
        {
            WayPointTarget = waypoints.Current;
        }
        else
        {
            WayPointTarget = transform;
        }

        // 값 초기화 작업
        State = EnemyState.Wait;      // 기본 상태 설정(wait)
        anima.ResetTrigger("Stop");     // 트리거가 쌓이는걸 방지

        // 테스트 코드
        onHealthChange += Test_HP_Change;
        onDie += Test_Die;
    }

    private void FixedUpdate()
    {
        if (State != EnemyState.Dead && State != EnemyState.Attack && SearchPlayer())     // 매번 추적대상을 찾기
        {
            State = EnemyState.Chase;       // 추적 대상이 있으면 추적 상태로 변경
        }

        StateUpdate();
    }

    /// <summary>
    /// Patrol 상태일 떄 실행될 업데이트 함수
    /// </summary>
    void Update_Patrol()
    {
        // 도착 확인
        // agent.pathPending : 경로 계산이 진행중인지 확인. true면 아직 경로 계산중
        // agent.remainingDistance : 도착지점까지 남아있는 거리
        // agent.remainingDistance : 도착지점에 도착했다고 인정되는 거리
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)    // 경로 계산이 완료 됐고
        {
            WayPointTarget = waypoints.MoveNext();
            State = EnemyState.Wait;
        }
    }

    /// <summary>
    ///  Wiat 상태일 때 실행될 업데이트 함수
    /// </summary>
    void Update_Wait()
    {
        WaitTimer -= Time.fixedDeltaTime;       // 시간 지속적으로 감소
    }

    /// <summary>
    /// Chase 상태일 때 실행될 업데이트 함수
    /// </summary>
    void Update_Chase()
    {
        if (chaseTarget != null)        // 추적 대상이 있는지 확인
        {
            agent.SetDestination(chaseTarget.position);     // 추적 대상이 있으면 추적 대상의 위치로 이동
        }
        else
        {
            State = EnemyState.Wait;            // 추적 대상이 없으면 잠시 대기
        }
    }

    /// <summary>
    /// Updata 상태일 때 실행될 업데이트 함수
    /// </summary>
    private void Updata_Attack()
    {
        attackCoolTime -= Time.fixedDeltaTime;          // 쿨타임 감소
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attackTarget.transform.position - transform.position), 0.1f); // 공격 대상 바라보게 만들기
        if(attackCoolTime < 0)                  // 쿨타임 체크
        {
            // 공격하고
            anima.SetTrigger("Attack");         // 공격 애니메이션 재생
            Attact(attackTarget);               // 공격 처리
        }
    }

    /// <summary>
    /// Dead 상태일 때 실행될 업데이트 함수
    /// </summary>
    void Update_Dead()
    {

    }

    /// <summary>
    /// 플레이어를 감지하는 함수
    /// </summary>
    /// <returns>적이 플레이어를 감지하면 true, 아니면 false</returns>
    bool SearchPlayer()
    {
        bool result = false;
        chaseTarget = null;

        // 특정 범위안에 존재하는지 확인
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)
        {
            // player가 몬스터 주변에 있다.
            Vector3 playerPos = colliders[0].transform.position;            // 플레이어 위치
            Vector3 toPlayerDir = playerPos - transform.position;           // 플레이어로 가는 방향

            if( toPlayerDir.sqrMagnitude < closeSightRange * closeSightRange)       // 근접 시야 범위 안에 있는지 확인
            {
                // 근접 시야 범위 안에 player가 있음
                chaseTarget = colliders[0].transform;           // 추척할 플레이어 저장
                result = true;
            }
            else
            {
                if (IsInSightAngle(toPlayerDir))
                {
                    // player가 시야각 안에 들어왔다.
                    if (!IsSightBlocked(toPlayerDir))
                    {
                        // 시야가 다른 물체로 인해 막히이 않았다.
                        chaseTarget = colliders[0].transform;   // 추적할 플레이어 저장
                        result = true;
                    }
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 대상이 시야각안에 들어와 있는지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDir">대상으로 가는 방향 벡터</param>
    /// <returns>대상이 있다면 true</returns>
    bool IsInSightAngle(Vector3 toTargetDir)
    {
        float angle = Vector3.Angle(transform.forward, toTargetDir);    // forward 벡터와 플레이어로 가는 방향 벡터의 시야각 구하기
        return sightHalfAngle > angle;
    }

    /// <summary>
    /// 플레이어를 바라보는 시야가 막혔는지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDir">대상으로 가는 방향 벡터</param>
    /// <returns>방해하는 물건이 없다면 false</returns>
    bool IsSightBlocked(Vector3 toTargetDir)
    {
        bool result = true;
        // 레이 만들기 : 시점점 = 적의 위치 + 적의 눈높이, 방항 = 적에서 플레이어로 가는 방향
        Ray ray = new(transform.position + transform.up * 0.5f, toTargetDir);
        if (Physics.Raycast(ray, out RaycastHit hit, sightRange))
        {
            // 레이에 부딪친 컬라이더가 있다.
            if (hit.collider.CompareTag("Player"))
            {
                // 컬라이더가 player면
                result = false;
            }
        }
        return result;
    }

    public void Test()
    {
        SearchPlayer();
    }

    void Test_HP_Change(float ratino)
    {
        UnityEngine.Debug.Log($"{gameObject.name}이 피해를 받았습니다. 현재 HP : {hp}");
    }

    void Test_Die()
{
        UnityEngine.Debug.Log($"{gameObject.name}이 죽었습니다. ");
    }


    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        //Gizmos.DrawWireSphere(transform.position, sightRange);
        Vector3 forward = transform.forward * sightRange;

        Handles.DrawLine(transform.position, transform.position + forward);         // 몬스터의 앞 방향

        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);         // 시야 반경만큼 원 그리기

        if (SearchPlayer())             // 플레이어가 보이는지 여부에 따라 색상 지정
        {
            Handles.color = Color.red;          // 보이면 빨간색
        }

        Quaternion q1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up);        // up벡터를 축으로 반시계방향으로 회전
        Quaternion q2 = Quaternion.AngleAxis(sightHalfAngle, transform.up);         // up벡터를 축으로 시계방향으로 회전

        Handles.DrawLine(transform.position, transform.position + q1 * forward, 5.0f);    // 중심선을 반시계방향으로 회전
        Handles.DrawLine(transform.position, transform.position + q2 * forward, 5.0f);    // 중심선을 시계방향으로 회전

        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfAngle * 2, sightRange, 5.0f);      // 호 그리기

        // 근접시야 처리
        Handles.color = new Color(255, 127, 0);        
        Handles.DrawWireDisc(transform.position, transform.up, closeSightRange);
#endif
    }

    public void Die()
    {
        State = EnemyState.Dead;
        onDie?.Invoke();
        MakeDropItem();
    }

    /// <summary>
    /// 사망 연출용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DeadRepresent()
    {
        dieEffect.transform.parent = null;
        Enemy_HP_Bar hpBar = GetComponentInChildren<Enemy_HP_Bar>();
        Destroy(hpBar.gameObject);
        badycollider.enabled = false;   // 컬라이더 컴포넌트 끄기

        yield return new WaitForSeconds(1.0f);
        dieEffect.Play();               // 사망 이펙트 재생    

        yield return new WaitForSeconds(1.5f);
        agent.enabled = false;          // 네브메쉬 에이전트 컴포넌트 끄기
        rigid.isKinematic = false;      // 키네마틱 끄기
        rigid.drag = 10.0f;             // 마찰력 10으로 설정

        yield return new WaitForSeconds(1.5f);
        Destroy(dieEffect.gameObject);
        Destroy(this.gameObject);
    }

    public void Attact(IBattle target)
    {
        target?.Defence(attackPower);
        attackCoolTime = attackSpeed;       // 쿨타임 초기화
    }

    public void Defence(float damage)
    {
        if (State != EnemyState.Dead)
        {
            anima.SetTrigger("Hit");
            HP -= (damage - DefencePower);
        }
    }

   void MakeDropItem()
    {
        float percentage = UnityEngine.Random.Range(0.0f, 1.0f);        // 드랍할 아이템을 결정하기 위한 랜덤 숫자 가져오기
        int slect = 0;      // 드랍할 (내가 가지고 있는) 아이템의 인덱스
        float max = 0;      // 가장 드랍할 확률이 높은 아이템을 찾기 위한 임시값
        for (int i = 0; i < dropItems.Length; i++)
        {
            if(max < dropItems[i].dropPercentage)
            {
                max = dropItems[i].dropPercentage;      // 가장 드랍 확률이 높은 아이템 찾기
                slect = i;                              // slect의 디폴트 값은 가장 드랍 확률이 높은 아이템
            }
        }

        float checkPercentage = 0.0f;                   // 아이템의 드랍 확률을 누적하는 임식 값
        for (int i = 0; i < dropItems.Length; i++)
        {
            checkPercentage += dropItems[i].dropPercentage;     // checkPercentage를 단계별로 계속 누적
            if (percentage <= checkPercentage)                  // checkPercentage와 percentage 비교 (랜덤 숫자가 누적된 확률보다 낮은지 확인, 낮으면 해당 아이템 생성)
            {
                slect = i;          // 생성할 아이템 결정
                break;              // for문 종료
            }
        }

        GameObject obj = ItemFactory.MakeItem(dropItems[slect].id, transform.position);     // 선택된 아이템 생성
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (State != EnemyState.Dead)
        {
            // 드랍 아이템의 드랍 확률의 합을 1로 만들기
            float total = 0.0f;
            foreach (var item in dropItems)
            {
                total += item.dropPercentage;           // 전체 합 구하기
            }

            if (total > 0)          // 안걸러주면 NaN 뜸
            {
                for (int i = 0; i < dropItems.Length; i++)
                {
                    dropItems[i].dropPercentage /= total;   // 전체 합으로 나누어서 최종합을 1로 만들기
                }
            }
        }
    }
#endif
}
