using System.Collections;
using UnityEngine;

public class LiftController : MonoBehaviour
{
    private int currentFloor = 0;
    private Animator ani;
    private GameObject leftDoor;
    private GameObject rightDoor;

    public Transform player;

    private void Start()
    {
        ani = GetComponent<Animator>();
        leftDoor = transform.GetChild(0).gameObject;
        rightDoor = transform.GetChild(1).gameObject;
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
    }

    public void ChangeFloor(int value)
    {
        if(currentFloor != value)
        {
            player.parent = transform;
            if(value == 0)
            {
                if (currentFloor == 2)
                {
                    ani.Play("DownSecondFloor");
                }
                else
                {
                    ani.Play("DownTerreo");
                }
            }else if(value == 1)
            {
                if (currentFloor == 2)
                {
                    ani.Play("DownFirstFloor");
                }
                else
                {
                    ani.Play("MoveUpTerreo");
                }
            }
            else
            {                
                if (currentFloor == 1)
                {
                    ani.Play("MoveUpFirstFloor");
                }
                else
                {
                    ani.Play("MoveUpSecondFloor");
                }
            }
            currentFloor = value;
        }
    }
}
