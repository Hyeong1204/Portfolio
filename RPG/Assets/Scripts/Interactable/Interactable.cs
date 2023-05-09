using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float size;
    public Transform playerTransform;
    public Transform guideTransform;

    private void Update()
    {
        float distance = Vector3.Distance(guideTransform.position, playerTransform.position);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(guideTransform.position, size);
    }
}
