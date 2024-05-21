using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class GameController : MonoBehaviour
{
    private bool startHoverBunda = false;
    private bool startForteDosReis = false;
    private bool startPontaNegra = false;
    private UnityEngine.XR.InputDevice targetDevice;

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
}
