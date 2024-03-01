using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public Transform player;

    private float distanceToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if(distanceToPlayer > 40)
        {
            GetComponent<AudioSource>().volume = 0.15f;
        }else if(distanceToPlayer > 20)
        {
            GetComponent<AudioSource>().volume = 0.25f;
        }else if(distanceToPlayer > 10)
        {
            GetComponent<AudioSource>().volume = 0.50f;
        }else if(distanceToPlayer > 0)
        {
            GetComponent<AudioSource>().volume = 0.75f;
        }
    }
}
