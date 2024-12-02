using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            if(FindFirstObjectByType<GameForteController>().GetMode() == 0)
            {
                FindFirstObjectByType<GameForteController>().GetZombieModeButton().onClick.Invoke();
            }else if(FindFirstObjectByType<GameForteController>().GetMode() == 1)
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

    public IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while(!asyncLoad.isDone)
        {
            yield return null;
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
