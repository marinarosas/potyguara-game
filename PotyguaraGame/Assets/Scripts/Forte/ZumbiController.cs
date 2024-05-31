using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ZumbiController : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMesh;
    //public AudioClip attackEnemy;

    private float velocityWalking = 0.6f, velocityPersecution = 3f;
    private float distanceFollow = 30f, distanceAttack = 3f;

    private float timeForAttack = 1.5f;
    private float distanceForPlayer, distanceForAIPoint;
    private bool followSomething, attackSomething;

    public Transform[] destinyRandow;
    private int AIPointCurrent;
    private Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        AIPointCurrent = Random.Range(0, destinyRandow.Length);
    }

    // Update is called once per frame
    void Update()
    {
        distanceForPlayer = Vector3.Distance(player.transform.position, transform.position);
        distanceForAIPoint = Vector3.Distance(destinyRandow[AIPointCurrent].transform.position, transform.position);

        RaycastHit hit;
        Vector3 from = transform.position;
        Vector3 to = player.position;
        Vector3 direction = to - from;
        if (Physics.Raycast(transform.position, direction, out hit, 1000)) // for to see if the player is in the enemy perception ray
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                if(distanceForPlayer < distanceFollow)
                {
                    Follow();
                    followSomething = true;
                }
                else
                {
                    followSomething = false;
                    Walking();
                    
                }
            }
        }

        if (distanceForPlayer <= distanceAttack) // for check if the enemy can attack the player
        {
            Attack();
        }

        if (distanceForAIPoint <= 2f) // for change the enemy's random destiny
        {
            AIPointCurrent = Random.Range(0, destinyRandow.Length);
            Walking();
        }
    }

    void Walking()
    {
        if (!followSomething)
        {
            ani.SetBool("IsWalking", true);
            ani.SetBool("IsRunning", false);
            navMesh.acceleration = 5f;
            navMesh.speed = velocityWalking;
            navMesh.destination = destinyRandow[AIPointCurrent].transform.position;
        }
    }

    void Follow()
    {
        ani.SetBool("IsWalking", false);
        ani.SetBool("IsRunning", true);
        navMesh.acceleration = 8f;
        navMesh.speed = velocityPersecution;
        navMesh.destination = player.position;
    }

    void Attack()
    {
        attackSomething = true;
        navMesh.isStopped = true;
        ani.SetBool("isShouting", true);
        // End Game
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            navMesh.isStopped = true;
            ani.SetBool("IsDead", true);
            Debug.Log("Acertou!!!");
        }
    }
}
