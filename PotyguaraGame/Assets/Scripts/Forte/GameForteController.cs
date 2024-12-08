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
    public GameObject handMenuLevel3;

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
        leftGunController = FindFirstObjectByType<LeftHandController>().gameObject.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<SimpleShoot>();

        leftController.ChangeHand();
        leftGunController.setLeftHand();
        mainCamera.GetChild(0).GetChild(1).gameObject.SetActive(true);
        mainCamera.GetChild(0).GetChild(2).gameObject.SetActive(true);
        leftGunController.Reload();
    }

    public void SetRightHand()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").transform;
        rightController = FindFirstObjectByType<RightHandController>();
        rightGunController = FindFirstObjectByType<RightHandController>().gameObject.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<SimpleShoot>();

        rightController.ChangeHand();
        rightGunController.setLeftHand();
        mainCamera.GetChild(0).GetChild(1).gameObject.SetActive(true);
        mainCamera.GetChild(0).GetChild(2).gameObject.SetActive(true);
        rightGunController.Reload();
    }

    public void SetInitScene()
    {
        portal.SetActive(true);
        zombieMode.gameObject.transform.parent.gameObject.SetActive(true);
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
        return zombieMode;
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

    private void ResetLevel()
    {
        try
        {
            if (gameMode == 0)
            {
                if (currentLevel == 1)
                {
                    handMenuLevel1.SetActive(true);
                    handMenuLevel1.GetComponent<FadeController>().FadeIn();
                    ChangeStateWalls(false);
                }

                if (currentLevel == 2)
                {
                    handMenuLevel2.SetActive(true);
                    handMenuLevel2.GetComponent<FadeController>().FadeIn();
                }
                if (currentLevel == 3)
                {
                    ResetLevelThree();
                }
                DestroyRemainingEnemies();
            }
            else
            { 
                FindFirstObjectByType<SpawnerController>().CleanSlot();

            }
            FindFirstObjectByType<SpawnerController>().SetLevelIsRunning(false);
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
            handMenuLevel3.SetActive(true);
        }
        catch (Exception e)
        {
            Debug.Log("Erro when reset level three: " + e);
        }
    }

    public void ResetGame()
    {
        FindFirstObjectByType<HeightController>().NewHeight(8.2f);
        GameObject.FindWithTag("Player").transform.position = new Vector3(809.36f, 8.2f, 400.38f);
        GameObject.FindWithTag("Player").transform.eulerAngles = new Vector3(0, -90f, 0);
        SetInitScene();
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
                    {
                        ManageWalls(true);
                    }
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
