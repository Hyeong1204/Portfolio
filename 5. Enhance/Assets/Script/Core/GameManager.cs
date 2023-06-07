using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    UI ui;
    WeaponItemManager weaponManager;

    Weapon[] weapons;

    ScenEnum scenState = ScenEnum.Title;


    public UI UI => ui;
    public WeaponItemManager WeaponManager => weaponManager;
    public Weapon[] Weapons => weapons;

    public ScenEnum ScenState
    {
        get => scenState;
        set
        {
            if (value != scenState)
            {
                scenState = value;
                switch (scenState)
                {
                    case ScenEnum.Main:
                        ui.HP -= 0;
                        gameStart?.Invoke();
                        break;
                    case ScenEnum.Store:
                        ui.HP -= 1;
                        break;
                    case ScenEnum.Blacksmithshop:
                        ui.HP -= 0;
                        break;
                    case ScenEnum.Blackmarket:
                        ui.HP -= 2;
                        break;
                    case ScenEnum.Title:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public Action gameStart;

    protected override void Initialize()
    {
        base.Initialize();
        ui = FindObjectOfType<UI>();
        weaponManager = GetComponent<WeaponItemManager>();
        weapons = new Weapon[3];
        for (int i = 0; i < weapons.Length; i++)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = transform;
            obj.name = $"Weapon({i})";
            obj.AddComponent<Weapon>();
            weapons[i] = obj.GetComponent<Weapon>();
        }
    }

    public void WeaponSave(Weapon waepon, int count)
    {
        if (weapons != null)
        {
            weapons[count].EnganceRange = waepon.EnganceRange;
        }
    }

    public void SaleWeapon(int index)
    {
        if (weapons.Length <= index)
        {
            Debug.Log("잘못된 인덱스");
        }
        else
        {
            // 판매
            // 판매시 골드 증가
            ui.Gold += weapons[index].WeaponModel.saleValue;

            // 판매시 명성치 증가
            ui.Fame += weapons[index].WeaponModel.fameValue;

            // 판매시 해당 무기 강화수치 0으로 만들기
            weapons[index].EnganceRange = 0;
        }
    }

    public void BlackMarketSaleWeapon(int index, float ratio)
    {
        if (weapons.Length <= index && index >= 0)
        {
            Debug.Log("잘못된 인덱스");
        }
        else
        {
            // 판매
            // 판매시 골드 증가
            ui.Gold += Mathf.FloorToInt(weapons[index].WeaponModel.saleValue * ratio);

            // 판매시 명성치 증가
            ui.Fame -= (int)(weapons[index].WeaponModel.fameValue * 0.7f);

            // 판매시 해당 무기 강화수치 0으로 만들기
            weapons[index].EnganceRange = 0;
        }
    }

    public void GameSave()
    {
        SaveData saveData = new SaveData();
        saveData.day = ui.Day;
        saveData.gold = ui.Gold;
        saveData.fame = ui.Fame;
        saveData.hp = ui.HP;


        Inventory inven = Inventory.Inst;
        string path = $"{Application.persistentDataPath}/Save/";

        // 무기 저장
        for (int i = 0; i < saveData.weapons.Length; i++)
        {
            saveData.weapons[i] = weapons[i].EnganceRange;      // 무기의 강화 레벨만 저장
        }


        // 아이템 저장
        for (int i = 0; i < saveData.item.Length; i++)
        {
            saveData.item[i] = inven.inventory[i].itemCount;
        }

        string json = JsonUtility.ToJson(saveData);

        if (!Directory.Exists(path))            // path경로에 파일이 없으면
        {
            Directory.CreateDirectory(path);    // path경로에 파일을 만들어라
        }

        string fullPath = $"{path}data.json";
        File.WriteAllText(fullPath, json);

    }

    public void GameLoad()
    {
        string path = $"{Application.persistentDataPath}/Save/";

        string fullPath = $"{path}data.json";
        Inventory inven = Inventory.Inst;

        if (Directory.Exists(path) && File.Exists(fullPath))
        {
            // 파일 경로에 파일이 있으면 그파일을 불러온다
            string json = File.ReadAllText(fullPath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            ui.Day = saveData.day;
            ui.Gold = saveData.gold;
            ui.Fame = saveData.fame;
            ui.HP = saveData.hp;

            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].EnganceRange = saveData.weapons[i];
            }

            for (int i = 0; i < inven.inventory.Length; i++)
            {
                inven.inventory[i].itemCount = saveData.item[i];
            }
        }
        else
        {
            // 폴더나 파일이 없으면 초기값으로 변경
            ui.Day = 1;
            ui.Gold = 1000;
            ui.Fame = 0;
            ui.HP = ui.MaxHP;

            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].EnganceRange = 0;
            }

            for (int i = 0; i < inven.inventory.Length; i++)
            {
                inven.inventory[i].itemCount = 0;
            }
        }
    }
}
