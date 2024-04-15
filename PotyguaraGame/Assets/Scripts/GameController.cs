using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool startHoverBunda = false;
    private bool startForteDosReis = false;

    public void setHoverBunda(bool value)
    {
        startHoverBunda = value;
    }

    public void setForteDosReis(bool value)
    {
        startForteDosReis = value;
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

    }
}
