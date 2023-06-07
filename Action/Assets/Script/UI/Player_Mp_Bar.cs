using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Mp_Bar : MonoBehaviour
{
    Slider slider;
    TextMeshProUGUI mpText;
    string maxMpText;
    float maxMp;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        mpText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = Gamemanager.Inst.Player;            // 게임 메니저가 가지고 있는 플레이어 가져오기
        slider.value = 1.0f;                                // 슬라이더 최대치 만들기
        maxMpText = player.MaxMP.ToString();                // 최대 MP 표시용 글자 만들기
        maxMp = player.maxMP;                               // 치대 Mp 가져오기
        player.onManaChange += OnManaChange;                // MP가 변경될 때 실행되는 델리게이트에 함수 연결
        mpText.text = $"{maxMp} / {maxMpText}";             // 슬라이더 글자 최대치로 찍기
    }

    private void OnManaChange(float ratio)
    {
        ratio = Mathf.Clamp(ratio, 0, 1);           // 숫자가 넘치는 것을 방지
        slider.value = ratio;                       // 비율에 맞춰 슬라이더 조절

        float mp = maxMp * ratio;                   // 비율을 이용해서 현재 HP 계산
        mpText.text = $"{(int)mp} / {maxMpText}";        // Mp 출력
    }
}
