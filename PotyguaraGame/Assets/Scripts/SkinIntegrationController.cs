using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SkinIntegrationController : MonoBehaviour
{
    public XRNode inputSource = XRNode.LeftHand; // Define qual controle será utilizado
    public float speed = 1.5f; // Velocidade do movimento

    private Animator animator; // Referência ao Animator do avatar;
    private Vector2 inputAxis;

    private void Start()
    {
        animator = transform.GetChild(2).GetComponent<Animator>();
        if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 5)
            transform.GetChild(2).gameObject.SetActive(false);
        else
            transform.GetChild(2).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        //device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        //bool isMoving = inputAxis.magnitude > 0.1f;
        /*if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }*/

        transform.GetChild(2).rotation = transform.rotation;
    }
}
