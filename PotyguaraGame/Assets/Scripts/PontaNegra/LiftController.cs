using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LiftController : MonoBehaviour
{
    private int currentFloor = 0;

    public Transform player;
    public GameObject leftDoor;
    public GameObject rightDoor;

    private void Update()
    {
        if (currentFloor == 0)
        {
            //player.GetComponent<HeightController>().NewHeight(8.47f);
            if(transform.position.y < 8.47f)
            {
                //leftDoor.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).
                transform.position = new Vector3(214.0915f, Mathf.Lerp(transform.position.y, 8.47f, Time.deltaTime), -8.3801f);
            }
            else
            {
                leftDoor.GetComponent<Animator>().Play("OpenLeft");
                rightDoor.GetComponent<Animator>().Play("OpenRight");
            }
        }
        else if (currentFloor == 1)
        {
            //player.GetComponent<HeightController>().NewHeight(17.68f);
            if (transform.position.y < 17.68f)
            {
                transform.position = new Vector3(214.0915f, Mathf.Lerp(transform.position.y, 17.68f, Time.deltaTime), -8.3801f);
            }
        }
        else
        {
            //player.GetComponent<HeightController>().NewHeight(26.65f);
            if (transform.position.y < 26.65f)
            {
                transform.position = new Vector3(214.0915f, Mathf.Lerp(transform.position.y, 26.65f, Time.deltaTime), -8.3801f);
            }
        }
    }

    public void ChangeFloor(int value)
    {
        if(currentFloor != value)
        {
            player.parent = transform;
            /*if(value == 0)
            {
                player.GetComponent<HeightController>().NewHeight(8.47f);
            }else if(value == 1)
            {
                player.GetComponent<HeightController>().NewHeight(17.68f);
            }
            else
            {
                player.GetComponent<HeightController>().NewHeight(26.65f); 
            }*/
            currentFloor = value;
        }
    }
}
