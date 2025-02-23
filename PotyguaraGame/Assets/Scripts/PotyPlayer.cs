using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;

public class PotyPlayer : MonoBehaviour
{
    public string nickname { get; set; }

    private int normalModeGameForteScore = 0;
    private int zombieModeGameForteScore = 0;
    private int positionRankingZombieMode;
    private int positionRankingNormalMode;
    private struct Skin
    {
        public int gender;
        public int index;
        public int variant;
    }
    private Skin skin;
    private int potycoins=0;

    public void SetSkin(int gender, int index, int variant)
    {
        skin.gender = gender;
        skin.index = index;
        skin.variant = variant;
    }

    public void SetPotycoins(int value)
    {
        potycoins += value;
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
