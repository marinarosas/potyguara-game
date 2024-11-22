using UnityEngine;
using UnityEngine.AI;

public class BulletController : MonoBehaviour
{
    private bool MarkedPontuacion = false;

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.CompareTag("Zumbi"))
        { 
            if(!MarkedPontuacion)
            {
                collision.gameObject.GetComponent<ZumbiController>().Dead();
                FindFirstObjectByType<GameForteController>().SetCurrentScore(1);
                MarkedPontuacion = true;
                Destroy(gameObject);
            }
        }*/

       if (collision.gameObject.CompareTag("Head"))
       {
           Debug.Log("Acertou a cabeça");

           var parent = collision.gameObject.transform.parent;
           while (parent.gameObject.layer != 7)
           {
               parent = parent.parent;
           }
           parent.gameObject.GetComponent<ZumbiController>().Dead();

           if (!MarkedPontuacion)
           {
               FindFirstObjectByType<GameForteController>().SetCurrentScore(2);
               MarkedPontuacion = true;
           }
            Destroy(gameObject);
       }

       if (collision.gameObject.CompareTag("Body"))
       {
           Debug.Log("Acertou o corpo");

           var parent = collision.gameObject.transform.parent;
           while(parent.gameObject.layer != 7)
           {
               parent = parent.parent;
           }
           parent.gameObject.GetComponent<ZumbiController>().Dead();

           if (!MarkedPontuacion)
           {
               FindFirstObjectByType<GameForteController>().SetCurrentScore(1);
               MarkedPontuacion = true;
           }
            Destroy(gameObject);
       }
    }
}
