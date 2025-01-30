using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PotyPlayerController : MonoBehaviour
{
    private PotyPlayer potyPlayer;
    private NetworkManager nm;

    public static PotyPlayerController Instance = null;
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

    // Start is called before the first frame update
    void Awake()
    {
        nm = NetworkManager.Instance;
        /*if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Instance.gameObject.transform.eulerAngles = transform.eulerAngles;
            Destroy(gameObject);
        }*/
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        potyPlayer = new PotyPlayer("Bianca", new GameObject());
    }

    public void SetScore(int value, int gameMode)
    {
        if(gameMode == 0)
            potyPlayer.SetScoreZombieMode(value);
        else
            potyPlayer.SetScoreNormalMode(value);
    }

    private void Update()
    {
        /*if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
            //GetComponent<XROrigin>().Camera.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
            XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            FindFirstObjectByType<GameForteController>().GameOver();
        }

    }
}
