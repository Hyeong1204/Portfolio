using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float size;
    public Transform playerTransform;
    public Transform guideTransform;

    bool isFocus = false;
    bool isInteracted = false;

    private void Update()
    {
        if (isFocus)
        {
            float distance = Vector3.Distance(guideTransform.position, playerTransform.position);

            if (distance < size)
            {
                isInteracted = true;
                Interact();
            }
        }

    }

    public virtual void Interact()
    {
        
    }

    public void OnFocused(Transform tf)
    {
        isFocus = true;
        playerTransform = tf;
        isInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        playerTransform = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(guideTransform.position, size);
    }
}
