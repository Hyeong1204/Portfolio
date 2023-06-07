using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Planet : MonoBehaviour
{
    public float speed = 1.0f;
    public float minRightEnd = 40.0f;
    public float maxRightEnd = 60.0f;
    public float minhegith = -8.0f;
    public float maxhegith = -5.0f;


    const float movePositionX = -10.0f;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * -transform.right);

        if(transform.position.x < movePositionX)
        {
            //transform.Translate(Random.Range(minRightEnd, maxRightEnd) * transform.right);

            Vector3 newPos = new Vector3(transform.position.x + Random.Range(minRightEnd, maxRightEnd), Random.Range(minhegith,maxhegith),0.0f);
            transform.position = newPos;
        }
    }
}
