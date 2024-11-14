using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils.Bindings.Variables;
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
    private bool startCurve = false;
    private int numVoltas = 2;
    private float currentValueCurve = 0f;
    private bool startDragon = false;
    
    // Start is called before the first frame update
    void Start()
    {
        currentIAPoint = ways[count];
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startDragon)
        {
            float distanceForAIPoint = Vector3.Distance(currentIAPoint.position, transform.position);
            if (distanceForAIPoint < 0.4f)
            {
                if (count == ways.Length - 1)
                {
                    numVoltas--;
                    if (numVoltas == 0)
                    {
                        count = 0;
                        Destroy(gameObject, 15f);
                    }
                    else
                    {
                        count = 3;
                    }
                }
                else
                {
                    count++;
                }
                currentIAPoint = ways[count];
            }
            Flying();
        }
    }

    private void Flying()
    {
        ani.SetBool("isFlying", true);
        ani.SetBool("isRunning", false);
        if (numVoltas == 0)
        {
            if (transform.eulerAngles.y > currentValueCurve - 180f)
            {
                transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y - 90f, 0), 30 * Time.deltaTime);
            }
        }
        else
        {
            if (startCurve)
            {
                if (transform.eulerAngles.y < currentValueCurve)
                {
                    transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + 90f, 0), 30 * Time.deltaTime);
                }
                Invoke("setBool", 3f);
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, currentIAPoint.position, speed*Time.deltaTime);
    }

    private void setBool()
    {
        startCurve = false;
    }

    public void setStartDragon(bool value)
    {
        startDragon = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisor!!");
        if (other.tag.Equals("pointCurve"))
        {
            startCurve = true;
            currentValueCurve = transform.eulerAngles.y+90;
        }
    }

}
