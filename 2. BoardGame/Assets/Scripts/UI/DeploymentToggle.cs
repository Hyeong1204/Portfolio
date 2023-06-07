using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeploymentToggle : MonoBehaviour
{
    /// <summary>
    /// 이 버튼이 처리할 함선의 종류
    /// </summary>
    public ShipType shiptpye = ShipType.None;

    /// <summary>
    /// 배치할 함선을 가진 플레이어
    /// </summary>
    UserPlayer player;

    // 버튼 관려 변수 ---------------------------------------------------------------------------

    /// <summary>
    /// 버튼의 이미지
    /// </summary>
    Image image;

    /// <summary>
    /// 이 스크립트가 있는 버튼
    /// </summary>
    Button button;

    /// <summary>
    /// 눌러졋을 때 보일 색
    /// </summary>
    readonly Color selectedColor = new Color(1, 1, 1, 0.2f);

    /// <summary>
    /// 버튼이 토글 되었는지 여부
    /// </summary>
    bool isToggled = false;

    /// <summary>
    /// 버튼이 토글 상태를 확인하거나 토글 되었을 때 수행할 일들을 처리하는 프로퍼티
    /// </summary>
    private bool IsToggled
    {
        get => isToggled;
        set
        {
            if (IsToggled != value)                 // 토글 상태가 변경되었는지 확인
            {
                isToggled = value;                  // 변경된 사항이면 변경 적용
                if (isToggled)
                {
                    image.color = selectedColor;    // 토글 되었으면 색상 변경하고
                    onTogglePress?.Invoke(this);    // 토글 되었다고 델리게이트 날리기
                }
                else
                {
                    image.color = Color.white;      // 토글 해제되었으면 원래 색상으로 되돌리기
                }
            }
        }
    }

    /// <summary>
    /// 토글이 안된 상태에서 눌러졋을 때 실행될 델리게이트
    /// </summary>
    public Action<DeploymentToggle> onTogglePress;

    // 함수들 -------------------------------------------------------------------------------------

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        player = GameManager.Inst.UserPlayer;
    }

    /// <summary>
    /// 버튼이 클릭 되었을 때 실행되는 함수
    /// </summary>
    private void OnClick()
    {
        if (IsToggled)
        {
            // 눌러져 있다가 해제될 예정
            player.UndoShipDeploy(shiptpye);        // 배치 취소
        }
        else
        {
            // 해제 되어있다가 눌려질 예정
            player.SelectShipToDeploy(shiptpye);        // 배치하기 위한 선택
        }

        IsToggled = !IsToggled; // 토글 상태만 반적 시킴
    }

    /// <summary>
    /// 토글 상태를 해제하는 함수
    /// </summary>
    public void UnToggle()
    {
        if (!player.Ships[(int)shiptpye - 1].IsDeployed)    // 배치되지 않은 함선은 
        {
            IsToggled = false;      // 무조건 해제
        }
    }

    /// <summary>
    /// 무조건 눌러진 상태로 만드는 함수
    /// </summary>
    public void SetPress()
    {
        isToggled = true;
    }
}
