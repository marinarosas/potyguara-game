using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<LiftController>().CloseTheDoors();
        FindObjectOfType<HeightController>().SetBool(true);
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<LiftController>().CloseTheDoors();
        FindObjectOfType<HeightController>().SetBool(false);
    }
}
