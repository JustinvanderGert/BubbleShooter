using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    SpawnerManager MyManager;

    public GameObject Platform;
    public float BoostChance;

    public bool Boost;

    void Start()
    {
        var Chance = Random.Range(0, BoostChance);

        MyManager = FindObjectOfType<SpawnerManager>();
        MyManager.Spawners.Add(this);
    }

    public void SpawnPlatform(bool AllowSpawn, float SpawnPosY)
    {
        var SpawnPos = transform.position;
        SpawnPos.y += SpawnPosY;

        if (AllowSpawn)
            Instantiate(Platform, SpawnPos, transform.rotation);
    }
}