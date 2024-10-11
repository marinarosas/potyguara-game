using System.Collections;
using Unity.XR.CoreUtils.Bindings.Variables;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class LiftController : MonoBehaviour
{
    private int currentFloor = 0;
    private Animator ani;
    private GameObject leftDoor;
    private GameObject rightDoor;
    private Transform player;

    private void Start()
    {
        ani = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        leftDoor = transform.GetChild(0).gameObject;
        rightDoor = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
    }

    public void OpenTheDoors()
    {
        leftDoor.GetComponent<Animator>().Play("OpenLeft");
        rightDoor.GetComponent<Animator>().Play("OpenRight");
        player.GetComponent<HeightController>().NewHeight(player.transform.position.y);
    }

    public void CloseTheDoors()
    {
        leftDoor.GetComponent<Animator>().Play("CloseLeft");
        rightDoor.GetComponent<Animator>().Play("CloseRight");
        player.GetComponent<HeightController>().NewHeight(player.transform.position.y);
    }

    public void ChangeFloor(int value)
    {
        if(currentFloor != value)
        {
            player.parent = transform;
            Transform floor = transform.GetChild(6);
            player.GetComponent<HeightController>().NewHeight(floor.position.y+3.74f);
            if(value == 0)
            {
                ani.Play("DownTerreo");
            }
            else
            {
                ani.Play("MoveUpFirstFloor");
            }
            currentFloor = value;
        }
    }
}
