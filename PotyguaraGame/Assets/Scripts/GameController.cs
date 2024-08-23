using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // GENERAL
    private bool startHoverBunda = false;
    private bool startForteDosReis = false;
    private bool startPontaNegra = false;
    private GameObject player;

    // FORTE DOS REIS MAGOS
    private float count = 10;
    private GameObject timer;
    private int levelCurrent = 1;
    private bool startTimer = false;
    private int currentPoints = 0;
    private int totalPoints = 0;
    private int modeGame = -1;

    //Ponta Negra
    //HoverBunda

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void setHoverBunda(bool value)
    {
        startHoverBunda = value;
    }

    public void setForteDosReis(bool value)
    {
        startForteDosReis = value;
    }

    public void setPontaNegra(bool value)
    {
        startPontaNegra = value;
    }

    public void setStartMode(int value)
    {
        modeGame = value;
    }

    public int getMode()
    {
        return modeGame;
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
    
    public void ResetCount()
    {
        count = 10;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (startHoverBunda)
            {
                startHoverBunda = false;
                SceneManager.LoadScene("HoverBunda");
            }

            if (startForteDosReis)
            {
                startForteDosReis = false;
                SceneManager.LoadScene("ForteDosReisMagos");
            }

            if(startPontaNegra)
            {
                startPontaNegra = false;
                SceneManager.LoadScene("PontaNegra");
            }
        }
        catch (Exception e)
        {
            Debug.Log("Erro when load scenes: " + e);
        }

        if (startTimer)
        {
            InitTimer();
        }
    }

    public void TeleportEnterShow()
    {
        player.GetComponent<HeightController>().NewHeight(7.6f);
        player.transform.position = new Vector3(177.72f, 7.6f, 112.13f);
    }

    public void TeleportExitShow()
    {
        player.GetComponent<HeightController>().NewHeight(1.3f);
        player.transform.position = new Vector3(177.72f, 1.4f, 72.92f);
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
    public void InitTimer()
    {
        // timer bar
        timer = GameObject.FindGameObjectWithTag("Time");
        if(count > 0)
        {
            count -= Time.deltaTime;
            timer.transform.GetChild(0).GetComponent<Text>().text = count.ToString("F0");
            timer.GetComponent<UnityEngine.UI.Image>().fillAmount -= Time.deltaTime / 9.6f;
            if(count <= 0)
            {
                count = 0;
                startTimer = false;
                FindObjectOfType<SpawnerController>().SetSpawn();
                if(levelCurrent == 1 )
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
}
