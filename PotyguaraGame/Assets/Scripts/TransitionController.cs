using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class TransitionController : MonoBehaviour
{
    private GameObject player;
    private bool isSkip = false;
    private int tempMode;
    private int tempSceneIndex = -1;
    private bool isForShowArea = false; 
    private bool isTheFirstAcess;

    public static TransitionController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void UpdateMainMenu(bool value)
    {
        if (isTheFirstAcess != value)
        {
            if (value)
            {
                GameObject.Find("MainMenu").transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("MainMenu").transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => LoadSceneAsync(1));
                GameObject.Find("MainMenu").transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Criar Perfil";
            }
            else
            {
                GameObject.Find("MainMenu").transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("MainMenu").transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => LoadSceneAsync(2));
                GameObject.Find("MainMenu").transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Iniciar Jogo";
            }
            isTheFirstAcess = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSkip && SceneManager.GetActiveScene().buildIndex == 3)
        {
            FindFirstObjectByType<GameForteController>().SetStartMode(tempMode);
            if (tempMode == 0)
                FindFirstObjectByType<GameForteController>().GetZombieModeButton().onClick.Invoke();
            else if (tempMode == 1)
                FindFirstObjectByType<GameForteController>().GetNormalModeButton().onClick.Invoke();

            isSkip = false;
        }

        if (isForShowArea && SceneManager.GetActiveScene().buildIndex == 2)
        {
            player = GameObject.FindWithTag("Player");
            FindFirstObjectByType<HeightController>().NewHeight(6.06f);
            player.transform.position = new Vector3(177.7f, 6.06f, 114.34f);
            player.transform.eulerAngles = Vector3.zero;

            isForShowArea = false;
        }

            if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!isTheFirstAcess)
            {
                GameObject.Find("MainMenu").transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => FindFirstObjectByType<PotyPlayerController>().DeletePerfil());
                GameObject.Find("MainMenu").transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                GameObject.Find("MainMenu").transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("MainMenu").transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    public int GetTempIndex()
    {
        return tempSceneIndex;
    }

    public void LoadSceneAsync(int sceneIndex)
    {
        tempSceneIndex = sceneIndex;
        StartCoroutine(LoadSceneAsyncRoutine(5));
    }

    IEnumerator LoadSceneAsyncRoutine(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            yield return null;
        }
    }

    public void LoadSceneWithTime(int sceneIndex, int time)
    {
        StartCoroutine(GoToSceneRoutine(sceneIndex, time));
    }

    IEnumerator GoToSceneRoutine(int sceneIndex, int time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(sceneIndex);
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
        isForShowArea = true;
        SceneManager.LoadSceneAsync(2);
    }

    public void TeleportExitShow()
    {
        FindFirstObjectByType<HeightController>().NewHeight(0f);
        player.transform.position = new Vector3(177.5f, 0f, 72f);
    }

    public void TeleportGallery()
    {
        player = GameObject.FindWithTag("Player");
        FindFirstObjectByType<HeightController>().NewHeight(0f);
        player.transform.position = new Vector3(132.53f, 0f, 15.69f);
        //player.transform.eulerAngles = new Vector3(0, -90, 0);
    }

    public void TeleportMeditationRoom()
    {
        player = GameObject.FindWithTag("Player");
        FindFirstObjectByType<HeightController>().NewHeight(0f);
        player.transform.position = new Vector3(160.36f, 0f, 10.88f);
        player.transform.eulerAngles = Vector3.zero;
    }

    public void TeleportGameForteZombieMode()
    {
        tempMode = 0;
        isSkip = true;
        SceneManager.LoadSceneAsync(3);
    }

    public bool GetIsSkip()
    {
        return isSkip;
    }

    public void TeleporGameForteNormalMode()
    {
        tempMode = 1;
        isSkip = true;
        SceneManager.LoadSceneAsync(3);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
