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
    private float count;
    private GameObject timer;

    [Header("Score")]
    private int currentPoints = 0;
    private int totalPoints = 0;

    [Header("Menus")]
    public GameObject handMenuLevel1;
    public GameObject handMenuLevel2;

    [Header("Buttons")]
    public Button normalMode;
    public Button zombieMode;

    [Header("General")]
    private int currentLevel;
    private int gameMode = -1;
    public GameObject portal;

    [Header("Player")]
    private Transform mainCamera;
    private SimpleShoot leftGunController;
    private SimpleShoot rightGunController;
    private LeftHandController leftController;
    private RightHandController rightController;

    private void Update()
    {
        if (startTimer)
        {
            InitTimer();
        }
    }

    public void SetLeftHand()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").transform;
        leftController = FindFirstObjectByType<LeftHandController>();
        leftGunController = leftController.gameObject.transform.GetChild(3).GetChild(2).GetChild(1).GetComponent<SimpleShoot>();

        leftController.ChangeHand();
        leftGunController.setLeftHand(true);
        mainCamera.GetChild(0).GetChild(1).gameObject.SetActive(true);
        mainCamera.GetChild(0).GetChild(2).gameObject.SetActive(true);
        leftGunController.Reload();
    }

    public void SetRightHand()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").transform;
        rightController = FindFirstObjectByType<RightHandController>();
        rightGunController = rightController.gameObject.transform.GetChild(3).GetChild(2).GetChild(1).GetComponent<SimpleShoot>();

        rightController.ChangeHand();
        rightGunController.setRightHand(true);
        mainCamera.GetChild(0).GetChild(1).gameObject.SetActive(true);
        mainCamera.GetChild(0).GetChild(2).gameObject.SetActive(true);
        rightGunController.Reload();
    }

    public void NextLevel()
    {
        FindFirstObjectByType<SpawnerController>().SetLevel();
        FindFirstObjectByType<LeftHandController>().ResetHand();
        FindFirstObjectByType<RightHandController>().ResetHand();
    }

    public Button GetZombieModeButton()
    {
        return zombieMode;
    }

    public Button GetNormalModeButton()
    {
        return normalMode;
    }

    public void ResetCount()
    {
        if (gameMode == 0)
        {
            timer.transform.GetChild(0).GetComponent<Text>().text = 10 + "";
            count = 10;
        }
        else
        {
            timer.transform.GetChild(0).GetComponent<Text>().text = 120 + "";
            count = 120;
        }
    }

    public void SetStartMode(int value)
    {
        Transform mainCamera = GameObject.FindWithTag("MainCamera").transform;
        timer = mainCamera.GetChild(0).Find("Timer").gameObject;
        if (value == 0)
        {
            timer.transform.GetChild(0).GetComponent<Text>().text = 10 + "";
            count = 10;
            
        }
        else
        {
            timer.transform.GetChild(0).GetComponent<Text>().text = 120 + "";
            count = 120;
        }
        gameMode = value;
    }

    public int GetMode()
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

    public void DestroyRemainingEnemies()
    {
        FindFirstObjectByType<SpawnerController>().CleanSlot();
    }

    public void ChangeStateWalls(bool value)
    {
        WallController[] walls = FindObjectsByType<WallController>(FindObjectsSortMode.InstanceID);
        foreach (WallController wall in walls)
            wall.resetWall();

        ManageWalls(value);
    }

    private void ResetLevel()
    {
        try
        {
            if (gameMode == 0)
            {
                if (currentLevel == 1)
                    ResetLevelOne();

                if (currentLevel == 2)
                    ResetLevelTwo();

                LeftHandController leftHand = FindFirstObjectByType<LeftHandController>();
                RightHandController rightHand = FindFirstObjectByType<RightHandController>();   

                if (leftHand.GetHand())
                    leftHand.ResetHand();
                if (rightHand.GetHand())
                    rightHand.ResetHand();

                DestroyRemainingEnemies();
            }
            else
                FindFirstObjectByType<SpawnerController>().CleanSlot();

            FindFirstObjectByType<SpawnerController>().SetLevelIsRunning(false);
        }
        catch (Exception e)
        {
            Debug.Log("Erro when reset level: " + e);
        }
    }

    private void ResetLevelOne()
    {
        try
        {
            FindFirstObjectByType<SpawnerController>().SetLevel();
            FindFirstObjectByType<GameForteController>().ResetCount();
            handMenuLevel1.SetActive(true);
            handMenuLevel1.GetComponent<FadeController>().FadeIn();
            ChangeStateWalls(false);
        }
        catch (Exception e)
        {
            Debug.Log("Erro when reset level one: " + e);
        }
    }

    private void ResetLevelTwo()
    {
        try
        {
            FindFirstObjectByType<TechGuaraController>().CreateReport("Zumbis a Vista!!!", "Olá jogador(a), para esse nível você não deve deixar que os zumbis cheguem até você. Se eles se aproximarem demais, você morre!!!", 7f);
            FindFirstObjectByType<GameForteController>().ResetCount();
            FindFirstObjectByType<SpawnerController>().NextLevel(90f, new Vector3(654.91f, 18.6f, 400.95f));
            handMenuLevel2.SetActive(true);
            handMenuLevel2.GetComponent<FadeController>().FadeIn();
        }
        catch (Exception e)
        {
            Debug.Log("Erro when reset level two: " + e);
        }
    }


    public void ResetGame()
    {
        FindFirstObjectByType<LeftHandController>().ResetHand();
        FindFirstObjectByType<RightHandController>().ResetHand();

        Transform finishUI = GameObject.FindWithTag("MainCamera").transform.GetChild(0).GetChild(0);
        finishUI.gameObject.SetActive(false);
        FindFirstObjectByType<HeightController>().NewHeight(8.2f);

        ResetCount();
        SetInitScene();
        //FindFirstObjectByType<RankingController>().ShowRanking();
    }

    private void SetInitScene()
    {
        GameObject.FindWithTag("Player").transform.position = new Vector3(809.36f, 8.2f, 400.38f);
        GameObject.FindWithTag("Player").transform.eulerAngles = new Vector3(0, -90f, 0);

        GameObject.FindWithTag("Ranking").SetActive(true);
        portal.SetActive(true);
        zombieMode.gameObject.transform.parent.gameObject.SetActive(true);
    }

    public void InitTimer()
    {
        // timer bar
        if (count > 0)
        {
            count -= Time.deltaTime;
            timer.transform.GetChild(0).GetComponent<Text>().text = count.ToString("F0");
            timer.GetComponent<Image>().fillAmount -= Time.deltaTime / (gameMode == 1 ? 119.6f : 9.6f);
            if (count <= 0)
            {
                count = 0;
                startTimer = false;
                if (gameMode == 0)
                {
                    FindFirstObjectByType<SpawnerController>().SetSpawn();
                    if (currentLevel == 1)
                        ManageWalls(true);
                    
                    timer.SetActive(false);
                    timer.GetComponent<Image>().fillAmount = 1f;
                }
            }
        }
    }

    private void ManageWalls(bool value)
    {
        Transform walls = GameObject.Find("Walls").transform;
        for (int ii = 0; ii < walls.childCount; ii++)
            walls.GetChild(ii).gameObject.SetActive(value);
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public void SetStartTimer()
    {
        startTimer = true;
    }

    public void SetCurrentScore(int value)
    {
        currentPoints += value;
    }

    public int GetCurrrentScore()
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
