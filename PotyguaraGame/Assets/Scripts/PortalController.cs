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
                FindObjectOfType<GameController>().setHoverBunda(true);
            }
            else if(transform.parent.name == "ForteDosReis")
            {
                FindObjectOfType<GameController>().setForteDosReis(true);
            }
            else
            {
                FindObjectOfType<GameController>().setPontaNegra(true);
            }
        }
    }
}
