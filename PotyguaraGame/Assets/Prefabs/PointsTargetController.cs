using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsTargetController : MonoBehaviour
{
    public bool receivedDamage = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (!receivedDamage)
            {
                if (transform.name.Equals("Yellow"))
                {
                    FindFirstObjectByType<GameForteController>().SetCurrentScore(10);
                }
                if (transform.name.Equals("Red"))
                {
                    collision.gameObject.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "10";
                    collision.gameObject.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                    collision.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                }
                if (transform.name.Equals("Blue"))
                {
                    collision.gameObject.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "5";
                    collision.gameObject.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                    collision.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                }
                receivedDamage = true;
            }
        }
    }
}
