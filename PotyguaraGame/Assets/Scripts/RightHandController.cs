using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class RightHandController : MonoBehaviour
{
    public Animator ani;
    private List<InputDevice> devices = new List<InputDevice>();

    public void ChangeHand()
    {
        InputDeviceCharacteristics rightHandCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightHandCharacteristics, devices);

        GetComponent<XRRayInteractor>().enabled = false;
        GetComponent<LineRenderer>().enabled = false;
        GetComponent<XRInteractorLineVisual>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void ResetHand()
    {
        GetComponent<XRRayInteractor>().enabled = true;
        GetComponent<LineRenderer>().enabled = true;
        GetComponent<XRInteractorLineVisual>().enabled = true;
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
