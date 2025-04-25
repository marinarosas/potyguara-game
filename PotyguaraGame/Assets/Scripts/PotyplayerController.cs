using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PotyPlayerController : MonoBehaviour
{
    public string nickname { get; set; }

    private string positionRankingZombieMode = "N/A";
    private string positionRankingNormalMode = "N/A";
    private string scoreNormalMode = "";
    private string scoreZombieMode = "";
    private int day = 0;
    private int potycoins = 0;
    private NetworkManager nm;
    private List<int> skinsMASC;
    private List<int> skinsFEM;
    private List<string> tickets;
    private List<string> sessions;

    private bool wasConsumed = false;
    struct Skin
    {
        public int gender;
        public int index;
        public int material;
    }
    private Skin skin;
    public string PlayerId
    {
        get
        {
            return nm.playerId;
        }
    }

    // vari�vel para armazenar a �lltime vez que a posi��o foi enviada
    float lastSentPositionTime = 0;

    // Quantas vezes por segundo enviar a posi��o para o servidor
    public float updateServerTimesPerSecond = 10;

    //  Singleton stuff
    public static PotyPlayerController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        nm = NetworkManager.Instance;

        if (SceneManager.GetActiveScene().buildIndex != 0)
            GameObject.FindWithTag("MainCamera").transform.GetChild(4).GetComponent<SteamProfileManager>().UpdatePotycoins(potycoins);
    }

    void Start()
    {
        SetSkin(-1, -1, -1);
        skinsMASC = new List<int>();
        skinsFEM = new List<int>();
        tickets = new List<string>();
        sessions = new List<string>();
    }

    public int GetDay() { return day;}
    public void SetDay(int value)  { day = value; }

    public void AddSkin(int value)
    {
        if (GetGender() == 0)
            skinsMASC.Add(value);
        else
            skinsFEM.Add(value);
    }
    public void ResetSkins() {
        if (GetGender() == 0)
            skinsMASC.Clear();
        else
            skinsFEM.Clear();
    }
    public bool VerifSkins(int index)
    {
        if (GetGender() == 0)
            return skinsMASC.Contains(index) ? true : false;
        else
            return skinsFEM.Contains(index) ? true : false;
    }

    public void AddTicket(string ticket) {  tickets.Add(ticket); }
    public List<string> GetTickets() {  return tickets; }
    public bool VerifTickets(string id) { return tickets.Contains(id) ? true : false; }

    public void AddSession(string session) { sessions.Add(session); }
    public List<string> GetSessions() { return sessions; }
    public bool VerifSessions(string id) { return sessions.Contains(id) ? true : false; }

    public void HideControllers()
    {
        Transform left = FindFirstObjectByType<LeftHandController>().gameObject.transform;
        left.GetChild(1).gameObject.SetActive(false);
        left.GetChild(2).gameObject.SetActive(false);

        Transform right = FindFirstObjectByType<RightHandController>().gameObject.transform;
        right.GetChild(1).gameObject.SetActive(false);
        right.GetChild(2).gameObject.SetActive(false);

        GameObject.FindWithTag("Player").transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ShowControllers()
    {
        Transform left = FindFirstObjectByType<LeftHandController>().gameObject.transform;
        left.GetChild(1).gameObject.SetActive(true);
        left.GetChild(2).gameObject.SetActive(true);

        Transform right = FindFirstObjectByType<RightHandController>().gameObject.transform;
        right.GetChild(1).gameObject.SetActive(true);
        right.GetChild(2).gameObject.SetActive(true);
        if (FindFirstObjectByType<LiftShowController>().GetStatus() == 1)
            FindFirstObjectByType<MenuShowController>().showLiberated = true;

        GameObject.FindWithTag("Player").transform.GetChild(1).gameObject.SetActive(true);
    }

    public void SetSkin(int skinGender, int skinIndex, int skinMaterial)
    {
        skin.gender = skinGender;
        skin.index = skinIndex; 
        skin.material = skinMaterial;   
    }

    public void SetGender(int gender)
    {
        skin.gender = gender;
    }

    public int GetIndex() { return skin.index; }
    public int GetGender() { return skin.gender; }
    public int GetMaterial() { return skin.material; }

    public void GetPotycoinsOfTheServer(int value)
    {
        potycoins = value;
        GameObject.FindWithTag("MainCamera").transform.GetChild(4).GetComponent<SteamProfileManager>().UpdatePotycoins(potycoins);
    }

    public void SetPotycoins(int value)
    {
        potycoins += value;
        FindFirstObjectByType<NetworkManager>().UpdatePotycoins(potycoins);
        GameObject.FindWithTag("MainCamera").transform.GetChild(4).GetComponent<SteamProfileManager>().UpdatePotycoins(potycoins);
    }

    public void ConsumePotycoins(int value)
    {
        if (!wasConsumed)
        {
            potycoins -= value;
            wasConsumed = true;
            FindFirstObjectByType<NetworkManager>().UpdatePotycoins(potycoins);
            GameObject.FindWithTag("MainCamera").transform.GetChild(4).GetComponent<SteamProfileManager>().UpdatePotycoins(potycoins);
            Invoke("ResetBoolean", 2f);
        }
    }

    private void ResetBoolean()
    {
        wasConsumed = false;
    }

    public int GetPotycoins()
    {
        return potycoins;
    }

    public void SetScoreNormalMode(string value) { scoreNormalMode = value; }
    public void SetScoreZombieMode(string value){  scoreZombieMode = value; }

    public int GetScoreNormalMode()
    {
        return int.Parse(scoreNormalMode);
    }

    public int GetScoreZombieMode()
    {
        return int.Parse(scoreZombieMode);
    }

    public string GetScoreZombie(){
        string formatedScore = nickname + ": " + scoreZombieMode + "pt";
        return formatedScore;
    }
    public string GetScoreNormal() {
        string formatedScore = nickname + ": " + scoreNormalMode + "pt";
        return formatedScore;
    }

    public void SetPositionRanking(string value, int mode)
    {
        if (mode == 0)
            positionRankingZombieMode = value.Equals("0") ? "N/A" : value;
        else
            positionRankingNormalMode = value.Equals("0") ? "N/A" : value;
    }
    public string GetPositionZombie() { return positionRankingZombieMode; }
    public string GetPositionNormal() { return positionRankingNormalMode; }
}
