using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : ScriptableObject
{
    float floorLevel = 1.5f;
    float spawnRadius = 2f;
    float xPos;
    float zPos;
    GameObject enemyPrefab;
    GameObject enemyInstance;

    float spawnDelay = 10f;
    float timer = 0f;       // respawns timer
    List<Transform> waypointList = new List<Transform>();

    public void SetEnemyType(GameObject enemyType)
    {
        enemyPrefab = enemyType;
    }

    // Provided the 4 coordinates of the room spows enemy at random point
    // in the room. Checks for collisions with environment. 
    public void SetDimensions(float minX, float maxX, float minZ, float maxZ)
    {
        LayerMask obstaclesLayer = LayerMask.GetMask("Obstacles");
        LayerMask propsLayer = LayerMask.GetMask("Props");

        Collider[] obstacles;
        Collider[] props;

        do
        {
            xPos = Random.Range(minX, maxX);
            zPos = Random.Range(minZ, maxZ);
            obstacles = Physics.OverlapSphere(new Vector3(xPos, floorLevel, zPos), spawnRadius, obstaclesLayer);
            props = Physics.OverlapSphere(new Vector3(xPos, floorLevel, zPos), spawnRadius, propsLayer);

        } while (props.Length != 0 || obstacles.Length != 0);

    }

    // Waypoints are different for each instance
    public void SetWaypoints(Transform waypoints)
    {
        foreach (Transform child in waypoints)
        {
            waypointList.Add(child);
        }

    }

    public void spawnEnemy()
    {
        if (timer <= 0)
        {
            enemyInstance = Instantiate(enemyPrefab, new Vector3(xPos, floorLevel, zPos), Quaternion.identity);
            enemyInstance.GetComponent<EnemyMovementAI>().SetWaypoints(waypointList);
            timer = spawnDelay;
        }
    }

    // Decrease respawn timer
    public void decreaseTimer(float delta)
    {
        timer -= delta;
    }

    public bool enemyIsDead()
    {
        return enemyInstance == null;
    }



}
