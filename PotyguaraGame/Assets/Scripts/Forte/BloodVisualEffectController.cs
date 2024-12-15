using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BloodVisualEffectController : MonoBehaviour
{
    [SerializeField] private VisualEffect blood;

    public void stopEffect()
    {
        gameObject.SetActive(false);
    }

    public void playEffect()
    {
        gameObject.SetActive(true);
        blood.SendEvent("Bleeding");
    }

}
