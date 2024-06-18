using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorController : MonoBehaviour
{
    public GameObject canvasAssociated;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            canvasAssociated.GetComponent<FadeController>().FadeIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            canvasAssociated.GetComponent<FadeController>().FadeOut();
        }
    }
}
