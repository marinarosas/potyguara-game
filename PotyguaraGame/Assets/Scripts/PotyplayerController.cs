using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotyPlayerController : MonoBehaviour
{
    public PotyPlayer potyPlayer = null;
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
    private static PotyPlayerController _instance;

    public static PotyPlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PotyPlayerController>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("PotyPlayerController");
                    _instance = obj.AddComponent<PotyPlayerController>();
                }
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        nm = NetworkManager.Instance;
    }
    public void SetScore(int value, int gameMode)
    {
        if(gameMode == 0)
            potyPlayer.SetScoreZombieMode(value);
        else
            potyPlayer.SetScoreNormalMode(value);
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

        btn.onClick.RemoveAllListeners();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
            FindFirstObjectByType<GameForteController>().GameOver();
    }
}
