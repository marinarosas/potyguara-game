using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    public static Achievement Instance;

    public int zombies = 0;
    public int eventos = 0;
    public int ships_levas = 0;
    public int partidas_hover = 0;
    public int partidas_defesaForte = 0;

    public bool isFirstShip = true;
    public bool isFirstDeadInZombieMode = true;
    public bool isFirstPurchase = true;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetAchievement()
    {
        if (!SteamManager.Initialized)
            return;

        SteamUserStats.ResetAllStats(true);
        SteamUserStats.RequestCurrentStats();

        zombies = 0;
        eventos = 0;
        ships_levas = 0;
        partidas_hover = 0;
        partidas_defesaForte = 0;

        isFirstShip = true;
        isFirstDeadInZombieMode = true;
        isFirstPurchase = true;
    }

    public void UnclockAchievement(string id)
    {
        if(!SteamManager.Initialized)
            return;

        SteamUserStats.SetAchievement(id);
        SteamUserStats.StoreStats();
    }

    public void SetStat(string id, int value)
    {
        if(!SteamManager.Initialized) return;

        SteamUserStats.SetStat(id, value);
        SteamUserStats.StoreStats();
    }
}
