using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    private Animator ani;
    public bool receivedDamage = false;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (!receivedDamage)
            {
                collision.gameObject.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "0";
                collision.gameObject.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                collision.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                receivedDamage = true;
            }
        }
    }
}