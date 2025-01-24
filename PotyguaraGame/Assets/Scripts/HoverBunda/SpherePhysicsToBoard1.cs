using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePhysicsToBoard1 : MonoBehaviour
{
    public GameObject sphere;
    public GameObject board;
    void Start()
    {

    }


    void Update()
    {
        board.transform.position.Set(sphere.transform.position.x, sphere.transform.position.y, sphere.transform.position.z);
        board.transform.eulerAngles = Vector3.zero;
    }
}
