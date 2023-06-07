using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보드의 배 배치정보. 총 256까지 가능
/// </summary>
public enum ShipType : byte
{
    None =0,        // 배가 배치되어 있지 않다.
    Carrier,        // 항공 모함이 배치 되어 있다. (사이즈 5)
    Battleship,     // 전함이 배치 되어 있다. (사이즈 4)
    Destroyer,      // 구축함이 배치 되어 있다 (사이즈 3)
    Submarine,      // 잠수함이 배치되어 있다. (사이즈 3)
    PatrolBoat      // 경비정이 배치되어 있다. (사이즈 2)
}

public enum ShipDirection : byte
{
    North = 0,
    East,
    South,
    West
}

public enum GameState : byte
{
    Title = 0,          // 타이틀 장면
    ShipDeploymnet,     // 함선 배치 장면
    Battle,             // 전투장면
    GAmeEnd             // 게임 종료 장면
}
