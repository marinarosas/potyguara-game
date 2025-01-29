using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverController : MonoBehaviour
{
    public GameObject player;
    private void Start()
    {
        StartPositionOfGame();
    }
    public void StartHover()
    {
        Invoke("ModifyPositionOfPlayer", 2f);
    }

    public void ModifyPositionOfPlayer() {
        GameObject point = GameObject.Find("PointOfStart");
        if(point != null)
        {
            player.transform.position = point.transform.position;
            player.transform.rotation = new Quaternion(0, -0.205623f, 0f, 0);
        }
    }

    public void RestartTheGame()
    {
        StartPositionOfGame();
    }

    public void StartPositionOfGame()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Transform initialPosition = GameObject.Find("InitialPosition").transform;
        player.transform.parent.position = initialPosition.position;
        player.transform.parent.eulerAngles = new Vector3(0, initialPosition.eulerAngles.y, 0);
    }
}
