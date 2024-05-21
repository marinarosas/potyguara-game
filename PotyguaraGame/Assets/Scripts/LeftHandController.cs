using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using static UnityEngine.Rendering.DebugUI;

public class LeftHandController : MonoBehaviour
{
    public Animator ani;

    public void ChangeHand()
    {
        GetComponent<XRRayInteractor>().enabled = false;
        GetComponent<LineRenderer>().enabled = false;
        GetComponent<XRInteractorLineVisual>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
    }
    
    public void AnimationFinger()
    {
        ani.SetTrigger("IsFire");
    }

}
