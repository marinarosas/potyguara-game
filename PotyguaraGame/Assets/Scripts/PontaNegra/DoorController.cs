using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entrou no elevador");
        FindObjectOfType<LiftController>().CloseTheDoors();
        FindObjectOfType<HeightController>().SetBool(true);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Saiu do Elevador");
        FindObjectOfType<LiftController>().CloseTheDoors();
        FindObjectOfType<HeightController>().SetBool(false);
    }
}
