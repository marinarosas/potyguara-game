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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
            FindFirstObjectByType<GameForteController>().GameOver();
    }
}
