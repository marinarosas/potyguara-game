using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class DinoController : MonoBehaviour
{
    private bool isRunning = false;
    private List<InputDevice> devices = new List<InputDevice>();

    // Update is called once per frame
    void Update()
    {
        InputDeviceCharacteristics leftHandCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftHandCharacteristics, devices);
        devices[0].TryGetFeatureValue(CommonUsages.trigger, out float trigger);
        if (trigger > 0.1f) // Y button pressed
        {
            if (!isRunning)
            {
                // começa a experiência
                isRunning = true;
            }
        }
    }

    public void ResetExperience()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
