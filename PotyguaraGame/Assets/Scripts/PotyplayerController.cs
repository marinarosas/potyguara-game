using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotyPlayerController : MonoBehaviour
{
    public PotyPlayer potyPlayer = new PotyPlayer();
    private NetworkManager nm;

    public string PlayerId
    {
        get
        {
            return nm.playerId;
        }
    }

    // variável para armazenar a úlltime vez que a posição foi enviada
    float lastSentPositionTime = 0;

    // Quantas vezes por segundo enviar a posição para o servidor
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

    public void SetScore(int value, int gameMode)
    {
        if(gameMode == 0)
            potyPlayer.SetScoreZombieMode(value);
        else
            potyPlayer.SetScoreNormalMode(value);
    }

    private void Start()
    {
        if (!SteamManager.Initialized)
            return;

        potyPlayer.nickname = SteamFriends.GetPersonaName();
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

    public void DeletePerfil()
    {
        FindFirstObjectByType<NetworkManager>().DeletePerfil(PlayerId);
    }

    public void UpdatePotycoins(int value, Button btn, GameObject canva)
    {
        potyPlayer.SetPotycoins(value);
        FindFirstObjectByType<NetworkManager>().UpdatePotycoins(potyPlayer.GetPotycoins());
        canva.GetComponent<FadeController>().FadeOutWithDeactivationOfGameObject(canva);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
            FindFirstObjectByType<GameForteController>().GameOver();
    }
}
