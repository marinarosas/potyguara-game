using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using static UnityEngine.Rendering.DebugUI;

public class LeftHandController : MonoBehaviour
{

    public Animator ani;
    private List<InputDevice> devices = new List<InputDevice>();

    public void ChangeHand()
    {
        InputDeviceCharacteristics leftHandCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftHandCharacteristics, devices);

        GetComponent<XRRayInteractor>().enabled = false;
        GetComponent<LineRenderer>().enabled = false;
        GetComponent<XRInteractorLineVisual>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
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
