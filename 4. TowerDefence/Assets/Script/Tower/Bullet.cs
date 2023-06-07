using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float damage = 0.0f;
    float speed = 10.0f; // 총알 스피드
    bool isphysics; // true 물리 false 마법
    Rigidbody2D rb;

    GameObject target;
    Vector2 direction;

    Transform targetPos;

    public bool isSlowAttack = false; // true 슬로우공격 false 기본 공격
    public bool isStunAttack = false; // true 스턴공격 false 기본공격

    BuffManager buffManager;

    public GameObject Target // 공격타겟
    {
        get { return target; }
        set { target = value; }
    }

    private void Start()
    {
        buffManager = GameManager.Instance.Buff; 
    }

    public float Damage
    {
        get => damage;
        set
        {
            damage = value;
        }
    }
    
    public bool IsPhysics
    {
        get => isphysics;
        set
        {
            isphysics = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //rb.velocity = transform.right * speed; // 총알에게 velocity값을 준다.(일정하게 움직이기 위해)
        if (target != null)
        {
            targetPos = target.transform;
            direction = (target.transform.position - transform.position).normalized; // 방향지정
            rb.MovePosition(rb.position + Time.fixedDeltaTime * speed * direction); //방향으로 이동
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && isTarget(collision.gameObject))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.onHit(Damage,isphysics,isSlowAttack);
            if (isStunAttack)
            {
                buffManager.CreateBuff(BuffType.Stun, 0, 1.0f, enemy); //1초동안 스턴
            }
            Destroy(this.gameObject); // 적을 만나면 총알을 제거
        }
    }

    bool isTarget(GameObject enemy)
    {
        return Target == enemy;
    }
}
