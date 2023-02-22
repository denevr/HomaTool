using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCubes : MonoBehaviour
{
    public Vector2 randomScaleXZ = new Vector2(1.5f, 4.0f);
    public Vector2 randomScaleY = new Vector2(3.0f, 6.0f);
    public Vector2 randomOffsetX = new Vector2(0.0f, 1.0f);
    public int quantity = 10;
    public GameObject cubePrefab;
    public Vector2 randomGapZ = new Vector3(3.0f, 8.0f);
    void Start()
    {
        Vector3 currPos = transform.position;
        for (int i= 0; i < quantity; i++)
        {
            GameObject newInstance = Instantiate(cubePrefab, currPos, Quaternion.identity, transform);
            float scaleXZ = Random.Range(randomScaleXZ.x, randomScaleXZ.y);
            float scaleY = Random.Range(randomScaleY.x, randomScaleY.y);
            float offsetX = Random.Range(randomOffsetX.x, randomOffsetX.y);
            newInstance.transform.localScale = new Vector3(scaleXZ, scaleY, scaleXZ);
            newInstance.transform.position += transform.right * offsetX;
            newInstance.transform.position += transform.up * scaleY / 2.0f;
            currPos += transform.forward * (scaleXZ + Random.Range(randomGapZ.x, randomGapZ.y));
        }
    }
    
}
