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
                FindObjectOfType<GameController>().LoadScene(3);
            }
            else if(transform.parent.name == "ForteDosReis")
            {
                FindObjectOfType<GameController>().LoadScene(2);
            }
            else
            {
                FindObjectOfType<GameController>().LoadScene(1);
            }
        }
    }
}
