using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class TransitionController : MonoBehaviour
{
    private GameObject player;
    private Vector3 initialPosition;
    private bool isSkip = false;
    private int tempMode;

    public static TransitionController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if(!isSkip)
            Start();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = GameObject.Find("InitialPosition").transform.position;
        FindFirstObjectByType<HeightController>().NewHeight(initialPosition.y);
        player.transform.position = initialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSkip && SceneManager.GetActiveScene().buildIndex == 2)
        {
            FindFirstObjectByType<GameForteController>().SetStartMode(tempMode);
            if(tempMode == 0)
            {
                FindFirstObjectByType<GameForteController>().GetZombieModeButton().onClick.Invoke();
            }else if(tempMode == 1)
            {
                FindFirstObjectByType<GameForteController>().GetNormalModeButton().onClick.Invoke();
            }
            isSkip = false;
        }

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            player.transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    public void LoadSceneAsync(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsyncRoutine(sceneIndex));
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

        //Launch the new scene
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
        FindFirstObjectByType<HeightController>().NewHeight(7.8f);
        player.transform.position = new Vector3(177.72f, 7.8f, 110.5f);
        player.transform.eulerAngles = Vector3.zero;
    }

    public void TeleportExitShow()
    {
        FindFirstObjectByType<HeightController>().NewHeight(1.84f);
        player.transform.position = new Vector3(177.72f, 1.84f, 72.92f);
    }

    public void TeleportGallery()
    {
        FindFirstObjectByType<HeightController>().NewHeight(11.6f);
        player.transform.position = new Vector3(205.4f, 11.6f, -6.8f);
        player.transform.eulerAngles = new Vector3(0, -90, 0);
    }

    public void TeleportGameForteZombieMode()
    {
        tempMode = 0;
        isSkip = true;
        SceneManager.LoadScene(2);
    }

    public bool GetIsSkip()
    {
        return isSkip;
    }

    public void TeleporGameForteNormalMode()
    {
        tempMode = 1;
        isSkip = true;
        SceneManager.LoadScene(2);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
