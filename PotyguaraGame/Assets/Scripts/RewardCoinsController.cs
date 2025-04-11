using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RewardCoinsController : MonoBehaviour
{
    private bool potycoinContabilized = false;
    public void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            DateTime today = DateTime.Now;
            int day = today.Day;

            if (FindFirstObjectByType<PotyPlayerController>().GetDay() != day)
            {
                NetworkManager.Instance.SendNewDay(day);
                Invoke("RewardCoins", 1f);
            }
        }
    }

    private void RewardCoins()
    {
        AudioSource audio = transform.GetChild(0).GetChild(4).GetComponent<AudioSource>();
        ParticleSystem confetti = transform.GetChild(1).GetComponent<ParticleSystem>();
        GetComponent<FadeController>().FadeInForFadeOutWithDeactivationOfGameObject(2f, gameObject);

        confetti.Play();
        audio.Play();

        if (!potycoinContabilized)
        {
            FindFirstObjectByType<PotyPlayerController>().SetPotycoins(50);
            potycoinContabilized = true;
        }
        StartCoroutine("ResetBoolean");
    }

    private IEnumerator ResetBoolean()
    {
        yield return new WaitForSeconds(2f);
        potycoinContabilized = false;
    }
}
