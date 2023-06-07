using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMap : MonoBehaviour
{

    Vector2 limitMix = new Vector2(9.0f, 5.0f);
    Vector2 limitMin = new Vector2(-9.0f, -5.0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > limitMix.x || transform.position.x < limitMin.x ||
            transform.position.y > limitMix.y || transform.position.y < limitMin.y)
        {
            Destroy(gameObject);
        }
    }
}
