using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    private bool initLevel = false;
    public void SetLevel(int value)
    {
        initLevel = true;
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

    private void Update()
    {
        if(slot.childCount == 0 && initLevel)
        {
            initLevel = false;
            GameObject finishUI = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).GetChild(0).gameObject;
            finishUI.SetActive(true);
            finishUI.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
            currentLevel++;

            Debug.Log("Fim de Level");
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
                    StartCoroutine(TimeForSpawn(point));
                }
            }
        }
    }

    IEnumerator TimeForSpawn(Transform point)
    {
        yield return new WaitForSeconds(5f);
        Instantiate(prefabZumbi, point.position, Quaternion.identity, slot);
    }
}
