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
    private float distanceFollow = 20f, distancePerception = 30f, distanceAttack = 2f;

    private float timeForAttack = 1.5f;
    private float distanceForPlayer, distanceForAIPoint;
    private bool followSomething, attackSomething, teste;

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
        if (Physics.Raycast(transform.position, direction, out hit, 1000) && distanceForPlayer < distancePerception) // for to see if the player is in the enemy perception ray
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Follow();
                followSomething = true;
            }
            else
            {
                Walking();
                teste = false;
                followSomething = false;
            }
        }
        if (distanceForPlayer > distancePerception)
        {
            Walking();
        }

        if (distanceForPlayer <= distanceAttack) // para verificar se o inimigo pode atacar o player
        {
            Attack();
        }

        if (distanceForAIPoint <= 2f) // para mudar o destino aleatorio do inimigo
        {
            AIPointCurrent = Random.Range(0, destinyRandow.Length);
            Walking();
        }

        if (teste)
        {
            ani.SetBool("IsWalking", false);
            ani.SetBool("IsRunning", true);
            //countPersecution += Time.deltaTime;
        }
        /*if (countPersecution >= 5f)
        {
            teste = false;
            followSomething = false;
            countPersecution = 0f;
        }*/

        if (attackSomething)
        {
            ani.SetBool("IsAttacking", true);
            //GetComponent<AudioSource>().PlayOneShot(attackEnemy);
            //countAttack += Time.deltaTime;
        }
        /*if (countAttack >= timeForAttack && distanceForPlayer <= distanceAttack)
        {
            attackSomething = true;
            countAttack = 0f;
            player.GetComponent<Animator>().SetBool("isDead", true);
            SceneManager.LoadScene(1); // derrota
        }
        else if (countAttack >= timeForAttack && distanceForPlayer > distanceAttack)
        {
            ani.SetBool("isAtacking", false);
            attackSomething = false;
            countAttack = 0f;
        }*/
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
        else
        {
            teste = true;
        }
    }

    void Follow()
    {
        ani.SetBool("IsWalking", false);
        ani.SetBool("IsRunning", true);
        StartCoroutine(WaitAnimation());
    }

    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        navMesh.acceleration = 8f;
        navMesh.speed = velocityPersecution;
        navMesh.destination = player.position;
    }

    void Attack()
    {
        attackSomething = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            ani.SetBool("IsDead", true);
            Debug.Log("Acertou!!!");
            Destroy(gameObject);
        }
    }
}
