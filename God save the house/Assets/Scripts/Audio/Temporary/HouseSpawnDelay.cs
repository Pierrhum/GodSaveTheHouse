using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSpawnDelay : MonoBehaviour
{
    public GameObject[] houses;
    public Vector2 delayRangeBetweenSpawns;

    private void Start()
    {
        foreach (var house in houses)
        {
            house.SetActive(false);
        }
        StartCoroutine(SpawnHouseAfterDelay());
    }

    IEnumerator SpawnHouseAfterDelay()
    {
        foreach (var house in houses)
        {
            yield return new WaitForSeconds(Random.Range(delayRangeBetweenSpawns.x, delayRangeBetweenSpawns.y));
            house.SetActive(true);
        }
    }
}
