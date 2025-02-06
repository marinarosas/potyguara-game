using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MainCamera"))
        {
            if (transform.parent.name.Equals("ForteDosReis"))
            {
                FindObjectOfType<TransitionController>().LoadSceneAsync(3);
            }
            else if(transform.parent.name.Equals("PontaNegra"))
            {
                FindFirstObjectByType<TransitionController>().LoadSceneAsync(2);
            }
            else if(transform.parent.name.Equals("HoverBunda"))
            {
                FindFirstObjectByType<TransitionController>().LoadSceneAsync(4);
            }
            else
            {
                FindFirstObjectByType<TransitionController>().TeleportMeditationRoom();
                transform.parent.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
