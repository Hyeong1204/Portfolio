using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Unity.VisualScripting;

public class Panel_TowerInformation : MonoBehaviour
{
    public ObjectDetector detector;

    GameObject panel_towerInfo;
    GameObject panel_closeButton;
    Button button_tower_upgrade;
    Button button_tower_advance;

    TextMeshProUGUI tower_name;
    TextMeshProUGUI tower_info;

    private void Awake()
    {
        panel_closeButton = transform.GetChild(0).gameObject;
        panel_towerInfo = transform.GetChild(1).gameObject;

        tower_name = panel_towerInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        tower_info = panel_towerInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        button_tower_upgrade = panel_towerInfo.transform.GetChild(3).GetComponent<Button>();
        button_tower_advance = panel_towerInfo.transform.GetChild(4).GetComponent<Button>();

    }

    private void Start()
    {
        panel_closeButton.SetActive(false);
        panel_towerInfo.SetActive(false);
    }
    private void Update()
    {
        if (detector.IsTowerSelect && !panel_towerInfo.activeSelf)
        {
            if (detector.selectTower && !detector.selectTile)
            {
                Open_Panel();
            }
        }
    }

    void Open_Panel()
    {
        panel_closeButton.SetActive(true);
        panel_towerInfo.SetActive(true);
        Panel_Position(Camera.main.WorldToScreenPoint(detector.selectTower.transform.position));
        //panel_towerInfo.transform.position = Camera.main.WorldToScreenPoint(detector.selectTower.transform.position);

        GameObject info_tower = detector.selectTower;

        if (info_tower.GetComponent<ArcherTower>())
        {
            tower_name.text = "아처";
        }
        if (info_tower.GetComponent<RangerTower>())
        {
            tower_name.text = "레인저";
        }
        if (info_tower.GetComponent<MagicianTower>())
        {
            tower_name.text = "수습마법사";
        }
        if (info_tower.GetComponent<MageTower>())
        {
            tower_name.text = "마법사";
        }
        if (info_tower.GetComponent<WarriorTower>())
        {
            tower_name.text = "전사";
        }
        if (info_tower.GetComponent<KinghtTower>())
        {
            tower_name.text = "기사";
        }

        //tower_name.text = info_tower.GetComponent<Tower>().type.ToString();
        tower_info.text = $"공격력 : {info_tower.GetComponent<Tower>().attackDamage}\n" +
            $"공격속도 : {info_tower.GetComponent<Tower>().attackSpeed}";
    }

    void Panel_Position(Vector3 pos)
    {
        RectTransform rect = (RectTransform)panel_towerInfo.transform.GetChild(0).transform;

        if (pos.x + rect.sizeDelta.x > Screen.width)
        {
            pos.x -= rect.sizeDelta.x;
        }
        if (pos.y + rect.sizeDelta.y > Screen.height)
        {
            pos.y -= (rect.sizeDelta.y + 100);
        }

        panel_towerInfo.transform.position = pos;
    }

    public void Close_Panel()
    {
        detector.IsTowerSelect = false;
        detector.selectTower = null;
        panel_closeButton.SetActive(false);
        panel_towerInfo.SetActive(false);
    }

    public void Upgrade_Tower()
    {
        Tower tower = detector.selectTower.GetComponent<Tower>();
        tower.towerUpgrade();
        Close_Panel();
    }

    public void Advance_Tower()
    {
        Tower tower = detector.selectTower.GetComponent<Tower>();
        if (tower.Level == tower.MaxTowerLevel)
        {
            tower.towerAdvance();
        }
        Close_Panel();
    }
}
