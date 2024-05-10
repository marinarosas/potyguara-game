using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
    NetworkManager nm;
    public string playerId {
        get {
            return nm.playerId;
        }
    }

    // variável para armazenar a úlltime vez que a posição foi enviada
    float lastSentPositionTime = 0;

    // Quantas vezes por segundo enviar a posição para o servidor
    public float updateServerTimesPerSecond = 10;
    
    // Start is called before the first frame update
    void Awake()
    {
        nm = NetworkManager.Instance;
    }

    // A cada update, enviar a posição para o servidor
    // A posição é enviada a cada 1/updateServerTimesPerSecond segundos
    void Update()
    {
        if (Time.time - lastSentPositionTime > 1/updateServerTimesPerSecond)
        {
            nm.SendPosition(transform.position);
            lastSentPositionTime = Time.time;
        }

    }
}
