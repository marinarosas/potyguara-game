using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;

public class PotyPlayer : MonoBehaviour
{
    public string nickname { get; set; }

    private int normalModeGameForteScore = 0;
    private int zombieModeGameForteScore = 0;
    public InputDevice inputDeviceLeft;
    public InputDevice inputDeviceRight;
    private GameObject skin;

    public static PotyPlayer Instance = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public PotyPlayer(string nick, InputDevice leftHand, InputDevice rightHand, GameObject skinStandard)
    {
        nickname = nick;
        inputDeviceLeft = leftHand;
        inputDeviceRight = rightHand;
        skin = skinStandard;
    }

    public void SetSkin(GameObject skin)
    {
        this.skin = skin;
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
