using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    float SpawnPosY;

    bool Started;

    public List<Spawner> Spawners;

    public float SpawnChance;
    public float PlatformDistance;
    

    void Update()
    {
        if (!Started)
        {
            Started = true;
            SpawnPosY = 0;
            for (int i = 0; i < 10; i++)
            {
                StartPlatformSpawn();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartPlatformSpawn();
        }
    }

    public void StartPlatformSpawn()
    {
        var TotalNoneSpawn = 0;
        SpawnPosY += PlatformDistance;

        for (int i = 0; i < Spawners.Count; i++)
        {
            var Chance = Random.Range(0, SpawnChance );

            if (Chance >= SpawnChance / 2)
            {
                Spawners[i].SpawnPlatform(true, SpawnPosY);
                continue;
            }

            TotalNoneSpawn++;
            Spawners[i].SpawnPlatform(false, SpawnPosY);
        }
        if (TotalNoneSpawn >= Spawners.Count)
        {
            int ForceSpawnNum = Random.Range(0, Spawners.Count);
            Spawners[ForceSpawnNum].SpawnPlatform(true, SpawnPosY);
        }
    }
}