using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenChange : MonoBehaviour
{
    public ScenEnum scenChange;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Inst.ScenState = scenChange;
    }
}
