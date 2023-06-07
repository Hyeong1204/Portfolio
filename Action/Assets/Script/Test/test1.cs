using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class test1 : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * 2.0f * Vector3.right, Space.World);
    }

    private void Start()
    {
        Debug.Log(transform.position);
        Debug.Log(transform.TransformPoint(transform.up));
    }
}
