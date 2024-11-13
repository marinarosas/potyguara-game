using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    public int health { get; set; } = 100;
    public bool receivedDamage = false;
    public float timeout = 2;


    private void Update()
    {
        if (receivedDamage)
        {
            if (timeout <= 0)
            {
                receivedDamage = false;
                timeout = 2f;
            }
            else
            {
                timeout -= Time.deltaTime;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Head") || collision.gameObject.CompareTag("Body"))
        {
            if (!receivedDamage)
            {
                receivedDamage = true;
                health -= 5;
                Debug.Log(health);
                transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = health + "";
                if (health <= 0)
                    FindFirstObjectByType<GameForteController>().GameOver();
            }
        }
    }
}