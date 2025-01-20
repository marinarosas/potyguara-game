using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverController : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Transform initialPosition = GameObject.Find("InitialPosition").transform;
        player.transform.parent.position = initialPosition.position;
        player.transform.parent.eulerAngles = new Vector3(0, initialPosition.eulerAngles.y, 0);
    }
    public void StartHover()
    {
        FindFirstObjectByType<ForceController>().SetBoolean();
    }
}
