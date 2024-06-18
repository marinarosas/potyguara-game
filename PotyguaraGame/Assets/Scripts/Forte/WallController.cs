using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            gameObject.GetComponent<MeshRenderer>().material = damageWall;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!receivedDamage)
        {
            if (collision.gameObject.tag.Equals("Hand") || collision.gameObject.tag.Equals("Body"))
            {
                setDamage(true);
                FindObjectOfType<SpawnerController>().setWallsDestroyed();
            }
        }
    }
}
