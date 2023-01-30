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
    float timer;
    List<Transform> waypointList = new List<Transform>();



    public void SetEnemyType(GameObject enemyType)
    {
        enemyPrefab = enemyType;
    }

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


    public void SetWaypoints(Transform waypoints)
    {
        foreach (Transform child in waypoints)
        {
            waypointList.Add(child);
            Debug.Log("Added " + child.position);
        }

    }

    public void spawnEnemy()
    {
        enemyInstance = Instantiate(enemyPrefab, new Vector3(xPos, floorLevel, zPos), Quaternion.identity);

        foreach (Transform wpd in waypointList)
            Debug.Log("in spawn: Waypoints list " + wpd.position);
        enemyInstance.GetComponent<EnemyAI>().SetWaypoints(waypointList);
    }

    public bool enemyIsDead()
    {
        return enemyInstance == null;
    }



}
