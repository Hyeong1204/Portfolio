using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTimer : MonoBehaviour
{
    Item_Enemy_Stun_Use item_Stun;
    new Collider2D collider;

    private void Awake()
    {
        item_Stun = GetComponentInParent<Item_Enemy_Stun_Use>();
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            StartCoroutine(item_Stun.StartTimer());
            collider.enabled = false;
        }
    }
}
