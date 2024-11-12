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
    private float distanceForPlayer;
    private bool isDead = false;
    private Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Walking();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnerController spawner = FindFirstObjectByType<SpawnerController>();
        if(spawner.GetCurrentLevel() == 3)
        {
            player = GameObject.Find("Alvo").transform;
        }
        if(FindFirstObjectByType<GameForteController>().getMode() == 0)
        {
            distanceForPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (!isDead)
            {
                if(distanceForPlayer < distanceFollow)
                {
                    Follow();
                }
                else
                {
                    Walking();
                }

                if (distanceForPlayer <= distanceAttack) // for check if the enemy can attack the player
                {
                    Attack();
                }
            }
        }  
    }

    void Walking()
    {
        ani.SetBool("IsWalking", true);
        ani.SetBool("IsRunning", false);
        navMesh.acceleration = 5f;
        navMesh.speed = velocityWalking;
        navMesh.destination = player.position;
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

    void Idle()
    {
        ani.SetBool("IsWalking", false);
        ani.SetBool("IsRunning", false);
        navMesh.acceleration = 0f;
        navMesh.speed = 0f;
    }

    void Follow()
    {
        ani.SetBool("IsWalking", false);
        ani.SetBool("IsRunning", true);
        navMesh.acceleration = 7f;
        navMesh.speed = velocityPersecution;
        navMesh.destination = player.position;
    }

    void Attack()
    {
        navMesh.isStopped = true;
        ani.SetBool("isShouting", true);
        // End Game
        FindFirstObjectByType<GameForteController>().GameOver();
    }

    private void DestroyZumbi()
    {
        Destroy(gameObject);
    }
}
