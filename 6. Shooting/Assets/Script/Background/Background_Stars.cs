using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class Background_Stars : Background
{
    SpriteRenderer[] sprite;


    protected override void Awake()
    {
        base.Awake();

        sprite = new SpriteRenderer[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            sprite[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();           // 자식의 SpriteRenderer를 찾아서 넣는다. 
        }
    }

    protected override void MoveRightEnd(int index)
    {
        int rand = Random.Range(0, 4);
        base.MoveRightEnd(index);

        sprite[index].flipX = ((rand & 0b_01) != 0);
        sprite[index].flipY = ((rand & 0b_01) != 0);
    }
}
