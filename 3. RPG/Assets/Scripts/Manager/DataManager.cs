using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public InputField nameField;
    public InputField ageField;

    private void Start()
    {
        nameField.text = PlayerPrefs.GetString(nameField.text);
        ageField.text = PlayerPrefs.GetInt(ageField.text).ToString();
    }

    public void SaveData()
    {
        if(nameField.text != "" && ageField.text != "")
        {
            PlayerPrefs.SetString("Name", nameField.text);
            PlayerPrefs.SetInt("Age", int.Parse(ageField.text));

            Debug.Log("데이터가 저장되었습니다.");
            PlayerPrefs.Save();     // 안해주면 메모리 단에만 올려두고 저장은 하지 않는다.
        }

        
    }
}
