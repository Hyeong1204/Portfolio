using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceUI : MonoBehaviour
{
    public Color selectColor;
    public Color ideColor;

    Slot selectSlot;
    Slot[] slots;
    Image[] slotImages;
    Image selectBox;
    Button enhanceButton;

    // 강화 창 ---------------------------------------------------------------

    Button yesButton;
    Button noButton;
    Button popUpButton;

    CanvasGroup windowGroup;

    Image presentWeaponImage;
    Image nextWeaponImage;
    TextMeshProUGUI presenWeaponName;
    TextMeshProUGUI nextWeaponName;
    ItemMaterial[] itemMaterials;
    TextMeshProUGUI enhanceRatioText;

    PopUpWindow popUpWindow;
    TextMeshProUGUI popUpText;

    bool isHighLevel = false;

    private void Awake()
    {
        enhanceButton = transform.GetChild(3).GetComponent<Button>();
        enhanceButton.onClick.AddListener(EnhanceAnnouncementText);
        enhanceButton.interactable = false;
        slots = GetComponentsInChildren<Slot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].selectWeaponSlot += SettingSlot;
            slots[i].weapon.enhanceSuccess += SuccessPopUpWindow;
            slots[i].weapon.enhanceFail += FailPopUpWindow;
            slots[i].weapon.enganceDestroy += DestroyPopUpWindow;
            slots[i].weapon.lack += LackPopUpWindow;
        }

        slotImages = new Image[slots.Length];
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i] = transform.GetChild(i).GetComponent<Image>();
        }

        selectBox = transform.GetChild(4).GetComponent<Image>();
        selectBox.gameObject.SetActive(false);

        // 강화 창 컴포넌트 찾기 --------------------------------------------------------------------------

        windowGroup = GetComponentInChildren<CanvasGroup>();

        yesButton = windowGroup.transform.GetChild(4).GetComponent<Button>();
        yesButton.onClick.AddListener(EnhanceStart);
        noButton = windowGroup.transform.GetChild(5).GetComponent<Button>();
        noButton.onClick.AddListener(EnhanceWindowClose);

        presentWeaponImage = windowGroup.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        nextWeaponImage = windowGroup.transform.GetChild(2).GetChild(0).GetComponent<Image>();

        presenWeaponName = presentWeaponImage.transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        nextWeaponName = nextWeaponImage.transform.parent.GetComponentInChildren<TextMeshProUGUI>();

        itemMaterials = GetComponentsInChildren<ItemMaterial>();
        enhanceRatioText = windowGroup.transform.GetChild(7).GetComponent<TextMeshProUGUI>();

        foreach (var itemMaterial in itemMaterials)
        {
            itemMaterial.gameObject.SetActive(false);
        }

        popUpWindow = GetComponentInChildren<PopUpWindow>();
        popUpButton = popUpWindow.GetComponentInChildren<Button>();
        popUpButton.onClick.AddListener(PopUpWindowClose);

        popUpText = popUpWindow.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        if (gameManager.Weapons[0] != null && gameManager.Weapons[1] != null && gameManager.Weapons[2] != null)
        {
            // 세이브 본이 있을 때
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].weapon.EnganceRange = gameManager.Weapons[i].EnganceRange;
            }
        }
        else
        {
            // 세이브 본이 없을 때
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].weapon.Rest();
            }
        }

        PopUpWindowClose();
        EnhanceWindowClose();
    }

    /// <summary>
    /// 강화안내문구 함수. (강화 상세정보 출력 및 할지 말지 선택)
    /// </summary>
    void EnhanceAnnouncementText()
    {
        EnhanceWindowOpen();
    }

    void EnhanceStart()
    {
        UI ui = UI.Instance;

        if (selectSlot != null && ui.HP > 0)
        {
            selectSlot.weapon.Enhance();        

            GameManager gameManager = GameManager.Inst;

            for (int i = 0; i < slots.Length; i++)
            {
                gameManager.WeaponSave(slots[i].weapon, i);
            }
        }
        else if(ui.HP == 0)
        {
            PopUpWindowOpen();
            popUpText.text = $"체력이 부족합니다.\r\n휴식을 취하세요!";
        }
    }

    /// <summary>
    /// 슬롯 선택
    /// </summary>
    /// <param name="slot"></param>
    void SettingSlot(Slot slot)
    {
        if (selectSlot != slot)
        {
            // 선택
            if (selectSlot == null)
            {
                enhanceButton.interactable = true;      // 버튼 활성화
            }

            selectSlot = slot;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == slot)
                {
                    // 선택한 슬롯이면
                    slotImages[i].color = selectColor;
                    selectBox.transform.SetParent(slotImages[i].transform);
                    selectBox.rectTransform.anchoredPosition = Vector3.zero;
                }
                else
                {
                    // 선택한 슬롯이 아니면
                    slotImages[i].color = ideColor;
                }
            }
            selectBox.gameObject.SetActive(true);
        }
        else
        {
            // 선택 해제
            selectSlot = null;
            enhanceButton.interactable = false;
            for (int i = 0; i < slots.Length; i++)
            {
                slotImages[i].color = ideColor;
            }
            selectBox.transform.SetParent(transform);
            selectBox.gameObject.SetActive(false);
        }
    }

    void EnhanceWindowOpen()
    {
        if (selectSlot != null)
        {
            windowGroup.alpha = 1.0f;
            windowGroup.blocksRaycasts = true;

            // 무기 이미지 적용
            presentWeaponImage.sprite = selectSlot.weapon.WeaponModel.itemIcon;
            nextWeaponImage.sprite = selectSlot.weapon.NextWeaponModel.itemIcon;

            // 무기 이름 적용
            presenWeaponName.text = selectSlot.weapon.WeaponModel.itemName;
            nextWeaponName.text = selectSlot.weapon.NextWeaponModel.itemName;

            enhanceRatioText.text = $"성공 확률 : {selectSlot.weapon.WeaponModel.enforceRatio * 100:f2}%\r\n하락 확률 : {selectSlot.weapon.WeaponModel.DropRatio * 100:f2}%\r\n파괴 확률 : {selectSlot.weapon.WeaponModel.destroy * 100:f2}%";

            if (selectSlot.weapon.EnganceRange < selectSlot.weapon.Model.WeaponDatas.Length - 1)
            {
                int index = selectSlot.weapon.WeaponModel.costData.Length;

                for (int i = 0; i < itemMaterials.Length; i++)
                {
                    if (i < index)
                    {
                        // 재료 종류 개수가 i보다 크면
                        itemMaterials[i].gameObject.SetActive(true);
                        itemMaterials[i].image.sprite = Inventory.Inst.itemData[(int)selectSlot.weapon.WeaponModel.costData[i].costItem].itemIcon;
                        itemMaterials[i].TextChange(Inventory.Inst.inventory[(int)selectSlot.weapon.WeaponModel.costData[i].costItem].itemCount, selectSlot.weapon.WeaponModel.costData[i].costItemValue);
                    }
                    else
                    {
                        itemMaterials[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                // 현재 최고 단계
                // 강화창 닫고 파업창 출력
                isHighLevel = !isHighLevel;
                WaeponHighestLevel();
            }
        }
    }

    void EnhanceWindowClose()
    {
        windowGroup.alpha = 0.0f;
        windowGroup.blocksRaycasts = false;

        for (int i = 0; i < itemMaterials.Length; i++)
        {
            itemMaterials[i].gameObject.SetActive(false);
        }
    }

    void SuccessPopUpWindow(int range)
    {
        PopUpWindowOpen();
        popUpText.text = $"강화에 성공하여 강화 단계가 상승합니다.\r\n현재 강화 단계 : {range}";
    }
    void FailPopUpWindow(int range)
    {
        PopUpWindowOpen();
        popUpText.text = $"강화에 실패하여 강화 단계가 하락합니다.\r\n현재 강화 단계 : {range}";
    }
    void DestroyPopUpWindow(int range)
    {
        PopUpWindowOpen();
        popUpText.text = $"무기가 파괴 되어 0강으로 돌아갑니다.\r\n현재 강화 단계 : {range}";
    }

    void LackPopUpWindow()
    {
        PopUpWindowOpen();
        popUpText.text = $"재료가 부족합니다.";
    }

    void WaeponHighestLevel()
    {
        PopUpWindowOpen();
        popUpText.text = $"현재 최고 단계입니다.";
    }

    void PopUpWindowOpen()
    {
        popUpWindow.gameObject.SetActive(true);
    }

    void PopUpWindowClose()
    {
        popUpWindow.gameObject.SetActive(false);

        if (!isHighLevel)
        {
            EnhanceWindowOpen(); 
        }
        else
        {
            isHighLevel = !isHighLevel;
            EnhanceWindowClose();
        }
    }
}
