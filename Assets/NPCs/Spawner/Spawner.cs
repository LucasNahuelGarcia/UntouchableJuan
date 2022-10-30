using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float SecondsBetweenSpawn = 10;
    public GameObject ObjectToSpawn;
    void Start()
    {
        StartCoroutine(SpawnEverySeconds(SecondsBetweenSpawn));
    }

    IEnumerator SpawnEverySeconds(float seconds)
    {
        while (true)
        {
            Instantiate(ObjectToSpawn, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(seconds);
        }
    }
}
