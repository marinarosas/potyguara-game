using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.Equals("Player"))
        {
            if(transform.parent.Equals("ForteDosReis"))
            {
                FindObjectOfType<TransitionController>().LoadScene(2);
            }
            else
            {
                FindFirstObjectByType<TransitionController>().LoadSceneAsync(1);
            }
        }
    }
}
