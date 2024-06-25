using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : MonoBehaviour
{
    private int currentFloor = 0;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public void ChangeFloor(int value)
    {
        if(currentFloor != value)
        {
            //player.parent = transform;
            if(value == 0)
            {
                //player.GetComponent<HeightController>().NewHeight(8.47f);
                transform.position = new Vector3(214.0915f, Mathf.Lerp(transform.position.y, 8.47f, Time.deltaTime), -8.3801f);
            }else if(value == 1)
            {
                //player.GetComponent<HeightController>().NewHeight(17.68f);
                transform.position = new Vector3(214.0915f, Mathf.Lerp(transform.position.y, 17.68f, Time.deltaTime), -8.3801f);
            }
            else
            {
                //player.GetComponent<HeightController>().NewHeight(26.65f);
                transform.position = new Vector3(214.0915f, Mathf.Lerp(transform.position.y, 26.65f, Time.deltaTime), -8.3801f); 
            }
            currentFloor = value;
        }
    }
}
