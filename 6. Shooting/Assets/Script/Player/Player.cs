
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.XR.Haptics;

public class Player : MonoBehaviour
{
    //public delegate void DelegateName();        // 이런 종류의 델리게이트가 있다. (리턴없고 파라메터도 없는 함수를 저장하는 델리게이트)

    //public DelegateName del;    // DelegateName 타입으로 del이라는 이름의 델리게이트를 만듬
    //Action del2;                // 리턴타입이 void, 파라메터도 없는 델리게이트 del2를 만듬
    //Action<int> del3;           // 리턴타입이 void, 파라메터는 int 하나인 델리게이트 del3를 만듬
    //Func<int, float> del4;      // 리턴타입이 int고 파라메터는 float 하나인 델리게이트 del4를 만듬

    // Awake > OnEnble > Start : 대체적으로 이 순서
    private PlayerinputAction inputActions; // InputSystem용 입력 액션
    private Rigidbody2D rigid;
    private Animator anim;
    private Collider2D bodyCollider;
    private SpriteRenderer sprite;
    private Transform firePositionRoot;     // 총알이 발사될 위치와 회전을 가지고 있는 트랜스 폼
    private AudioSource ShootAudio;

    [Header("프리펩")]
    private GameObject flash;               // 총알이 발사될 때 보일 플래시 이팩트 게임 오브젝트
    public GameObject BulletPrefab;         // 총알용 프리펩
    public GameObject explosionPrefab;      // 폭팔 프리펩

    Vector3 dir;                            // 이동 방향(입력에 따라 변경됨)
    //Vector3[] fireRot;

    IEnumerator fireCoroutine;              // 총알 연사용 코루틴

    private bool isDead = false;            // 플레이어의 사망여부(true면 사망 false면 생존)
    private bool isInvincbleMode = false;   // 무적 상태인지 표시용(true면 on false off)
    //bool isFire = false;

    private int life;                       // 현재 생명수
    private int power = 0;                  // 파워업을 아이템을 획득한 갯수(최대값 = 3)
    public int initialLife = 3;             // 초기 생명 개수
    public int totalScore = 0;             // 플레이어가 획득한 점수
    private int extraPowerBouns = 100;


    private float boost = 1.0f;             // 부스트 속도(부스트 상태에 들어가면 2, 보통 상태일 때는 1)
    private float timeElapsed = 0.0f;       // 무적상태에 들어간 후의 경과 시간(의 30배)
    private float fireAngle = 30.0f;        // 총알이 한번에 여러발 발사될 때 총알간의 사이각도
    public float fireInterval = 0.5f;       // 총알 발사 시간간격
    public float Speed = 1.0f;              // 플레이어의 이동 속도(초당 이동 속도)
    private const float InvincbleTime = 2.0f;       // 피격시 무적 시간;
    //float fireTimeCount = 0.0f;

    // 델리 게이트 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public Action<int> onLifeChange;
    public Action<int> onScoreChange;

    // 프로퍼티ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    /// <summary>
    /// 생명갯수 용 프로퍼티. 0~3 사이의 값을 가진다
    /// </summary>
    private int Life            // 프로퍼티
    {
        get => life;
        set
        {
            if (life != value && !isDead)   // 값에 변경이 일어났다. 그리고 살아있다.
            {
                Power--;

                if (life > value)
                {
                    // life가 감소한 상황( 새로운 값(value)이 옛날 값(life)보다 작다 => 감소했다)
                    StartCoroutine(EnterIncibleMode());
                }

                life = value;

                if (life <= 0)      // 비교범위는 가능한 크게 잡는 쪽이 안전하다. 
                {
                    life = 0;
                    Dead();
                }
            }
            // (변수명)?. : 왼족 변수가 null이면 null.   null이 아니면 (변수명) 맴버에 접근
            onLifeChange?.Invoke(life); // 라이프가 변경될 때 onLifeChange 델리게이트에 등록된 함수들을 실행시킨다.
        }
    }

    /// <summary>
    /// 공격력 용 프로퍼티. 1~3 사이의 값으 가진다. 한번에 발사하는 총알의 숫자와 같다.
    /// </summary>
    private int Power
    {
        get => power;
        set
        {
            power = value;          // 들어온 값으로 파워 설정
            if (power > 3)          // 파워가 3을 벗어나면 3을 제한
            {
                AddScore(extraPowerBouns);
                power = 3;
            }
            if (power < 1)
            {
                power = 1;
            }

            // 기존에 있는 파이어 포지션 제거
            while (firePositionRoot.childCount > 0)                  // 자식이 0보다 많으면....
            {
                Transform temp = firePositionRoot.GetChild(0);      // temp에 firePositionRoot 첫번째 자식을 넣어라
                temp.parent = null;                                 // temp에 부모을 버려라
                Destroy(temp.gameObject);                           // 자기 자신을 지워라
            }

            // 파워 등급에 맞게 새로 배치
            for (int i = 0; i < power; i++)
            {
                GameObject firePos = new GameObject();          // 빈 오브젝트 생성하기
                firePos.name = $"FirePosition_{i}";             // firePos에 이름을 바꿈
                firePos.transform.parent = firePositionRoot;    // fiePos를 firePositionRoot에 자식으로 만듦
                firePos.transform.position = firePositionRoot.transform.position;       // firePos의 위치를 firePositionRoot위치로 바꿈

                // power가 1일때 : 0도
                // power가 2일때 : -15도, +15도
                // power가 3일때 : -30도, 0도, +30도
                firePos.transform.rotation = Quaternion.Euler(0, 0, (power - 1) * (fireAngle * 0.5f) + i * -fireAngle);
                firePos.transform.Translate(1.0f, 0, 0);            // 자기 자신을 x축으로 1.0f만큼 이동해라
            }
        }
    }


    // 입력 처리용 함수 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    /// <summary>
    /// 이동 부스트 발동 해제용 입력 처리용 (Shift 땠을 때)
    /// </summary>
    private void OffBooster(InputAction.CallbackContext context)
    {
        boost = 1.0f;       // 이동 속도에 계산에들 어가는 계수를 1로 변경
    }

    /// <summary>
    /// 이동 부스트 발동용 입력 처리용 (Shift 눌렀을 때)
    /// </summary>
    /// <param name="context"></param>
    private void OnBooster(InputAction.CallbackContext context)
    {
        boost *= 2.0f;  // 이동 속도에 계산에들 어가는 계수를 2로 변경
    }

    /// <summary>
    /// 이동 입력 처리용 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        // Exception : 예외 상황( 무엇을 해야 할지 지정이 안되어있는 예외 일때 )
        //throw new NotImplementedException();    // NotImplementedException 을 실행해라. => 코드 구현을 알려주기 위해 강제로 죽이는 코드
        Vector2 inputdir = context.ReadValue<Vector2>();    // 어느 방향으로 움직여야 하는지를 입력받음
        dir = inputdir;
        //Debug.Log("이동 입력");

        //dir.y > 0     // W를 눌렀다
        //dir.y == 0 // w,s 중 아무것도 안눌렀다,
        //dir.y < 0 // s를 눌렀다,
        anim.SetFloat("InputY", dir.y);
    }

    /// <summary>
    /// 총알 발사 시작 입력 처리용 (Space를 눌렀을 때)
    /// </summary>
    /// <param name="context"></param>
    private void OnFireStart(InputAction.CallbackContext context)
    {
        //Debug.Log("발사");
        //float value = Random.Range(0.0f, 10.0f);      // value에는 0.0 ~ 10.0 의 랜덤값이 들어간다.
        //Instantiate(Bullet, transform.position, Quaternion.identity);
        //isFire = true;
        StartCoroutine(fireCoroutine);  // 코루틴 실행
    }

    /// <summary>
    /// 총알 발사 중지 입력 처리용 (Space를 땠을 때)
    /// </summary>
    /// <param name="context"></param>
    private void OnFireStop(InputAction.CallbackContext context)
    {
        //isFire = false;
        //StopAllCoroutines();
        StopCoroutine(fireCoroutine);       // 코루틴 정지
    }

    // 유니티 이벤트 함수 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 생성된 직후에 호출
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerinputAction();     // 액션맵 인스턴스 생성
        rigid = GetComponent<Rigidbody2D>();        // 한번만 찾고 저장해서 계속 쓰기(메모리 더 쓰고 성능 아끼기)
        anim = GetComponent<Animator>();
        bodyCollider = GetComponent<Collider2D>();  // CapsuleCollider2D가 Collider2D의 자식이라서 가능
        sprite = GetComponent<SpriteRenderer>();

        fireCoroutine = Fire();     // 연사용 코루틴 저장

        firePositionRoot = transform.GetChild(0);   // 발사 트랜스폼 찾기
        flash = transform.GetChild(1).gameObject;   // flash 가져오기
        flash.SetActive(false);                     // flash 비활성화

        ShootAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 활성화 되었을때 호출
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Enable();   // 오브젝트가 생성되면 입력을 받도록 활성화
        inputActions.Player.Move.performed += OnMove;   // performed일 때 Onmove 함수 실행하도록 연결
        inputActions.Player.Move.canceled += OnMove;    // canceled일 때 Onmove 함수 실행하도록 연결
        inputActions.Player.Fire.performed += OnFireStart;
        inputActions.Player.Fire.canceled += OnFireStop;
        inputActions.Player.Booster.performed += OnBooster;
        inputActions.Player.Booster.canceled += OffBooster;
    }

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 비활성화 되었을 때 호출
    /// </summary>
    private void OnDisable()
    {
        InputDisable(); // 입력도 비호라성화
    }


    /// <summary>
    /// 충돌이 발생했을 때 실행.(충동한 순간)
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionEnter2D");        // Collider와 부딪쳤을 때 실행
        if (collision.gameObject.CompareTag("PowerUp"))
        {

            // 파워업 아이템을 먹었으면
            Power++;                                // 파워 증가
            Destroy(collision.gameObject);          // 파워업 오브젝트 삭제
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            //if(isDead == false)
            //Dead();     // 적이랑 부딪치면 죽이기

            Life--; // 적이랑 부딪치면 life가 1 감소한다.


        }
    }

    /// <summary>
    /// 시작할 때. 첫번째 Update 함수가 실행되기 직전에 호출.
    /// </summary>
    private void Start()
    {
        Power = 1;      // 시잘할 때 파워를 1로 설정(발싸 위치 갱신용)
        Life = initialLife; // 생명숫자도 초기화
        totalScore = 0;     // 점수 초기화
        AddScore(0);        // ui 갱신용
    }

    /// <summary>
    /// 매 프레임마다 호출.
    /// </summary>
    private void Update()
    {
        if (isInvincbleMode)        // 무적 상태용 코드
        {
            timeElapsed += Time.deltaTime * 30.0f;          // 시간의 30배 누적
            float alpha = (Mathf.Cos(timeElapsed) + 1.0f) * 0.5f;       // cos의 결과를 1~0으로 변경
            sprite.color = new Color(1, 1, 1, alpha);       // 알파값 변경
        }
    }

    /// <summary>
    /// 일정 시간 간격(물리 업데이트 시간 간격)으로 호출
    /// </summary>
    private void FixedUpdate()
    {
        if (!isDead)
        {
            //transform.position += (Speed * Time.fixedDeltaTime * dir);
            // 이 스크립트 파일이 들어 있는 게임 오브젝트에서 Rigiboody2D 컴포넌트를 찾아 리턴.(없으면 null)
            // 그런데 GetComponent는 무거운 함수 => (Update나 FixedUpdate처럼 주기적 또는 자주 호촐되는 함수 안에서는 안쓰는 것이 좋다
            //rigid = GetComponent<Rigidbody2D>();

            // rigid.AddForce(Speed * Time.fixedDeltaTime * dir); // 관성이 있는 움직임을 할 때 유용
            rigid.MovePosition(transform.position + boost * Speed * Time.fixedDeltaTime * dir);     // 관성없는 움직임을 처리할 때 유용
                                                                                                    //fireTimeCount += Time.fixedDeltaTime;
                                                                                                    //if(isFire && fireTimeCount > fireInterval)
                                                                                                    //{
                                                                                                    //    Instantiate(Bullet, transform.position, Quaternion.identity);
                                                                                                    //    fireTimeCount = 0.0f;
                                                                                                    //}
        }
        else
        {
            rigid.AddForce(Vector2.left * 0.1f, ForceMode2D.Impulse);       // 죽었을 때 연출용. 뒤로 돌면서 튕겨나가기
            rigid.AddTorque(10.0f);
        }
    }



    /// <summary>
    /// 총알 연속 발사용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Fire()
    {
        //yield return null;      // 다음 프레임에 이어서 시작해라

        //yield return new WaitForSeconds(1.0f);      // 1초 후에 이어서 시작해라

        while (true)
        {
            for (int i = 0; i < firePositionRoot.childCount; i++)
            {
                // Bullet이라는 프리팹을 firePosition[i]의 위치에 (0,0,0) 회전으로 만들어라
                Instantiate(BulletPrefab, firePositionRoot.GetChild(i).position, firePositionRoot.GetChild(i).rotation);

                // Instantiate(생성할 프리팹);        // 프리팹이 (0,0,0) 위치에 (0,0,0) 회전에 (1,1,1) 스케일로 만드러짐
                // Instantiate(생성할 프리팹, 생성할 위치, 생성될 때의 회전);

                //obj.transform.Rotate(fireRot[i]);
                //obj.transform.rotation = firePosition[i].rotation;  // 총알의 회전 값으로 firePosition[i]의 회전값을 그래도 사용한다.

                //Vector3 angle = firePosition[i].rotation.eulerAngles; // 현재 회전 값을 x,y,z축별로 몇도씩 회전 했는지 확인 가능
            }
            ShootAudio.Play();
            flash.SetActive(true);      // flash 켜기
            StartCoroutine(Flashoff()); // 0.1초 후에 flash를 끄는 코루틴 실행
            yield return new WaitForSeconds(fireInterval);      // 총알 발사 시간 간격만큼 대기
        }
    }

    /// <summary>
    /// 0.1초 후에 flash를 끄는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Flashoff()
    {
        yield return new WaitForSeconds(0.1f);      // 0.1초 대기
        flash.SetActive(false);     // flash 끄기
    }

    /// <summary>
    /// 충돌 막고, 무적 모드 설정, 타이머 초기화를 진행한 후 InvincbleTime초 후에 다시 원상 복구
    /// </summary>
    /// <returns></returns>
    IEnumerator EnterIncibleMode()
    {
        //bodyCollider.enabled = false;       // 충돌이 안일어나게 만들기
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        isInvincbleMode = true;             // 무적모드 켜기
        timeElapsed = 0.0f;                 // 타이머 초기화

        yield return new WaitForSeconds(InvincbleTime);     // 무적시간 동안 대기

        isInvincbleMode = false;            // 무적모드 끄기
        if (!(Life <= 0))
        {
            //bodyCollider.enabled = true;        // 살아있을 때만 충돌이 다시 발생하게 만들기
        }
        gameObject.layer = LayerMask.NameToLayer("Player");
        sprite.color = Color.white;         // 원래 색으로 되돌리기
    }

    // 함수(매서드) ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    /// <summary>
    /// 플레이어가 죽었을 때 실행될 일들
    /// </summary>
    private void Dead()
    {
        isDead = true;          // 죽었다고 표시
        GetComponent<Collider2D>().enabled = false;             // 콜라이더를 비활성화
        gameObject.layer = LayerMask.NameToLayer("Player");     // 죽었을 때 플레이어로 원상 복구
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);      //폭팔 이팩트 생성
        InputDisable();                     // 입력 막기
        rigid.gravityScale = 1.0f;          // 중력으로 떨어지게 만들기
        rigid.freezeRotation = false;       // 회전 막아놓은 것 풀기
        StopCoroutine(fireCoroutine);       // 총을 쏘던 중이면 더이상 쏘지 않게 처리
    }

    /// <summary>
    /// 입력 막기. 모든 액션맵을 비활성화 하고 입력 이벤트에 인결된 함수들 제거
    /// </summary>
    private void InputDisable()
    {
        inputActions.Player.Move.performed -= OnMove;   // 연결해 놓은 함수 해제(안전을 위해)
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Fire.performed -= OnFireStart;
        inputActions.Player.Booster.performed -= OnBooster;
        inputActions.Player.Booster.canceled -= OffBooster;
        inputActions.Player.Fire.canceled -= OnFireStop;
        inputActions.Player.Disable();  // 오브젝트가 사라질때 더 이상 입력을 받지 않도록 비활성화
    }

    public void AddScore(int score)
    {
        totalScore += score;
        onScoreChange?.Invoke(totalScore);
    }

}