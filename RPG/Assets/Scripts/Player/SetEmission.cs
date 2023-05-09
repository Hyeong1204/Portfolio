using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEmission : MonoBehaviour
{
    public SkinnedMeshRenderer skinRenderer;
    Material material;

    bool isAttacked = false;
    public float duration = 1.0f;
    float timeLeft = 0.0f;

    private void Start()
    {
        //material = skinRenderer.material;
    }

    public void FX()
    {
        isAttacked = true;
        material.EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        if (isAttacked)
        {
            if(timeLeft < duration)
            {
                timeLeft += Time.deltaTime;
            }
            else
            {
                material.DisableKeyword("_EMISSION");
                isAttacked = false;
                timeLeft = 0.0f;
            }
        }

        transform.localPosition = Vector3.zero;
    }
}
