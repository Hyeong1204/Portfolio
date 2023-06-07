using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMark : MonoBehaviour
{
    /// <summary>
    /// 공격이 성공했을 때 보일 프리팹
    /// </summary>
    public GameObject successMark;

    /// <summary>
    /// 공격이 실패했을 때 보일 프리팹
    /// </summary>
    public GameObject failureMark;

    /// <summary>
    /// 테스트 용도로 고격한 지점 표시하는 프리팹
    /// </summary>
    public GameObject testInfoPrefab;

    /// <summary>
    /// testInfoPrefab을 보여줄지 결정하는 변수. true면 보여준다.
    /// </summary>
    public bool isShowTestInfo = true;

    /// <summary>
    /// 공격 받은 위치에 포탄 명중 여부를 표시해주는 함수
    /// </summary>
    /// <param name="position">공격 받은 위치 (그리드를 다시 월드로 변경한 것)</param>
    /// <param name="isSuccess">배에 명중 되었으면 true, 아니면 false</param>
    public void SetbombMakr(Vector3 position, bool isSuccess)
    {
        GameObject markPrefab = isSuccess ? successMark : failureMark;      // isSuccess가 true면 "O", false면 "X"
        GameObject markInstance = Instantiate(markPrefab, transform);       // 생성해서 위치 설정
        markInstance.transform.position = position + Vector3.up * 2f;

        if(isShowTestInfo)      // 테스트용 오브젝트 보여 줄거면
        {
            GameObject testobj = Instantiate(testInfoPrefab, markInstance.transform);    // 생성해서 위치 설정
            testobj.transform.position = position + Vector3.up * 0.5f;
        }
    }

    /// <summary>
    /// 붐 마크 리셋용
    /// </summary>
    public void RestBomMark()
    {
        while(transform.childCount > 0)     // 자식은 포탄 마크 밖에 없으니 자식이 없을 때까지 반복
        {
            Transform child = transform.GetChild(0);    // 첫 번째 자식 가져와서
            child.SetParent(null);                      // 부모 제거
            Destroy(child.gameObject);                  // 게임 오브젝트 삭제
        }
    }
}
