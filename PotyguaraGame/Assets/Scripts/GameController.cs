using System;
using System.Runtime.CompilerServices;
using TMPro;
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
    }

    public void LoadScene(int number)
    {
        if(SceneManager.GetActiveScene().buildIndex != number)
            SceneManager.LoadScene(number);
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
}
