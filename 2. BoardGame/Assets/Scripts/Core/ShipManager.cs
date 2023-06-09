using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ShipManager : Singleton<ShipManager>
{
    /// <summary>
    /// 함선의 프리팹 (모델 정보만 있는 프리펩)
    /// </summary>
    public GameObject shipPrefab;

    /// <summary>
    /// 모델 정보만 있는 프리펩
    /// </summary>
    public GameObject[] shipModels;

    /// <summary>
    /// 함선의 머티리얼, (0번째 : 보통 상황용, 1번째 : 배치모드인 상황용)
    /// </summary>
    public Material[] shipMaterials;

    /// <summary>
    /// 배치모드 머티리얼 사용할 색상 정보 (배치 가능용)
    /// </summary>
    readonly Color ssuccessColor = new Color(0, 1, 0, 0.2f);

    /// <summary>
    /// 배치모드 머티리얼에서 사용할 색상 정보 (배치 불가능용) 
    /// </summary>
    readonly Color failColor = new Color(1, 0, 0, 0.2f);

    /// <summary>
    /// 배의 종류 가지 수
    /// </summary>
    int shipTpyeCount;

    /// <summary>
    /// 배의 방향 가지수
    /// </summary>
    int shipDirectionCount;

    /// <summary>
    /// 배의 이름들
    /// </summary>
    readonly public string[] shipnames = { "항공모함", "전함", "구축함", "잠수함", "정비정" };

    // 프로퍼티 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 배의 종류가 몇 가지인지 확인하기 위한 프로퍼티, 읽기 전용
    /// </summary>
    public int ShipTpyeCount => shipTpyeCount;

    /// <summary>
    /// 배가 바라볼 수 있는 방향의 가지수를 확인하기 위한 프로퍼티, 읽기 전용
    /// </summary>
    public int ShipDirectionCount => shipDirectionCount;

    /// <summary>
    /// 배가 보통 상황일 때 사용하는 머티리얼을 읽기 위한 프로퍼티, 읽기 전용
    /// </summary>
    public Material NormalShipMaterial => shipMaterials[0];

    /// <summary>
    /// 배가 배치 모드일 때 사용하는 머티리얼을 읽기 위한 프로퍼티, 읽기 전용
    /// </summary>
    public Material DeployModeShipMaterial => shipMaterials[1];

    // 함수 ------------------------------------------------------------------------------------------

    /// <summary>
    /// 기본 초기화 작업
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
        shipTpyeCount = Enum.GetValues(typeof(ShipType)).Length -1;             // 자주 사용할 변수들 미리 저장하기
        shipDirectionCount = Enum.GetValues(typeof(ShipDirection)).Length;
    }

    /// <summary>
    /// 실제로 함성 생성하는 함수
    /// </summary>
    /// <param name="type">생성할 배의 종류</param>
    /// <param name="ownerTransform">생성한 배를 가지는 플레이어의 트랜스폼</param>
    /// <returns>생성 완료된 배</returns>
    public Ship MakeShip(ShipType type, Transform ownerTransform )
    {
        GameObject shipObj = Instantiate( shipPrefab, ownerTransform);  // 함선 생성

        GameObject modelPrefab = GetShipModel(type);                    // 함선의 모델 생성해서 함선에 붙이기
        Instantiate(modelPrefab, shipObj.transform);                    

        Ship ship = shipObj.GetComponent<Ship>();                       // 함선의 초기화 작업
        ship.Initialize(type);

        ship.gameObject.name = $"{type}_{ship.Size}";                   // 함선 게임 오브젝트 이름 변경
        ship.gameObject.SetActive(false);                               // 생성한 함선이 안보이게 만들기

        return ship;
    }

    /// <summary>
    /// 함선의 모델 프리팹을 리턴하는 함수
    /// </summary>
    /// <param name="type">리턴할 함선의 종류</param>
    /// <returns>요구받은 함선의 프리팹</returns>
    private GameObject GetShipModel(ShipType type)
    {
        return shipModels[(int)type-1];
    }

    /// <summary>
    /// 배치 모드일 때 배의 머티리얼 색상을 지정하는 함수
    /// </summary>
    /// <param name="isSuccess">true면 successColor 적용, false면 failColor 적용</param>
    public void SetDeployModeColor(bool isSuccess)
    {
        if( isSuccess)
        {
            DeployModeShipMaterial.SetColor("_BaseColor", ssuccessColor);
        }
        else
        {
            DeployModeShipMaterial.SetColor("_BaseColor", failColor);
        }
    }
}
