using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(transform.parent.name == "HoverBunda")
            {
                FindObjectOfType<TransitionController>().LoadScene(3);
            }
            else if(transform.parent.name == "ForteDosReis")
            {
                FindObjectOfType<TransitionController>().LoadScene(2);
            }
            else
            {
                FindObjectOfType<TransitionController>().LoadScene(1);
            }
        }
    }
}
