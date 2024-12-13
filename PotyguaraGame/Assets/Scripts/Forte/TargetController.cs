using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    public bool receivedDamage = false;
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private float speed = 0.3f;
    void Start()
    {
        startPoint = transform.position;
        endPoint = new Vector3(transform.position.x, transform.position.y, 390.1875f);
    }
    void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            float pp = Mathf.PingPong(Time.deltaTime * speed, 1f);
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(startPoint.z, endPoint.z, 5f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (!receivedDamage)
            {
                receivedDamage = true;
            }
        }
    }
}