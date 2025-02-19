using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;

public class PotyPlayer : MonoBehaviour
{
    public string nickname { get; set; }
    public int currentDay { get; set; }

    private int normalModeGameForteScore = 0;
    private int zombieModeGameForteScore = 0;
    private Skin skin;
    private int potycoins=0;

    public PotyPlayer(string nick, Skin skinStandard, int potycoins, int currentDay)
    {
        nickname = nick;
        skin = skinStandard;
        this.potycoins = potycoins;
        this.currentDay = currentDay;
    }

    public void SetSkin(Skin skin)
    {
        this.skin = skin;
    }

    public void SetPotycoins(int value)
    {
        potycoins += value;
    }

    public int GetPotycoins()
    {
        return potycoins;
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
