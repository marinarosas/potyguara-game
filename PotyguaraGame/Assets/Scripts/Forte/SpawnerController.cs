using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject prefabZumbi;
    public Transform slot; // gameobject parent of the zombies
    public Transform[] pointsForSpawn1;
    public Transform[] pointsForSpawn2;
    public Transform[] pointsForSpawn3;

    private int currentLevel = 0;
    private int currentAmountZumbis;

    public void SetLevel(int value)
    {
        currentLevel = value;
        if (currentLevel == 1)
        {
            currentAmountZumbis = 3;
            InitSpawner(pointsForSpawn1);
        }
        if (currentLevel == 2)
        {
            currentAmountZumbis = 4;
            InitSpawner(pointsForSpawn2);
        }
        if (currentLevel == 3)
        {
            currentAmountZumbis = 5;
            InitSpawner(pointsForSpawn3);
        }
    }

    public void InitSpawner(Transform[] points)
    {
        foreach(Transform point in points)
        {
            for (int ii = 0; ii < currentAmountZumbis; ii++)
            {
                if (ii == 0)
                {
                    Instantiate(prefabZumbi, point.position, Quaternion.identity, slot);
                }
                else
                {
                    StartCoroutine(TimeForSpawn());
                    Instantiate(prefabZumbi, point.position, Quaternion.identity, slot);
                }
            }
        }
    }

    IEnumerator TimeForSpawn()
    {
        yield return new WaitForSeconds(5f);
    }
}
