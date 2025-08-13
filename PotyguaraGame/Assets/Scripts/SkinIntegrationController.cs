using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SkinIntegrationController : MonoBehaviour
{
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice leftHandDevice;
    public float movementThreshold = 0.1f;
    Animator animator;
    private void Start()
    {
        Transform mainCam = transform.GetChild(0).GetChild(0);
        Transform avatar = transform.GetChild(0).GetChild(5);
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 5)
        {
            mainCam.GetChild(4).gameObject.SetActive(false);
            avatar.gameObject.SetActive(false);
        }
        else
        {
            mainCam.GetChild(4).gameObject.SetActive(true);
            avatar.gameObject.SetActive(true);
            GetSkin();
        }
    }

    private void GetSkin()
    {
        int skinIndex = FindFirstObjectByType<PotyPlayerController>().GetIndex();
        int skinMaterial = FindFirstObjectByType<PotyPlayerController>().GetMaterial();
        int skinGender = FindFirstObjectByType<PotyPlayerController>().GetGender();
        Transform avatar = transform.GetChild(0).GetChild(5);
        avatar.GetComponent<SetSkin>().SetSkinAvatar(skinIndex, skinMaterial, skinGender);
        animator = avatar.GetComponent<SetSkin>().UpdateAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        InputDeviceCharacteristics leftHandedController = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftHandedController, devices);
        if (leftHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickInput))
        {
            if (joystickInput.magnitude > movementThreshold)
            {
                Debug.Log("Personagem se movimentou");
                PotyPlayerController.Instance.playerData.position_x = transform.position.x;
                PotyPlayerController.Instance.playerData.position_y = transform.position.y;
                PotyPlayerController.Instance.playerData.position_z = transform.position.z;
                NetworkManager.Instance.SendPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z));
            }
        }
    }
}
