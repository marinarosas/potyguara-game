using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameForteController : MonoBehaviour
{
    [Header("Timer")]
    private bool startTimer = false;
    private float count = 10;
    private GameObject timer;

    [Header("Pontuation")]
    private int currentPoints = 0;
    private int totalPoints = 0;

    [Header("Menus")]
    public GameObject MenuLevel1;
    public GameObject MenuLevel2;
    public GameObject MenuLevel3;

    [Header("General")]
    private int currentLevel;
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
        Transform finishUI = GameObject.FindWithTag("MainCamera").transform.GetChild(0).GetChild(0);
        finishUI.GetChild(1).GetComponent<Text>().text = "Você Perdeu!!!";
        finishUI.GetChild(3).GetChild(0).GetComponent<Text>().text = "Repetir Nivel";
        finishUI.GetChild(3).GetComponent<Button>().onClick.AddListener(ResetLevel);

        finishUI.gameObject.SetActive(true);
    }

    public void DestroyEnemiesRemanecentes()
    {
        Transform slot = FindFirstObjectByType<SpawnerController>().GetSlotEnemies();
        for (int ii = 0; ii < slot.childCount; ii++)
        {
            Destroy(slot.GetChild(ii).gameObject);
        }
        GameObject finishUI = GameObject.FindWithTag("MainCamera").transform.GetChild(0).GetChild(0).gameObject;
        finishUI.transform.GetChild(3).GetComponent<Button>().onClick.RemoveListener(ResetLevel);
    }


    public void ChangeStateWalls(bool value)
    {
        WallController[] walls = FindObjectsByType<WallController>(FindObjectsSortMode.InstanceID);
        foreach (WallController wall in walls)
        {
            wall.resetWall();
        }
        ManageWalls(value);
    }

    public void SetInformes(string message)
    {
        Transform mainCamera = GameObject.FindWithTag("MainCamera").transform;
        Transform informes = mainCamera.GetChild(2).GetChild(0).GetChild(2);
        informes.GetComponent<TextMeshProUGUI>().text = message;
        informes.parent.parent.gameObject.SetActive(true);
        informes.parent.parent.GetComponent<FadeController>().FadeInForFadeOutWithDeactivationOfGameObject(6f, informes.parent.parent.gameObject);
    }

    private void ResetLevel()
    {
        try
        {
            if (currentLevel == 1)
            {
                MenuLevel1.SetActive(true);
                ChangeStateWalls(false);
            }
             
            if (currentLevel == 2)
            {
                MenuLevel2.SetActive(true);
            }
            if (currentLevel == 3)
            {
                ResetLevelThree();
            }
            FindFirstObjectByType<SpawnerController>().SetLevelIsRunning(false); 
            DestroyEnemiesRemanecentes();
        }
        catch (Exception e)
        {
            Debug.Log("Erro when reset level: " + e);
        }
    }

    private void ResetLevelThree()
    {
        try
        {
            Transform target = GameObject.Find("Target").transform;
            target.GetComponent<TargetController>().health = 100;
            target.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = 100 + "";
            MenuLevel3.SetActive(true);
        }
        catch (Exception e)
        {
            Debug.Log("Erro when reset level three: " + e);
        }
    }

    public void ResetGame()
    {
        player.transform.position = new Vector3(809.36f, 8.2f, 400.38f);
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
                FindFirstObjectByType<SpawnerController>().SetSpawn();
                if (currentLevel == 1)
                {
                    ManageWalls(true);
                }
                timer.SetActive(false);
                timer.GetComponent<Image>().fillAmount = 1f;
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

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
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
