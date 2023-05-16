using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float size;
    //public Transform playerTransform;
    public Transform guideTransform;

    bool isFocus = false;
    bool isInteracted = false;

    private void Update()
    {
        if (isFocus)
        {
            float distance = Vector3.Distance(guideTransform.position, Player.instance.transform.position);

            if (distance < size && !isInteracted)
            {
                isInteracted = true;
                Interact();
            }
        }

    }

    public virtual void Interact()
    {
        
    }

    public void OnFocused()
    {
        isFocus = true;
        isInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(guideTransform.position, size);
    }
}
