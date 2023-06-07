using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject Spawner;
    float maxY = 4.0f;
    float minY = -4.0f;
    float SpawnTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn()
    {
        while(true)
        {
        GameObject obj = Instantiate(Spawner, transform.position, Quaternion.identity);
        obj.transform.Translate(0, Random.Range(minY, maxY), 0);
            yield return new WaitForSeconds(SpawnTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new(2, 10, 1));
        Gizmos.DrawLine(new(-13,  -5, 1), new(-13, 5, 1));
    }

}
