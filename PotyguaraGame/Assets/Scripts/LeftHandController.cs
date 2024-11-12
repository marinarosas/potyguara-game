using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

using UnityEngine.XR.Interaction.Toolkit.Inputs;
using static UnityEngine.Rendering.DebugUI;

public class LeftHandController : MonoBehaviour
{
    public Animator ani;
    public bool controlMenu = false;
    private List<InputDevice> devices = new List<InputDevice>();

    private void Update()
    {
        /*InputDeviceCharacteristics leftHandCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftHandCharacteristics, devices);
        devices[0].TryGetFeatureValue(CommonUsages.secondaryButton, out bool Ybutton);*/
        if (/*Ybutton ||*/ Input.GetKeyDown(KeyCode.M)) // Y button pressed
        {
            GameObject menu = GameObject.FindWithTag("MainCamera").transform.GetChild(1).gameObject;
            if(menu != null)
            {
                controlMenu = !controlMenu;
                menu.SetActive(controlMenu);
            }
        }
    }

    public void ChangeHand()
    {
        InputDeviceCharacteristics leftHandCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftHandCharacteristics, devices);

        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>().enabled = false;
        GetComponent<LineRenderer>().enabled = false;
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void ResetHand()
    {
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>().enabled = true;
        GetComponent<LineRenderer>().enabled = true;
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public InputDevice GetTargetDevice()
    {
        return devices[0];
    }

    public void AnimationFinger()
    {
        ani.SetTrigger("IsFire");
        StartCoroutine(timeForStop());
    }
    
    IEnumerator timeForStop()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        ani.ResetTrigger("IsFire");
    }
}
