using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Image hpBar;

    public Transform target;
    public CharacterStat stat;
    Transform cam;

    public void Init(Transform newTarget, CharacterStat newStat)
    {
        this.target = newTarget;
        this.stat = newStat;
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        SetHp();
    }

    public void SetHp()
    {
        transform.position = target.position;
        transform.LookAt(new Vector3 (cam.position.x, transform.position.y, cam.position.z), Vector3.down);
        hpBar.fillAmount = NomalizeHp();
    }

    float NomalizeHp()
    {
        return Mathf.Clamp01(stat.CurrentHp / (float)stat.maxHp);
    }
}
