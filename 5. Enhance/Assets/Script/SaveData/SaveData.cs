using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int day;
    public int gold;
    public int fame;
    public int hp;

    public int[] weapons = { 0, 0, 0 };
    public int[] item = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        
}
