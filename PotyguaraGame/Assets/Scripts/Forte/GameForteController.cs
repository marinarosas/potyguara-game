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

    [Header("Ranking")]
    public GameObject ranking;
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
            InitTimer();
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
        FindFirstObjectByType<TechGuaraController>().StopTutorial();
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
        FindFirstObjectByType<TechGuaraController>().StopTutorial();
        rightGunController.Reload();
    }

    public void NextLevel()
    {
        timer.transform.GetChild(0).GetComponent<Text>().text = 90 + "";
        count = 90;
        FindFirstObjectByType<SpawnerController>().SetLevelZombieMode();
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
            timer.transform.GetChild(0).GetComponent<Text>().text = 90 + "";
            count = 90;

            Transform mainCam = GameObject.FindWithTag("MainCamera").transform;
            int potycoins = FindFirstObjectByType<PotyPlayerController>().GetPotycoins();
            if (potycoins >= 10)
            {
                handMenuLevel1.SetActive(true);
                handMenuLevel1.GetComponent<FadeController>().FadeIn();
                portal.SetActive(false);
                ranking.GetComponent<FadeController>().FadeOut();
                FindFirstObjectByType<PotyPlayerController>().ConsumePotycoins(10);
                FindFirstObjectByType<SpawnerController>().SetLevelZombieMode();
                zombieMode.gameObject.transform.parent.gameObject.SetActive(false);
                mainCam.GetChild(5).gameObject.SetActive(false);
            }
            else
                mainCam.GetChild(5).GetChild(4).GetComponent<FadeController>().FadeInForFadeOut(2f);
        }
        else
        {
            timer.transform.GetChild(0).GetComponent<Text>().text = 120 + "";
            count = 120;

            Transform mainCam = GameObject.FindWithTag("MainCamera").transform;
            int potycoins = FindFirstObjectByType<PotyPlayerController>().GetPotycoins();
            if (potycoins >= 10)
            {
                portal.SetActive(false);
                ranking.GetComponent<FadeController>().FadeOut();
                FindFirstObjectByType<PotyPlayerController>().ConsumePotycoins(10);
                FindFirstObjectByType<SpawnerController>().SetLevelNormalMode();
                SetStartTimer();
                normalMode.gameObject.transform.parent.gameObject.SetActive(false);
                mainCam.GetChild(5).gameObject.SetActive(false);
            }
            else
                mainCam.GetChild(5).GetChild(4).GetComponent<FadeController>().FadeInForFadeOut(2f);
        }
        gameMode = value;
    }

    public int GetMode()
    {
        return gameMode;
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

    public void ResetGame()
    {
        FindFirstObjectByType<LeftHandController>().ResetHand();
        FindFirstObjectByType<RightHandController>().ResetHand();

        Transform finishUI = GameObject.FindWithTag("MainCamera").transform.GetChild(0).GetChild(0);
        finishUI.gameObject.SetActive(false);
        GameObject.FindWithTag("MainCamera").transform.GetChild(5).gameObject.SetActive(true);
        FindFirstObjectByType<HeightController>().NewHeight(6.96f);

        ResetCount();
        SetInitScene();
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
            timer.GetComponent<Image>().fillAmount -= Time.deltaTime / (gameMode == 1 ? 119.6f : 79.6f);
            if (count <= 0)
            {
                count = 0;
                startTimer = false;
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
