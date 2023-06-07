using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    Transform[] children;
    int index = 0;

    /// <summary>
    /// 현재 웨이포인트를 돌려주는 프로퍼티
    /// </summary>
    public Transform Current => children[index];

    private void Awake()
    {
        children = new Transform[transform.childCount];
        for(int i =0; i<transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
    }

    /// <summary>
    /// 다음 웨이포인트 리턴
    /// </summary>
    /// <returns></returns>
    public Transform MoveNext()
    {
        index++;                        // 1증가
        index %= children.Length;       // 계속 반복을 위해 %연산 사용

        return children[index];
    }
}
