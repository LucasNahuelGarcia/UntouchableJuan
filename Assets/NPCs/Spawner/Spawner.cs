using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float SecondsBetweenSpawn = 10;
    public float TimeOffset = 5;
    public GameObject ObjectToSpawn;
    void Start()
    {
        StartCoroutine(SpawnEverySeconds(SecondsBetweenSpawn, TimeOffset));
    }

    IEnumerator SpawnEverySeconds(float seconds, float TimeOffset)
    {
        yield return new WaitForSeconds(TimeOffset);
        while (true)
        {
            Instantiate(ObjectToSpawn, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(seconds);
        }
    }
}
