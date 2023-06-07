using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile_Obstacle : MonoBehaviour
{
    Item_Bomb_Use item_Bomb;            // Item_Bomb_Use 아이템
    Transform tree;                     // 장애물

    public bool IsBuildItem { get; set; }       // 설치 여부 판단

    int layerindex;                     // 레이어 인덱스

    private void Awake()
    {
        tree = transform.GetChild(0);   // 자식 찾기
        IsBuildItem = false;
    }

    private void Start()
    {
        layerindex = LayerMask.NameToLayer("WallTile");             // 레이어 WallTile의 인덱스 값을 찾아 layerindex에 넣기
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item_Bomb"))
        {
            item_Bomb = collision.GetComponent<Item_Bomb_Use>();        // 컴포넌트 찾기
            item_Bomb.onItemUse += OnDistroy;                       // Item_Bomb_Use에 있는 델리게이트랑 연결
        }
    }

    private void OnDistroy()
    {
        this.gameObject.layer = layerindex;             // 오브젝트 레이어를 layerindex으로 변경
        this.gameObject.tag = "WallTile";               // 오브젝트 태그를 "WallTile"로 변경
        gameObject.name = "WallTile";
        Destroy(tree.gameObject);                       // 자식(장애물)를 삭제
        gameObject.AddComponent<Tile>();
        Destroy(this.gameObject.GetComponent<Tile_Obstacle>());
    }

}
