using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.XR;

public class RightHandController : MonoBehaviour
{
    public Animator ani;
    public bool isRight = false;
    private List<InputDevice> devices = new List<InputDevice>();

    public bool GetHand()
    {
        return isRight;
    }
    public void ChangeHand()
    {
        InputDeviceCharacteristics rightHandCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightHandCharacteristics, devices);

        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>().enabled = false;
        GetComponent<LineRenderer>().enabled = false;
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
        isRight = true;
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
        yield return new WaitForSecondsRealtime(0.4f);

        ani.ResetTrigger("IsFire");
    }
}
