using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZumbiController : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent navMesh;
    //public AudioClip attackEnemy;

    private float velocityWalking = 0.6f, velocityPersecution = 3f;
    private float distanceFollow = 30f, distanceAttack = 3f;

    private float timeForAttack = 1.5f;
    private float distanceForPlayer, distanceForAIPoint;
    private bool followSomething, isDead = false;
    private Transform AIPointCurrent = null;
    private Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        AIPointCurrent = FindObjectOfType<SpawnerController>().getIAPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<GameController>().getMode() == 0)
        {
            if (AIPointCurrent == null)
            {
                AIPointCurrent = FindObjectOfType<SpawnerController>().getIAPoint();
            }
            distanceForPlayer = Vector3.Distance(player.transform.position, transform.position);
            distanceForAIPoint = Vector3.Distance(AIPointCurrent.position, transform.position);

            RaycastHit hit;
            Vector3 from = transform.position;
            Vector3 to = player.position;
            Vector3 direction = to - from;
            if (!isDead)
            {
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
                    else
                    {
                        followSomething = false;
                        Walking();
                    }
                }

                if (distanceForPlayer <= distanceAttack) // for check if the enemy can attack the player
                {
                    Attack();
                }

                if (distanceForAIPoint <= 2f) // for change the enemy's random destiny
                {
                    AIPointCurrent = FindObjectOfType<SpawnerController>().getIAPoint();
                    Walking();
                }
            }
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
        ani.SetBool("IsWalking", false);
        ani.SetBool("IsRunning", false);
        isDead = true;
        Invoke("DestroyZumbi", 4f);
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
        navMesh.isStopped = true;
        ani.SetBool("isShouting", true);
        // End Game
        FindObjectOfType<GameController>().GameOver();
    }

    private void DestroyZumbi()
    {
        Destroy(gameObject);
    }
}
