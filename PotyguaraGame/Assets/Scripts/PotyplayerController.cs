using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class PotyPlayerController : MonoBehaviour
{
    public Transform reportTxt;

    private PotyPlayer potyPlayer;
    //private Report report;
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
        //InputDevice inputDeviceLeft = FindFirstObjectByType<LeftHandController>().GetTargetDevice();
        //InputDevice inputDeviceRight = FindFirstObjectByType<RightHandController>().GetTargetDevice();
        potyPlayer = new PotyPlayer("Bianca", /*inputDeviceLeft, inputDeviceRight,*/ new GameObject());
        CreateReport("Bem-vindo(a) " + potyPlayer.nickname, "Esse é o PotyguaraVerse, um ambiente imersivo no qual você poderá curtir shows, interagir com outros jogadores e jogar jogos criados com base em grandes pontos turisticos de Natal");
    }

    public float GetTriggerLeftButton()
    {
        potyPlayer.GetLeftController().TryGetFeatureValue(CommonUsages.trigger, out float triggerValueL);
        return triggerValueL;
    }

    public float GetTriggerRightButton()
    {
        potyPlayer.GetRightController().TryGetFeatureValue(CommonUsages.trigger, out float triggerValueR);
        return triggerValueR;
    }

    public void CreateReport(string title, string message)
    {
        reportTxt.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
        reportTxt.GetChild(2).GetComponent<TextMeshProUGUI>().text = message;
        reportTxt.parent.gameObject.SetActive(true);
        reportTxt.parent.GetComponent<FadeController>().FadeInForFadeOutWithDeactivationOfGameObject(6f, reportTxt.parent.gameObject);
    }

    public void SetScore(int value, int gameMode)
    {
        if(gameMode == 0)
            potyPlayer.SetScoreZombieMode(value);
        else
            potyPlayer.SetScoreNormalMode(value);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
        {
           
        }
    }
}
