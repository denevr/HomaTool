using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] smallObstacles;
    [SerializeField]
    GameObject[] mediumObstacles;
    [SerializeField]
    GameObject[] bigObstacles;
    [SerializeField]
    float obstacleDistance;
    [SerializeField]
    float maxDistance;

    public void Spawn()
    {        
        int count = Mathf.RoundToInt(maxDistance / obstacleDistance);
        Vector3 currPos = Vector3.zero;
        for (int i = 0; i < count; i++)
        {
            int dif = Random.Range(0, 3);
            GameObject prefab = null;
            switch (dif)
            {
                case 0:
                    prefab = smallObstacles[Random.Range(0, smallObstacles.Length)];
                    break;
                case 1:
                    prefab = mediumObstacles[Random.Range(0, mediumObstacles.Length)];
                    break;
                case 2:
                    prefab = bigObstacles[Random.Range(0, bigObstacles.Length)];
                    break;
                default:
                    goto case 0;
            }
            currPos.x = Random.Range(-1, 2) * 1.5f;
            GameObject newObstacle = Instantiate(prefab, transform.position + currPos, Quaternion.Euler(0, Random.Range(-30, 30), 0), transform);
            currPos += transform.forward * obstacleDistance;
            if (Random.value > 0.5f)
                newObstacle.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void DestroyChildren()
    {
        for(int i = transform.childCount; i > 0; i--)
        {
            Destroy(transform.GetChild(i - 1).gameObject);
        }
    }
}
