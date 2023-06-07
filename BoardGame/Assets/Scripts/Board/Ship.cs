using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.Mesh;

public class Ship : MonoBehaviour
{
    // 변수 ---------------------------------------------------------------------

    /// <summary>
    /// 배의 이름
    /// </summary>
    string shipName;

    /// <summary>
    /// 배의 종류, 배의 크기 및 최대 HP결정
    /// </summary>
    ShipType type = ShipType.None;

    /// <summary>
    /// 배가 바라보는 바향, 기본적으로 북동남서 순서가 정방향 회전 순서
    /// </summary>
    ShipDirection direction = ShipDirection.North;

    /// <summary>
    /// 배의 크기, 초기화할 때 배의 종류에 맞게 설정된다.
    /// </summary>
    int size = 0;

    /// <summary>
    /// 배의 현재 HP. 초기화할 때 배의 크기에 맞게 설정되고 0이 되면 침몰
    /// </summary>
    int hp = 0;

    /// <summary>
    /// 배의 생존 여부, 기본값 true
    /// </summary>
    bool isAlive = true;

    /// <summary>
    /// 배의 배치 여부, 기본값 false
    /// </summary>
    bool isDeployed = false;

    /// <summary>
    /// 배의 모델 색상 변경 용
    /// </summary>
    Renderer shipRenderer = null;

    /// <summary>
    /// 배가 배치된 위치, 배의 각 칸들의 위치
    /// </summary>
    Vector2Int[] positions = null;

    /// <summary>
    /// 배의 모델 부분의 트랜스폼
    /// </summary>
    Transform model;

    PlayerBase owner;

    // 델리게이트 ---------------------------------------------------------

    /// <summary>
    /// 함선이 배치되거나 배치 해제가 되었을 때 실행되는 델리게이트
    /// 파라메터 : 배치할 때 ture, 배치 해제할 때 false
    /// </summary>
    public Action<bool> onDeploy;

    /// <summary>
    /// 함선이 공격을 당했을 때 실행될 델리게이트
    /// 파라메터 : 자기 자신
    /// </summary>
    public Action<Ship> onHit;

    /// <summary>
    /// 함선이 침몰했을 떄 실행될 델리게이트
    /// 파라메터 : 자기 자신
    /// </summary>
    public Action<Ship> onSinking;

    // 프로퍼티 -----------------------------------------------------------

    /// <summary>
    /// 배 이름 확인용 프로퍼티, 일기 전용
    /// </summary>
    public string ShipName => shipName;

    /// <summary>
    /// 배 종류 확인용 프로퍼티, 읽기 전용
    /// </summary>
    public ShipType Type => type;

    /// <summary>
    /// 배의 방향 확인용  프로퍼티, 읽기 전용
    /// </summary>
    public ShipDirection Direction
    {
        get => direction;
        set
        {
            direction = value;
            model.rotation = Quaternion.Euler(0, (int)direction * 90.0f, 0.0f);
        }
    }

    /// <summary>
    /// 배의 크기 확용 프로퍼티, 읽기 전용, 배의 종류에 따라 결정됨
    /// </summary>
    public int Size => size;
    //{
    //    get
    //    {
    //        switch (type)
    //        {
    //            
    //        }
    //    }
    //}

    /// <summary>
    /// 배의 현재 HP 확인용 프로퍼티, 읽기 전용
    /// </summary>
    public int HP
    {
        get => hp;
        private set
        {
            hp = value;
            if(hp <= 0 && IsAlive)  // HP가 0 이하인데 살아있으면
            {
                OnSinking();        // 함선 침몰
            }
        }
    }

    /// <summary>
    /// 배의 생존 여부 확인용 프로퍼티, 읽기 전용
    /// </summary>
    public bool IsAlive => isAlive;

    /// <summary>
    /// 배의 배치 여부 확인요 프로퍼티, 읽기 전용
    /// </summary>
    public bool IsDeployed => isDeployed;

    /// <summary>
    /// 배의 각 칸별 위치 확인용 프로퍼티, 읽기 전용
    /// </summary>
    public Vector2Int[] Positions => positions;

    public PlayerBase Owner => owner;

    ///// <summary>
    ///// 배의 랜더러 접근용 프로퍼티, 읽기 전용
    ///// </summary>
    //public Renderer ShipRenderer => shipRenderer;

    // 함수 -------------------------------------------------------------------------------

    /// <summary>
    /// 배 생성 직후에 각종 초기화 작업을 하기 위한 함수
    /// </summary>
    /// <param name="shipType">이 배의 타입</param>
    public void Initialize(ShipType shipType)
    {

        type = shipType;
        switch (type)
        {
            case ShipType.Carrier:         // 항공모한은 5칸
                size = 5;
                break;
            case ShipType.Battleship:   // 전함은 4칸
                size = 4;
                break;
            case ShipType.Destroyer:    // 구축함은 3칸
                size = 3;
                break;
            case ShipType.Submarine:    // 잠수함은 3칸
                size = 3;
                break;
            case ShipType.PatrolBoat:   // 정비정은 2칸
                size = 2;
                break;
            default:
                break;
        }


        shipName = ShipManager.Inst.shipnames[(int)type -1];     // 함선 이름 설정
       
        model = transform.GetChild(0);
        shipRenderer = model.GetComponentInChildren<Renderer>();       // ShipManager에서 프리펩이 모두 만들어진 타이밍에서 실행시켜야 함

        owner = GetComponentInParent<PlayerBase>();

        // 모든 함선 공통
        RestData();
    }

    void RestData()
    {
        hp = size;      // HP는 크기를 그대로 사용

        Direction = ShipDirection.North;
        isAlive = true;
        isDeployed = false;
        positions = null;
        isDeployed = false;


    }

    /// <summary>
    /// 함선의 머티리얼을 선택하는 함수
    /// </summary>
    /// <param name="isNormal">true면 normal 머티리얼, false면 deploy 머티리얼</param>
    public void SetMaterialType(bool isNormal = true)
    {
        if (isNormal)
        {
            shipRenderer.material = ShipManager.Inst.NormalShipMaterial;
        }
        else
        {
            shipRenderer.material = ShipManager.Inst.DeployModeShipMaterial;
        }
    }

    /// <summary>
    /// 함선이 배치될 때 실행되는 함수
    /// </summary>
    /// <param name="deployPosition">배치되는 위치들</param>
    public void Deploy(Vector2Int[] deployPosition)
    {
        positions = deployPosition;     // 배치된 위치 기록
        isDeployed = true;              // 배치 되었다고 표시
        onDeploy?.Invoke(true);         // 배치 되었다고 알림
    }

    /// <summary>
    /// 함선이 배치 해제 되었을 때 실행되는 함수
    /// </summary>
    public void UnDeploy()
    {
        RestData();
        onDeploy?.Invoke(false);
    }

    /// <summary>
    /// 함선을 90도 씩 회전 시키는 함수
    /// </summary>
    /// <param name="isCCW">true면 반시계 방향으로 회전, false면 시계 반향으로 회전</param>
    public void Rotate(bool isCCW)
    {
        int count = ShipManager.Inst.ShipDirectionCount;
        if(isCCW)
        {
            Direction = (ShipDirection)(((int)direction + count - 1) % count);
        }
        else
        {
            Direction = (ShipDirection)(((int)direction + 1) % count); ;
        }
    }

    /// <summary>
    /// 함선을 랜덤한 방향으로 회전시키는 함수
    /// </summary>
    public void RandomRotate()
    {
        int rotateCount = UnityEngine.Random.Range(0, ShipManager.Inst.ShipDirectionCount);     // 바라볼 방향 결정
        bool isCCw = UnityEngine.Random.Range(0, 2) == 0;       // 시계방향 / 반시계반향 고르기
        for (int i = 0; i < rotateCount; i++)
        {
            Rotate(isCCw);      // 바라볼 방향을 향해 회전
        }
    }

    /// <summary>
    /// 함선이 공격 받았을 때 실행되는 함수
    /// </summary>
    public void OnAttacked()
    {
        Debug.Log($"{type}이 공격 받음");
        HP--;
        if(IsAlive)
        {
            onHit?.Invoke(this);        // 함선이 데미지를 입어다고 알림
        }
    }

    /// <summary>
    /// 함선이 침몰 했을 때 실행되는 함수
    /// </summary>
    private void OnSinking()
    {
        Debug.Log($"{type}이 침몰했씁니다.");
        isAlive = false;            // 함선이 침몰했다고 길록
        onSinking?.Invoke(this);    // 함선이 침몰했다고 알림
    }
}
