using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    public void ShowRanking()
    {
        string ranking = FindObjectOfType<NetworkManager>().GetRanking();
        string[] playersRanking = ranking.Split('|');
        transform.GetChild(2).GetComponent<Text>().text = playersRanking[0] + "pt";
        for(int ii=1; ii < 8; ii++)
        {
            if(ii < playersRanking.Length && playersRanking[ii].Length > 1)
            {
                transform.GetChild(ii+2).GetComponent<Text>().text = playersRanking[ii] + "pt";
            }
            else
            {
                transform.GetChild(ii+2).GetComponent<Text>().text = "-";
            }
        }
    }
}
