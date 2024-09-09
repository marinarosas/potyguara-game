using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class DragonController : MonoBehaviour
{
    public Transform[] ways;
    private Transform currentIAPoint;
    private int count = 0;
    private Animator ani;
    private float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        currentIAPoint = ways[count];
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceForAIPoint = Vector3.Distance(currentIAPoint.position, transform.position);
        if(distanceForAIPoint < 2f)
        {
            if (count == ways.Length - 1)
            {
                count = 0;
            }
            else
            {
                count++;
            }
            currentIAPoint = ways[count];
        }
        Flying();
    }

    private void Flying()
    {
        ani.SetBool("isFlying", true);
        ani.SetBool("isRunning", false);
        if(currentIAPoint.position.x > transform.position.x){
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y - 90f/*90f*/, 0), 30 * Time.deltaTime);
        }
        else if(currentIAPoint.position.x < transform.position.x){
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y+90f/*270*/, 0), 30 * Time.deltaTime);
        }else if (currentIAPoint.position.z > transform.position.z)
        {
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + 90f/*360*/, 0), 30 * Time.deltaTime);
        }
        transform.position = Vector3.MoveTowards(transform.position, currentIAPoint.position, speed*Time.deltaTime);
    }
}
