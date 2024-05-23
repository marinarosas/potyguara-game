using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GameController : MonoBehaviour
{
    private bool startHoverBunda = false;
    private bool startForteDosReis = false;
    private bool startPontaNegra = false;
    private GameObject player;

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

    public void LevelOne()
    {
        player.transform.position = new Vector3(746.14f, 9.3f, 400.35f);
        player.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public void LevelTwo()
    {
        player.transform.position = new Vector3(654.91f, 18.6f, 400.95f);
        player.transform.eulerAngles = new Vector3(0, 180, 0);
    }
    public void LevelThree()
    {
        player.transform.position = new Vector3(710.36f, 8.35f, 401.15f);
        player.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
