using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowRanking()
    {
        string ranking = FindObjectOfType<NetworkManager>().GetRanking();
        string[] playersRanking = ranking.Split('|');
        transform.GetChild(2).GetComponent<Text>().text = playersRanking[0];
        transform.GetChild(3).GetComponent<Text>().text = playersRanking[1] != null ? playersRanking[1] + "pt" : " - ";
        transform.GetChild(4).GetComponent<Text>().text = playersRanking[2] != null ? playersRanking[3] + "pt" : " - ";
        transform.GetChild(5).GetComponent<Text>().text = playersRanking[3] != null ? playersRanking[3] + "pt" : " - ";
        transform.GetChild(6).GetComponent<Text>().text = playersRanking[4] != null ? playersRanking[4] + "pt" : " - ";
        transform.GetChild(7).GetComponent<Text>().text = playersRanking[5] != null ? playersRanking[5] + "pt" : " - ";
        transform.GetChild(8).GetComponent<Text>().text = playersRanking[6] != null ? playersRanking[6] + "pt" : " - ";
        transform.GetChild(9).GetComponent<Text>().text = playersRanking[7] != null ? playersRanking[7] + "pt" : " - ";
        transform.GetChild(10).GetComponent<Text>().text = playersRanking[8] != null ? playersRanking[8] + "pt" : " - ";
    }
}
