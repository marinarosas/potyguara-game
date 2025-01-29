using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Entrou no elevador");
            other.gameObject.transform.parent = transform.parent;
            FindObjectOfType<LiftController>().CloseTheDoors();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Saiu do elevador");
            other.gameObject.transform.parent = null;
            FindObjectOfType<LiftController>().CloseTheDoors();
            FindFirstObjectByType<HeightController>().SetBool(false);
        }
    }
}
