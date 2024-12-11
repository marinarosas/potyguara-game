using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class ZumbiController : MonoBehaviour
{
    private Transform player = null;
    private NavMeshAgent navMesh;
    //public AudioClip attackEnemy;

    private float velocityWalking = 0.6f, velocityPersecution = 3f;
    private float distanceFollow = 30f, distanceAttack = 3f;

    private float distanceForPlayer;
    private bool isDead = false;
    private SpawnerController spawner;
    private Animator ani;

    [SerializeField] private VisualEffect blood;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindFirstObjectByType<SpawnerController>();
        navMesh = GetComponent<NavMeshAgent>();
        ani = gameObject.transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            if(spawner.GetCurrentLevel() == 2)
            {
                player = GameObject.FindWithTag("PlayerStabilized").transform;
            }
            if (spawner.GetCurrentLevel() == 3)
            {
                player = GameObject.Find("Target").transform;
            }
            if (spawner.GetCurrentLevel() == 1)
            {
                WallController[] walls = FindObjectsByType<WallController>(FindObjectsSortMode.InstanceID);
                player = walls[Random.Range(0, walls.Length - 1)].transform;
            }
        }

        if (FindFirstObjectByType<GameForteController>().GetMode() == 0)
        {
            distanceForPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (!isDead)
            {
                if(distanceForPlayer < distanceFollow)
                {
                    Follow();
                    if (distanceForPlayer <= distanceAttack) // for check if the enemy can attack the player
                    {
                        Attack();
                    }
                }
                else
                {
                    Walking();
                }
                if(spawner.GetCurrentLevel() == 1)
                    if (distanceForPlayer < 0.5f)
                        ChangeTarget();
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
        ani.SetBool("IsWalking", false);
        ani.SetBool("IsRunning", false);
        if (!isDead)
        {
            //ani.SetBool("IsDead", true);
            isDead = true;
        }
        Invoke("DestroyZumbi", 200f);
    }

    void Idle()
    {
        ani.SetBool("IsWalking", false);
        ani.SetBool("IsRunning", false);
        ani.SetBool("isShouting", false);
        navMesh.acceleration = 0f;
        navMesh.speed = 0f;
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
        if(spawner.GetCurrentLevel() != 3)
            FindFirstObjectByType<GameForteController>().GameOver();
    }

    private void DestroyZumbi()
    {
        Destroy(gameObject);
    }

    private void ChangeTarget()
    {
        WallController[] walls = FindObjectsByType<WallController>(FindObjectsSortMode.InstanceID);
        player = walls[Random.Range(0, walls.Length - 1)].transform;
    }

    public void TriggerBleedEffect(Vector3 contactPosition)
    {
        if (blood == null){
            Debug.LogWarning("VisualEffect 'blood' não está atribuído.");
            return;
        }

        blood.transform.localPosition = contactPosition;
        blood.transform.LookAt(player.position);
        blood.SendEvent("Bleeding");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 hitPoint = contact.point;
            //Debug.Log("Ponto de impacto global: " + hitPoint);

            Vector3 hitPointLocal = transform.InverseTransformPoint(hitPoint);
            //Debug.Log("Ponto de impacto local: " + hitPointLocal);

            TriggerBleedEffect(hitPointLocal);
        }
    }
}
