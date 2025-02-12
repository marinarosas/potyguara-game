using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Instrument
{
    VOCALS,
    GUITAR,
    BASS,
    DRUMS,
    KEYBOARD
}

public class BandMember : MonoBehaviour
{
    [Header("Member")]
    [SerializeField] protected string name;
    [SerializeField] protected bool isBackingVocal;
    [SerializeField] private Instrument instrument;
    protected Animator animator;

    public void IniciateMember()
    {
        if (isBackingVocal && instrument != Instrument.VOCALS)
            setBackingVocalMic();
        
        Debug.Log("Iniciate "+ name + " - "+ instrument);
    }

    protected void setBackingVocalMic()
    {
        GameObject micPrefab = Resources.Load<GameObject>("MicrophonePrefab");

        if (micPrefab != null)
        {
            Vector3 micPosition = transform.position + transform.forward * 0.4f;
            GameObject micInstance = Instantiate(micPrefab, micPosition, transform.rotation);
        }
        else
            Debug.LogError("Microphone backing vocal prefab not found in Resources.");
    }

    public void setAnimator(Animator anim)
    {
        animator = anim;
    }

    void Update()
    {
        
    }

    public Instrument getInstrument() => instrument;

    public void pauseAnimation() => animator.SetBool("isIdle", true);
    public void playAnimation() => animator.SetBool("isIdle", false);
}
