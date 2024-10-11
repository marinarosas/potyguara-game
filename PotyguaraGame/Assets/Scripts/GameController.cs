using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int gameMode = -1;
    private bool isSkip = false;

    private GameObject player;

    public static GameController instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public bool GetIsSkip()
    {
        return isSkip;
    }

    public void SetIsSkip(bool value)
    {
        isSkip = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSkip)
        {
            if(gameMode == 0)
            {
                FindObjectOfType<SpawnerController>().SetLevel();
                GameObject.Find("PontaNegra").SetActive(false);
                GameObject.Find("MainMenu").GetComponent<FadeController>().FadeOut();
                GameObject.Find("MainMenu").SetActive(false);
            }else if(gameMode == 1)
            {
                FindObjectOfType<SpawnerController>().SetSpawn();
                GameObject.Find("PontaNegra").SetActive(false);
                GameObject.Find("MainMenu").GetComponent<FadeController>().FadeOut();
                GameObject.Find("MainMenu").SetActive(false);
            }
            isSkip = false;
        }
    }

    public void LoadScene(int number)
    {
        try
        {
            if (SceneManager.GetActiveScene().buildIndex != number)
                SceneManager.LoadScene(number);
        }
        catch (Exception e)
        {
            Debug.Log("Error when load scenes: " + e);
        }
    }

    public void TeleportEnterShow()
    {
        player.GetComponent<HeightController>().NewHeight(7.8f);
        player.transform.position = new Vector3(177.72f, 7.8f, 110.5f);
        player.transform.eulerAngles = Vector3.zero;
    }

    public void TeleportExitShow()
    {
        player.GetComponent<HeightController>().NewHeight(1.84f);
        player.transform.position = new Vector3(177.72f, 1.84f, 72.92f);
    }

    public void TeleportGallery()
    {
        player.GetComponent<HeightController>().NewHeight(11.6f);
        player.transform.position = new Vector3(205.4f, 11.6f, -6.8f);
        player.transform.eulerAngles = new Vector3(0, -90, 0);
    }

    public void TeleportGameForteZombieMode()
    {
        isSkip = true;
        gameMode = 0;
        SceneManager.LoadScene("ForteDosReisMagos");
    }

    public void TeleporGameForteNormalMode()
    {
        isSkip = true;
        gameMode = 1;
        SceneManager.LoadScene("ForteDosReisMagos");
    }

    public void setStartMode(int value)
    {
        gameMode = value;
    }

    public int getMode()
    {
        return gameMode;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
