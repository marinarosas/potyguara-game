using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverController : MonoBehaviour
{
    public GameObject board;
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
            board.transform.position = point.transform.position;
            board.transform.GetChild(0).eulerAngles = new Vector3(0f, point.transform.eulerAngles.y, 0f);
            board.transform.eulerAngles = new Vector3(0f, point.transform.eulerAngles.y, 0f);
            board.GetComponent<Rigidbody>().isKinematic = false;
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
