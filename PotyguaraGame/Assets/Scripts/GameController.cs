using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Transição de Cena")]
    private bool startHoverBunda = false;
    private bool startForteDosReis = false;
    private bool startPontaNegra = false;
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
   

    // Update is called once per frame
    void Update()
    {
        if (isSkip)
        {
            FindObjectOfType<SpawnerController>().SetLevel();
            FindObjectOfType<GameForteController>().setStartMode(0);
            GameObject.Find("PontaNegra").SetActive(false);
            GameObject.Find("MainMenu").GetComponent<FadeController>().FadeOut();
            GameObject.Find("MainMenu").SetActive(false);
            isSkip = false;
        }

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
    }

    public void LoadScene(int number)
    {
        if(SceneManager.GetActiveScene().buildIndex != number)
            SceneManager.LoadScene(number);
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
        SceneManager.LoadScene("ForteDosReisMagos");
    }
}
