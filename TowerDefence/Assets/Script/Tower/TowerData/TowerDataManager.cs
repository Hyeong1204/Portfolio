using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDataManager : MonoBehaviour
{
    /// <summary>
    /// 모든 타워 데이터
    /// </summary>
    public TowerData[] towerData;

    /// <summary>
    /// 모든 타워 데이터[ID]
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns></returns>
    public TowerData this[uint id] => towerData[id];

    /// <summary>
    /// 모든 타워 데이터[Code]
    /// </summary>
    /// <param name="code">코드</param>
    /// <returns></returns>
    public TowerData this[TowerIDCode code] => towerData[(int)code];
}
