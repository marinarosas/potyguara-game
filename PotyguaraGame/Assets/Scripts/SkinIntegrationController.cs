using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SkinIntegrationController : MonoBehaviour
{
    public XRNode inputSource = XRNode.LeftHand; // Define qual controle ser� utilizado

    private Animator animator; // Refer�ncia ao Animator do avatar;
    private Vector2 inputAxis;

    private void Start()
    {
        FindObjectOfType<NetworkManager>().GetSkin();
        Transform mainCam = transform.GetChild(0).GetChild(0);
        Transform avatar = transform.GetChild(0).GetChild(5);
        animator = transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Animator>();
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 5)
            transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
        else
            transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
    }

    public void SetSkin(int skinIndex, int skinMaterial, int skinGender)
    {
        Transform avatar = transform.GetChild(0).GetChild(5);
        avatar.GetComponent<SetSkin>().SetSkinAvatar(skinIndex, skinMaterial, skinGender);
    }

    // Update is called once per frame
    void Update()
    {
        //InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        //device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        //bool isMoving = inputAxis.magnitude > 0.1f;
        /*if (animator != null)
        {
            animator.SetBool("isWalking", isMoving);
        }*/
        if (animator != null)
        {
            if (!animator.GetBool("isWalking"))
            {
                Transform mainCam = transform.GetChild(0).GetChild(0);
                Transform avatar = transform.GetChild(0).GetChild(5);

                avatar.rotation = Quaternion.Euler(new Vector3(avatar.eulerAngles.x, mainCam.eulerAngles.y, avatar.eulerAngles.z));
            }
            else
                transform.GetChild(2).rotation = transform.rotation;
        }
    }
}
