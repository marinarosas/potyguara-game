using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CanonBallController : MonoBehaviour
{
    private Rigidbody rig;

    public float speed = 2;
    public GameObject canonBallPrefab;
    public Transform attach;
    public Transform canon;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
            Invoke("NewCanonBall", 3f);
        }
    }

    void NewCanonBall()
    {
        Instantiate(canonBallPrefab, attach.position, Quaternion.identity, canon);
    }
}
