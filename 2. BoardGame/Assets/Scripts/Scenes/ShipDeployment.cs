using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployment : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Inst.GameState = GameState.ShipDeploymnet;   
    }
}
