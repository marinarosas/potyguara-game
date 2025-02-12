using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    public int rankingUpdated = -1;
    void Update()
    {
        if (rankingUpdated == 0)
            UpdateRanking(0);
        if(rankingUpdated == 1)
            UpdateRanking(1);
    }

    public void UpdateRanking(int mode)
    {
        string ranking = mode == 0 ? FindObjectOfType<NetworkManager>().GetRankingZombieMode() : FindObjectOfType<NetworkManager>().GetRankingBatalhaMode();
        string[] playersRanking = ranking.Split('|');

        Transform parent = transform.GetChild(mode == 0 ? 2 : 3);
        parent.GetChild(0).GetComponent<Text>().text = playersRanking[0] + "pt";

        for (int ii = 1; ii < 8; ii++)
        {
            if (playersRanking[ii].Length > 1)
                parent.GetChild(ii).GetComponent<Text>().text = playersRanking[ii] + "pt";
            else
                parent.GetChild(ii).GetComponent<Text>().text = "-";
        }
        rankingUpdated = -1;
    }
}
