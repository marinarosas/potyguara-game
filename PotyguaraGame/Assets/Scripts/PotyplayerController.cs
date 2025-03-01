using Steamworks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotyPlayerController : MonoBehaviour
{
    public string nickname { get; set; }

    private int normalModeGameForteScore = 0;
    private int zombieModeGameForteScore = 0;
    private int positionRankingZombieMode;
    private int positionRankingNormalMode;
    private int potycoins = 0;
    private NetworkManager nm;
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
    }

    void Start()
    {
        SetSkin(-1, -1, -1);
    }

    public void SetScore(int value, int gameMode)
    {
        if(gameMode == 0)
            SetScoreZombieMode(value);
        else
            SetScoreNormalMode(value);
    }

    public void HideControllers()
    {
        Transform left = FindFirstObjectByType<LeftHandController>().gameObject.transform;
        left.GetChild(1).gameObject.SetActive(false);
        left.GetChild(2).gameObject.SetActive(false);

        Transform right = FindFirstObjectByType<RightHandController>().gameObject.transform;
        right.GetChild(1).gameObject.SetActive(false);
        right.GetChild(2).gameObject.SetActive(false);
    }

    public void ShowControllers()
    {
        Transform left = FindFirstObjectByType<LeftHandController>().gameObject.transform;
        left.GetChild(1).gameObject.SetActive(true);
        left.GetChild(2).gameObject.SetActive(true);

        Transform right = FindFirstObjectByType<RightHandController>().gameObject.transform;
        right.GetChild(1).gameObject.SetActive(true);
        right.GetChild(2).gameObject.SetActive(true);
    }

    public void SetSkin(int skinGender, int skinIndex, int skinMaterial)
    {
        skin.gender = skinGender;
        skin.index = skinIndex; 
        skin.material = skinMaterial;   
    }

    public int GetIndex() { return skin.index; }
    public int GetGender() { return skin.gender; }
    public int GetMaterial() { return skin.material; }

    public void UpdatePotycoins(int value, Button btn, GameObject canva)
    {
        SetPotycoins(value);
        FindFirstObjectByType<NetworkManager>().UpdatePotycoins(potycoins);
        canva.GetComponent<FadeController>().FadeOutWithDeactivationOfGameObject(canva);
    }

    public void SetPotycoins(int value)
    {
        potycoins += value;
    }

    public void ConsumePotycoins(int value)
    {
        potycoins -= value;
        FindFirstObjectByType<NetworkManager>().UpdatePotycoins(potycoins);
    }

    public int GetPotycoins()
    {
        return potycoins;
    }

    public void SetPositionRanking(int value, int mode)
    {
        if (mode == 0)
            positionRankingZombieMode = value;
        else
            positionRankingNormalMode = value;
    }

    public void SetScoreZombieMode(int value)
    {
        zombieModeGameForteScore = value;
    }

    public void SetScoreNormalMode(int value)
    {
        normalModeGameForteScore = value;
    }
}
