using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Create_Tower : MonoBehaviour
{
    GameObject create_panel;
    GameObject panel_background;
    Button cancel;
    Tile tile;

    public ObjectDetector detector;
    public TowerSpwaner towerSpwaner;
    public TowerDataManager towerDataManager;
    //public GameObject towerPrefab;

    int current_tower = 3;
    float tower_radius = 150.0f;

    private void Awake()
    {
        panel_background = transform.GetChild(0).gameObject;
        create_panel = transform.GetChild(1).gameObject;
        cancel = create_panel.transform.GetChild(0).GetComponent<Button>();

        cancel.onClick.AddListener(OnClick_Cancel);

    }

    private void Start()
    {
        //for (int i = 0; i < towerDataManager.towerData.Length; i += 2)
        //{
        //    GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        //    tower.transform.parent = create_panel.transform;
        //    tower.name = towerDataManager.towerData[i].towerName;
        //    tower.transform.GetComponent<Image>().sprite = towerDataManager.towerData[i].towerIcon;
        //    tower.transform.GetComponent<Image>().SetNativeSize();
        //    tower.transform.GetComponent<Button>().onClick.AddListener(() => OnClick_BuildButton(i / 2));
        //}
        for (int i = 0; i < towerDataManager.towerData.Length; i += 2)
        {
            GameObject tower = create_panel.transform.GetChild(i / 2 + 1).gameObject;
            tower.name = towerDataManager.towerData[i].towerName;
            tower.transform.GetComponent<Image>().sprite = towerDataManager.towerData[i].towerIcon;
            tower.transform.GetComponent<Image>().SetNativeSize();
        }

        panel_background.SetActive(false);
        create_panel.SetActive(false);
    }

    private void Update()
    {
        if (detector.IsTileSelect && !create_panel.activeSelf)
        {
            if (detector.selectTile)
            {
                tile = detector.selectTile.GetComponent<Tile>();
                if (!tile.isBulidTower)
                {
                    Open_Panel();
                }
                else
                {
                    detector.IsTileSelect = false;
                    detector.selectTile = null;
                }
            }
        }
    }

    void Open_Panel()
    {
        panel_background.SetActive(true);
        create_panel.SetActive(true);
        create_panel.transform.position = Camera.main.WorldToScreenPoint(detector.selectTile.transform.position);
        for(int i = 0; i < current_tower; i++)
        {
            float tower_angle = Mathf.PI * 0.5f - i * (Mathf.PI * 2.0f) / current_tower;
            GameObject child_tower = create_panel.transform.GetChild(i + 1).gameObject;
            child_tower.transform.position = create_panel.transform.position + (new Vector3(Mathf.Cos(tower_angle), Mathf.Sin(tower_angle), 0)) * tower_radius;
            Panel_Position(child_tower, child_tower.transform.position);
        }
    }

    void Panel_Position(GameObject child_tower, Vector3 pos)
    {
        RectTransform rect = (RectTransform)child_tower.transform;

        if (pos.x + rect.sizeDelta.x > Screen.width)
        {
            pos.x -= rect.sizeDelta.x;
        }
        if (pos.x - rect.sizeDelta.x < 0)
        {
            pos.x += rect.sizeDelta.x;
        }
        if (pos.y + rect.sizeDelta.y > Screen.height)
        {
            pos.y -= tower_radius * 2;
        }
        if (pos.y - rect.sizeDelta.y < 0)
        {
            pos.y += tower_radius * 2;
        }

        child_tower.transform.position = pos;
    }


    private void OnClick_Cancel()
    {
        panel_background.SetActive(false);
        create_panel.SetActive(false);
        detector.IsTileSelect = false;
        detector.selectTile = null;
    }

    public void OnClick_BuildButton(int towerIndex)
    {
        BuildTower(towerIndex);
        panel_background.SetActive(false);
        create_panel.SetActive(false);
    }


    void BuildTower(int index)
    {
        if (detector.selectTile != null && detector.IsTileSelect)
        {
            towerSpwaner.SpawnTower(detector.selectTile, index); // 버튼이 눌려지면 타워 스포너를 통해 설치
            detector.IsTileSelect = false; // 타일 해제
            detector.selectTile = null;
        }
    }

}
