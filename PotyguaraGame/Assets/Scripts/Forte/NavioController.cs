using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class NavioController : MonoBehaviour
{
    private NavMeshAgent navMesh;
    private float speed;
    private float distanceForAIPoint;
    private bool followSomething = false, isDead = false;
    private Transform AIPointCurrent = null;
    private Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        //AIPointCurrent = FindObjectOfType<SpawnerController>().getIAPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GameController>().getMode() == 1)
        {
            distanceForAIPoint = Vector3.Distance(AIPointCurrent.position, transform.position);

            if (!isDead)
            {
                Walking();
                if (distanceForAIPoint <= 2f) // for change the enemy's random destiny
                {
                    AIPointCurrent = FindObjectOfType<SpawnerController>().getIAPoint();
                }
            }            
        }
    }
    void Walking()
    {
        if (!followSomething)
        {
            navMesh.acceleration = 5f;
            navMesh.speed = 3f;
            navMesh.destination = AIPointCurrent.position;
        }
    }

    public void Dead()
    {
        navMesh.isStopped = true;
        if (!isDead)
        {
            ani.SetBool("IsDead", true);
        }
        isDead = true;
        Invoke("DestroyNavio", 4f);
    }

    private void DestroyNavio()
    {
        Destroy(gameObject);
    }
}
