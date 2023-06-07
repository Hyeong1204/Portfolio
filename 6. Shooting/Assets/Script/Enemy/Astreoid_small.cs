using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.U2D;

public class Astreoid_small : MonoBehaviour
{
    GameObject explosion;

    public float speed = 5.0f;
    public int score = 5;
    Action<int> onDead;

    private void Awake()
    {
        explosion = transform.GetChild(0).gameObject;


        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        int rand = UnityEngine.Random.Range(0, 4);
        renderer.flipX = (rand & 0b_01) != 0;
        renderer.flipY = (rand & 0b_10) != 0;
    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        onDead += player.AddScore;
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector3.up, Space.Self);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            onDead?.Invoke(score);
            explosion.SetActive(true);
            explosion.transform.parent = null;

            Destroy(this.gameObject);
        }
    }

}
