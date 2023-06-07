using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delegateTest : MonoBehaviour
{
    public delegate int CharacterInfo(int value);

    int hp = 0;
    int defense = 0;
    public CharacterInfo characterInfo;

    public static event CharacterInfo characterInfoHandler;

    private void Start()
    {
        //CharacterInfo myCharacterInfo = new CharacterInfo(AddHp);

        characterInfoHandler = new CharacterInfo(AddDefense);
        characterInfoHandler += AddHp;
        characterInfoHandler(20);
        ChangeCharacterInfo(10, characterInfo);
    }


    int AddHp(int n)
    {
        Debug.Log(hp + n);
        return hp + n;
    }

    int AddDefense(int n)
    {
        Debug.Log(defense + n);
        return defense + n;
    }

    int ChangeCharacterInfo(int value, CharacterInfo characterInfo)
    {
        return characterInfo(value);
    }
}
