using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public Material damageWall;
    public Material normalWall;
    private bool receivedDamage = false;

    public void setDamage(bool value)
    {
        receivedDamage = value;
    }

    // Update is called once per frame
    void Update()
    {
        if(receivedDamage)
        {
            GetComponent<MeshRenderer>().material = damageWall;
            receivedDamage = false;
        }
        else 
        {
            GetComponent<MeshRenderer>().material = normalWall;
        }
    }
}
