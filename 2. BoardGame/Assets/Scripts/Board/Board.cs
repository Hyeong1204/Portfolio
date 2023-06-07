using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 보드의 가로 세로 길이 (항상 같은 크기)
    /// </summary>
    public const int BoardSize = 10;

    /// <summary>
    /// 보드의 배 배치 정보. 2차원 대신 1차원으로 저장
    /// </summary>
    ShipType[] shipInfo = null;

    /// <summary>
    /// 공격 당한 위치 정보. 공격을 했으면 true, 안햇으면 false
    /// </summary>
    bool[] bombInfo;

    /// <summary>
    /// 공격당한 위치를 시각적으로 보여주는 클래스. (성공(O), 실패(X), 그냥 표시(검은구))
    /// </summary>
    BombMark bombMark;

    /// <summary>
    /// 개발용 배 배치 정보를 보여줄지 여부. true면 보여주고, false면 보여주지 않는다
    /// </summary>
    public bool isShowShipDeploymentInfo = true;

    /// <summary>
    /// 개발용 배 배치 정보 생성용 클래스
    /// </summary>
    ShipDeploymentinfoMaker shipDeploymentInfo = null;

    public const int NOT_VALID = -1;

    // 델리게이트 ------------------------------------------------------------------

    public Dictionary<ShipType, Action> onShipAttacked;

    private void Awake()
    {
        shipInfo = new ShipType[BoardSize * BoardSize];     // shipInfo 초기화(none으로 초기화 됨)
        bombInfo = new bool[BoardSize * BoardSize];

        bombMark = GetComponentInChildren<BombMark>();

        onShipAttacked = new Dictionary<ShipType, Action>(ShipManager.Inst.ShipTpyeCount);
        onShipAttacked[ShipType.None] = null;       // ShipType.None 부분은 무조건 없음
        onShipAttacked[ShipType.Carrier] = null;
        onShipAttacked[ShipType.Battleship] = null;
        onShipAttacked[ShipType.Destroyer] = null;
        onShipAttacked[ShipType.Submarine] = null;
        onShipAttacked[ShipType.PatrolBoat] = null;

        if (isShowShipDeploymentInfo)
        {
            shipDeploymentInfo = GetComponentInChildren<ShipDeploymentinfoMaker>();
        }
    }

    /// <summary>
    /// 배열의 인덱스 값을 그리드 좌표로 변환해주는 static함수
    /// </summary>
    /// <param name="index">계산할 인덱스 값</param>
    /// <returns>변환된 그리드 좌표</returns>
    public static Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % BoardSize, index / BoardSize);
    }

    /// <summary>
    /// 그리드 좌표를 배열의 인덱스 값으로 변환해주는 static 함수
    /// </summary>
    /// <param name="grid"></param>
    /// <returns>변화된 인덱스 값</returns>
    public static int GridToIndex(Vector2Int grid)
    {
        if (IsValidPosition(grid))
        {
            return (grid.y * BoardSize) + grid.x;
        }

        return NOT_VALID;
    }

    /// <summary>
    /// 그리드 좌표를 배열의 인덱스 값으로 변환해주는 static 함수
    /// </summary>
    /// <param name="x">계산할 그리드 x좌표</param>
    /// <param name="y">계산할 그리드 y좌표</param>
    /// <returns>변화된 인덱스 값</returns>
    public static int GridToIndex(int x, int y)
    {
        return (y * BoardSize) + x;
    }

    /// <summary>
    /// 특정 위치가 보드 안인지 밖인지 확인하는 함수
    /// </summary>
    /// <param name="gridPos">확인할 위치</param>
    /// <returns>true면 보드 안쪽 false면 보드 밖</returns>
    public static bool IsValidPosition(Vector2Int gridPos)
    {
        return gridPos.x > -1 && gridPos.x < BoardSize && gridPos.y > -1 && gridPos.y < BoardSize;
    }

    // 일반 함수 ----------------------------------------------------------------------------------

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변환해주는 함수
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector3 GridToWorld(int x, int y)
    {
        return transform.position + new Vector3(x + 0.5f, 0, -(y + 0.5f));
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변환해주는 함수
    /// </summary>
    /// <param name="grid">계산할 그리드 좌표</param>
    /// <returns>변환된 인덱스 값</returns>
    public Vector3 GridToWorld(Vector2Int grid)
    {
        return GridToWorld(grid.x, grid.y);
    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변환해주는 함수
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        worldPos.y = 0;
        Vector3 diff = worldPos - transform.position;

        return new Vector2Int(Mathf.FloorToInt(diff.x), Mathf.FloorToInt(-diff.z));
    }

    /// <summary>
    /// 인덱스 값을 월드 좌표로 변환 해주는 함수
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector3 IndexToWorld(int index)
    {
        return GridToWorld(IndexToGrid(index));
    }

    /// <summary>
    /// 현재 마우스의 위치를 그리드 좌표로 변경해서 리턴
    /// </summary>
    /// <returns>현재 마우스의 그리드 좌표</returns>
    public Vector2Int GetMouseGridPosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();              // 현재 마우스 위치 가져오기
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);        // 마우스 위치를 월드 좌표로 변환

        return WorldToGrid(worldPos);                                       // 월드 좌표를 다시 그리드 좌표로 변겨해서 돌려주기
    }

    /// <summary>
    /// 특정 월드 좌표에 어떤 종류의 배가 배치되어 있는지 알려주는 함수
    /// </summary>
    /// <param name="worldPos">확인할 월드 좌표</param>
    /// <returns>해당 위치에 있는 배의 종류</returns>
    public ShipType GetShipType(Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);
        int index = GridToIndex(gridPos);
        return shipInfo[index];
    }

    /// <summary>
    /// 보드만 리셋 (함선이 가진 정보는 신경쓰지 않음)
    /// </summary>
    public void RestBoard(Ship[] ships)
    {
        // 함선 초기화
        foreach (var ship in ships)
        {
            UndoShipDeplyment(ship);        // 모든 함선 배치 취소
        }

        // 공격 표시 초기화
        for (int i = 0; i < bombInfo.Length; i++)
        {
            bombInfo[i] = false;            // 공격 여부는 모두 안한것으로 설정
        }

        bombMark.RestBomMark();             // 붐 마크 리셋


    }

    // 확인용 함수들 --------------------------------------------------------------------------

    /// <summary>
    /// 월드 좌표가 보드 안쪽인지 확인하는 함수
    /// </summary>
    /// <param name="worldPos">체크할 월드 좌표</param>
    /// <returns>보드 안쪽이면 true, 아니면 false</returns>
    public bool IsValidPosition(Vector3 worldPos)
    {
        Vector3 diff = worldPos - transform.position;

        return diff.x >= 0.0f && diff.x <= BoardSize && diff.y < 0.0f && diff.x > -BoardSize;
    }

    /// <summary>
    /// 특정 위치에 배가 있는지 확인하는 함수
    /// </summary>
    /// <param name="shipPos">확인할 위치</param>
    /// <returns>배가 없으면 false, 배가 있으면 true</returns>
    bool IsShipDeployted(Vector2Int shipPos)
    {
        return shipInfo[GridToIndex(shipPos)] != ShipType.None;
    }

    /// <summary>
    /// 함선 배치 함수
    /// </summary>
    /// <param name="pos">배치할 함선</param>
    /// <param name="ship">배치할 그리드 좌표</param>
    /// <returns>성공하면 true, 아니면 false</returns>
    public bool ShipDeplyment(Ship ship, Vector2Int pos)
    {
        Vector2Int[] gridPositions;
        bool result = IsShipDeployment(ship, pos, out gridPositions);       // 배치 가능여부 확인

        if (result)
        {
            foreach (var temp in gridPositions)
            {
                shipInfo[GridToIndex(temp)] = ship.Type;        // shipInfo에 함선 배치 표시
            }

            Vector3 worldPos = GridToWorld(pos);
            ship.transform.position = worldPos;     // 함선의 위치 옮기기
            ship.Deploy(gridPositions);             // 함선 배치 처리

            // 개발 오브젝트추가 부분
            if (shipDeploymentInfo != null)
            {
                Vector3[] worlds = new Vector3[gridPositions.Length];

                for (int i = 0; i < worlds.Length; i++)
                {
                    worlds[i] = GridToWorld(gridPositions[i]);
                }

                shipDeploymentInfo.MarkShipDeploymentInfo(ship.Type, worlds);
            }
        }

        return result;
    }

    /// <summary>
    /// 함선 배치 함수
    /// </summary>
    /// <param name="pos">배치할 함선</param>
    /// <param name="ship">배치할 월드 좌표</param>
    /// <returns>성공하면 true, 아니면 false</returns>
    public bool ShipDeplyment(Ship ship, Vector3 pos)
    {
        Vector2Int gridPos = WorldToGrid(pos);

        return ShipDeplyment(ship, gridPos);
    }

    /// <summary>
    /// 함선 배치 취소 함수
    /// <paramref name="ship">배치를 취소할 배</paramref>
    /// </summary>
    public void UndoShipDeplyment(Ship ship)
    {
        if (shipDeploymentInfo != null)
        {
            shipDeploymentInfo.UnMarkShipDeploymentInfo(ship.Type);
        }

        if (ship.Positions != null)
        {
            foreach (var temp in ship.Positions)
            {
                shipInfo[GridToIndex(temp)] = ShipType.None;
            }
        }

        ship.UnDeploy();
    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배(크기와 방향)</param>
    /// <param name="pos">확인할 배의 위치(배의 머리 위치, 그리드 포지션)</param>
    /// <param name="gridPositions">배가 배치될 수 있을 때 배치되는 위치</param>
    /// <returns>true면 배치 가능, false면 배치 불가능</returns>
    public bool IsShipDeployment(Ship ship, Vector2Int pos, out Vector2Int[] gridPositions)
    {
        gridPositions = new Vector2Int[ship.Size];

        Vector2Int dir = Vector2Int.zero;
        switch (ship.Direction)
        {
            case ShipDirection.North:
                dir = Vector2Int.up;        // (0, 1) 북쪽을 바라보니까 꼬리로 갈 수록 y는 증가
                break;
            case ShipDirection.East:
                dir = Vector2Int.left;
                break;
            case ShipDirection.South:
                dir = Vector2Int.down;
                break;
            case ShipDirection.West:
                dir = Vector2Int.right;
                break;
            default:
                break;
        }

        // 확인할 위치들 따로 뽑아 놓기
        for (int i = 0; i < ship.Size; i++)
        {
            gridPositions[i] = pos + dir * i;
        }

        // 확인할 위치들 하나씩 확인
        bool result = true;

        foreach (var temp in gridPositions)
        {
            // 한칸이라도 보드를 벗어나거나 배가 배치 되어있으면 실패
            if (!IsValidPosition(temp) || IsShipDeployted(temp))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 특정 위치가 공격 실패한 지점인 확인하는 함수
    /// </summary>
    /// <param name="gridPos">확인할 위치</param>
    /// <returns>true면 공격이 실패한 지점. false면 공격이 성공한 지점</returns>
    public bool IsAttackFailPostion(Vector2Int gridPos)
    {
        int index = GridToIndex(gridPos);
        // 공격을 한 지점은 상대방도 배가 있는지 없는지 알 수 있으므로 shipInfo를 봐도 상관이 없다.
        return bombInfo[index] && (shipInfo[index] == ShipType.None);       // 공격을 했고 거기에 배가 없었다.
    }

    /// <summary>
    /// 특정 위치가 공격 성공한 지점인지 확인하는 함수
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public bool IsAttackSuccessPostion(Vector2Int gridPos)
    {
        int index = GridToIndex(gridPos);
        // 공격을 한 지점은 상대방도 배가 있는지 없는지 알 수 있으므로 shipInfo를 봐도 상관이 없다.
        return bombInfo[index] && (shipInfo[index] != ShipType.None);       // 공격을 했고 거기에 배가 있었다.
    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배(크기와 방향)</param>
    /// <param name="gridPos">확인할 배의 위치(배의 머리 위치, 그리드 포지션)</param>
    /// <returns>true면 배치 가능, false면 배치 불가능</returns>
    public bool IsShipDeployment(Ship ship, Vector2Int gridPos)
    {
        return IsShipDeployment(ship, gridPos, out _);
    }

    /// <summary>
    /// 특정 배가 특정 위치에서 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배(크기와 방향)</param>
    /// <param name="worldPos">확인할 배의 위치(배의 머리 위치, 월드 좌표)</param>
    /// <returns>true면 배치 가능, false면 배치 불가능</returns>
    public bool IsShipDeployment(Ship ship, Vector3 worldPos)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);

        return IsShipDeployment(ship, gridPos, out _);
    }

    // 피격용 ----------------------------------------------------------------------------

    /// <summary>
    /// 상대 플레이어에게 공격을 받았을 때 실행되는 함수
    /// </summary>
    /// <param name="gridPos">공격 받은 위치 (그리드 좌표)</param>
    /// <returns>true면 공격에 의해 함선에 명중 했다., false면 실패했다.</returns>
    public bool OnAttacked(Vector2Int gridPos)
    {
        bool result = false;

        if (IsValidPosition(gridPos))           // 보드 위인지 확인
        {
            int index = GridToIndex(gridPos);
            if (IsAttackable(index))            // 공격 했던 지점인지 아닌지 확인
            {
                bombInfo[index] = true;         // 공격 가능하면 공격했다고 표시

                if (shipInfo[index] != ShipType.None)   // 그곳에 배가 있으면
                {
                    result = true;              // 공격으로 배가 명중 됐다고 표시
                    onShipAttacked[shipInfo[index]]?.Invoke();  // 공격 당한 배의 델리게이트 실행
                }

                bombMark.SetbombMakr(GridToWorld(gridPos.x, gridPos.y), result);        // 붐 마크 표시
            }
        }

        return result;
    }

    public bool IsAttackable(Vector2Int gridPos)
    {
        return IsAttackable(GridToIndex(gridPos));
    }

    public bool IsAttackable(int index)
    {
        return !bombInfo[index];
    }
}
