using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Hp_Bar : MonoBehaviour
{
    Slider slider;
    TextMeshProUGUI hpText;
    string maxHPText;
    float maxHp;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        hpText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = Gamemanager.Inst.Player;            // 게임 메니저가 가지고 있는 플레이어 가져오기
        slider.value = 1.0f;                                // 슬라이더 최대치 만들기
        maxHPText = player.MaxHP.ToString();                // 최대 HP 표시용 글자 만들기
        maxHp = player.MaxHP;                               // 치대 Hp 가져오기
        player.onHealthChange += OnHealthChange;             // HP가 변경될 때 실행되는 델리게이트에 함수 연결
        hpText.text = $"{maxHp} / {maxHPText}";      // 슬라이더 글자 최대치로 찍기
    }

    private void OnHealthChange(float ratio)
    {
        ratio = Mathf.Clamp(ratio, 0, 1);           // 숫자가 넘치는 것을 방지
        slider.value = ratio;                       // 비율에 맞춰 슬라이더 조절

        float hp = maxHp * ratio;                   // 비율을 이용해서 현재 HP 계산
        hpText.text = $"{hp} / {maxHPText}";        // hp 출력
    }
}
