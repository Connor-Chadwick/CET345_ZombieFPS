using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAISpawner : MonoBehaviour
{
    [Header("Number of AI to Spawn")]
    public int totalAIToSpawn;

    [Header("AI GameObject")]
    public GameObject AI;

    [Header("Spawn Area")]
    public float minX;
    public float minY;
    public float minZ;
    public float maxX;
    public float maxY;
    public float maxZ;

    private float spawnX;
    private float spawnY;
    private float spawnZ;
    private void Awake()
    {
        Spawn();
    }
    private void Start()
    {
       
    }

    private void Spawn()
    {
        for (int i = 0; i < totalAIToSpawn; i++)
        {
            spawnX = Random.Range(minX, maxX);
            spawnY = Random.Range(minY, maxY);
            spawnZ = Random.Range(minZ, maxZ);
            Instantiate(AI, new Vector3(transform.localPosition.x + spawnX, transform.localPosition.y + spawnY, transform.localPosition.z + spawnZ), Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.localPosition.x + minX, transform.localPosition.y + minY, transform.localPosition.z + minZ), new Vector3(transform.localPosition.x + maxX, transform.localPosition.y + maxY, transform.localPosition.z + maxZ));

    }


}
