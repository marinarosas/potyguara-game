using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MainCamera"))
        {
            if (transform.name.Equals("Detector1"))
            {
                FindFirstObjectByType<LiftShowController>().ChangeThePoint(1);
                FindFirstObjectByType<LiftShowController>().catraca2.GetComponent<Animator>().Play("CatracaOpen");
            }
            else
            {
                FindFirstObjectByType<LiftShowController>().ChangeThePoint(0);
                FindFirstObjectByType<LiftShowController>().catraca1.GetComponent<Animator>().Play("CatracaOpen");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MainCamera"))
        {
            if (transform.name.Equals("Detector1"))
            {
                FindFirstObjectByType<LiftShowController>().ChangeThePoint(1);
                FindFirstObjectByType<LiftShowController>().catraca2.GetComponent<Animator>().Play("CatracaClose");
            }
            else
            {
                FindFirstObjectByType<LiftShowController>().ChangeThePoint(0);
                FindFirstObjectByType<LiftShowController>().catraca1.GetComponent<Animator>().Play("CatracaClose");
            }
        }
    }
}
