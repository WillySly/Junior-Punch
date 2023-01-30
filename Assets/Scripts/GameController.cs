using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform patrolWaypoints;

    // central room dimensions
    float crMinX = -46f, crMaxX = -6f, crMinZ = 4f, crMaxZ = 45f;

    // empty room dimensions
    float erMinX = 2f, erMaxX = 42f, erMinZ = 10f, erMaxZ = 45f;

    // table room dimensions
    float trMinX = -18f, trMaxX = 17f, trMinZ = -45f, trMaxZ = -3f;

    // ladder room dimensions
    float lrMinX = -28f, lrMaxX = 46f, lrMinZ = -45f, lrMaxZ = -10f;


    List<EnemySpawner> spawners = new List<EnemySpawner>();

    void Start()
    {
        // create spawner for each room

        EnemySpawner centralRoomSpawner = ScriptableObject.CreateInstance("EnemySpawner") as EnemySpawner;
        centralRoomSpawner.SetDimensions(crMinX, crMaxX, crMinZ, crMaxZ);
        centralRoomSpawner.SetWaypoints(patrolWaypoints.Find("CentralRoom"));


        EnemySpawner tableRoomSpawner = ScriptableObject.CreateInstance("EnemySpawner") as EnemySpawner; ;
        tableRoomSpawner.SetDimensions(trMinX, trMaxX, trMinZ, trMaxZ);
        tableRoomSpawner.SetWaypoints(patrolWaypoints.Find("TableRoom"));

        EnemySpawner emptyRoomSpawner = ScriptableObject.CreateInstance("EnemySpawner") as EnemySpawner; ;
        emptyRoomSpawner.SetDimensions(erMinX, erMaxX, erMinZ, erMaxZ);
        emptyRoomSpawner.SetWaypoints(patrolWaypoints.Find("EmptyRoom"));

        EnemySpawner ladderRoomSpawner = ScriptableObject.CreateInstance("EnemySpawner") as EnemySpawner; ;
        ladderRoomSpawner.SetDimensions(lrMinX, lrMaxX, lrMinZ, lrMaxZ);
        ladderRoomSpawner.SetWaypoints(patrolWaypoints.Find("LadderRoom"));


        spawners.Add(centralRoomSpawner);
        spawners.Add(tableRoomSpawner);
        spawners.Add(emptyRoomSpawner);
        spawners.Add(ladderRoomSpawner);

        foreach (EnemySpawner spawner in spawners)
        {
            spawner.SetEnemyType(enemyPrefab);
            spawner.spawnEnemy();
        }
    }

    void Update()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            if (spawner.enemyIsDead())
            {
                spawner.decreaseTimer(Time.deltaTime);
                spawner.spawnEnemy();
            }
        }
    }
}
