using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager_Scene : MonoBehaviour
{
    public void moveScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}
