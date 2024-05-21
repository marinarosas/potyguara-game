using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class RightHandController : MonoBehaviour
{
    private Animator ani;
    public GameObject rightHandWithGun;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void ChangeHand()
    {
        FindObjectOfType<XRInputModalityManager>().rightController = rightHandWithGun;
        GetComponent<XRRayInteractor>().enabled = false;
        GetComponent<LineRenderer>().enabled = false;
        GetComponent<XRInteractorLineVisual>().enabled = false;
    }
    
    public void ShootGun()
    {
        ani.Play("FireGun");
    }
}
