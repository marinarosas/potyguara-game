using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (transform.parent.name.Equals("ForteDosReis"))
            {
                FindObjectOfType<TransitionController>().LoadSceneAsync(2);
            }
            else
            {
                FindFirstObjectByType<TransitionController>().LoadSceneAsync(1);
            }
        }
    }
}
