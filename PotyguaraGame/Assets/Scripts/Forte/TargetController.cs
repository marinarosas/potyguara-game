using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    public bool receivedDamage = false;

    private void Update()
    {
        transform.position = Mathf.Lerp(Mathf.PingPong());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Body"))
        {
            if (!receivedDamage)
            {
                receivedDamage = true;
            }
        }
    }
}