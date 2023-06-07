using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject Hit;

    float speed = 12.0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right, Space.Self);        // Space.Self : 자기 기준, Space.World : 씬 기준
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"on : {collision.gameObject.name}");
        //collision.contacts[0].point;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"ontrigger : {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Hit.gameObject.SetActive(true);
            Hit.transform.parent = null;
            //Hit.transform.position = collision.contacts[0].point;
            // conllision.contacts[0].normal : 법선 백터(노말 백터)
            // 노말 백터 : 특정 평면에 수직인 백터
            // 노말 백터는 반사를 계산하기 위해 반드시 필요하다. => 반사를 이용해서 그림자를 계산한다. 물리적인 반사도 계산한다.
            // 노말 백터를 구하기 위해 백터의 외적을 사용한다.
            //Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

}
