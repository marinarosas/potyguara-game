using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float timeToDestroy = 2.5f;
    private void Start()
    {
        Invoke("autoDestroy", timeToDestroy);
    }
    private void autoDestroy()
    {
        Destroy(gameObject);
    }
}
