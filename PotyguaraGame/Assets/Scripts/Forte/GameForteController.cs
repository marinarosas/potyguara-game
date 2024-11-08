using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameForteController : MonoBehaviour
{
    [Header("Timer")]
    private bool startTimer = false;
    private float count = 10;
    private GameObject timer;

    [Header("Pontuação")]
    private int levelCurrent = 1;
    private int currentPoints = 0;
    private int totalPoints = 0;

    private int gameMode = -1;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (startTimer)
        {
            InitTimer();
        }
    }

    public void ResetCount()
    {
        count = 10;
    }

    public void setStartMode(int value)
    {
        gameMode = value;
    }

    public int getMode()
    {
        return gameMode;
    }

    public void GameOver()
    {
        totalPoints -= currentPoints;
        count = 10;
        GameObject finishUI = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).GetChild(0).gameObject;
        finishUI.transform.GetChild(1).GetComponent<Text>().text = "Você Perdeu!!!";
        finishUI.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "Repetir Nivel";
        finishUI.SetActive(true);
    }

    public void ResetGame()
    {
        player.transform.position = new Vector3(809.36f, 8.2f, 400.38f);
        try
        {
            WallController[] walls = FindObjectsByType<WallController>(FindObjectsSortMode.InstanceID);
            foreach (WallController wall in walls)
            {
                wall.resetWall();
            }
            ManageWalls(false);
        }
        catch (Exception e)
        {
            Debug.Log("Erro when finding the walls: " + e);
        }

    }

    public void InitTimer()
    {
        // timer bar
        timer = GameObject.FindGameObjectWithTag("Time");
        if (count > 0)
        {
            count -= Time.deltaTime;
            timer.transform.GetChild(0).GetComponent<Text>().text = count.ToString("F0");
            timer.GetComponent<Image>().fillAmount -= Time.deltaTime / 9.6f;
            if (count <= 0)
            {
                count = 0;
                startTimer = false;
                FindObjectOfType<SpawnerController>().SetSpawn();
                if (levelCurrent == 1)
                {
                    ManageWalls(true);
                }
                timer.SetActive(false);
            }
        }
    }

    private void ManageWalls(bool value)
    {
        Transform walls = GameObject.Find("Walls").transform;
        for (int ii = 0; ii < walls.childCount; ii++)
        {
            walls.GetChild(ii).gameObject.SetActive(value);
        }
    }

    public void SetStartTimer()
    {
        startTimer = true;
    }

    public void SetCurrentPoints(int value)
    {
        currentPoints += value;
    }

    public int GetCurrrentPoints()
    {
        return currentPoints;
    }

    public void SetTotalPoints()
    {
        totalPoints += currentPoints;
        currentPoints = 0;
    }

    public int GetTotalPoints()
    {
        return totalPoints;
    }
}
