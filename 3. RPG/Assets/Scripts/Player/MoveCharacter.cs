using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    Transform myTransform;
    public float moveSpeed;

    private void Awake()
    {
        myTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        //myTransform.position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    private void Update()
    {
        //Vector3 vec = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        Vector3 vecRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

        myTransform.Translate(vecRaw * Time.deltaTime * moveSpeed);


        //myTransform.position = Vector3.MoveTowards();
    }
}
