using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Player target;

    Vector3 offset;
    Vector3 diePosition = Vector3.zero;
    Quaternion dieRotation = Quaternion.identity;

    bool isTargetAlive = true;

    private void Start()
    {
        if( target == null)
        {
            target = Gamemanager.Inst.Player;       // 대상 구하기
        }

        target.onDie += OnTargetDie;
        offset = transform.position - target.transform.position;        // 대상과의 간격
    }

    private void LateUpdate()
    {
        if (isTargetAlive)
        {
            // 카메라의 위치 = 대상의 위치 + 간격
            transform.position = target.transform.position + offset;
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, diePosition, Time.deltaTime * 2.5f);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, dieRotation, Time.deltaTime * 2.5f);
        }
    }

    void OnTargetDie()
    {
        isTargetAlive = false;
        diePosition = target.transform.position + target.transform.up * 10.0f;
        dieRotation = Quaternion.LookRotation(-target.transform.up, -target.transform.forward);
    }
}
