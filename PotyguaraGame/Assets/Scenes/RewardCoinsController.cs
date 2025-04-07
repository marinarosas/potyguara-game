using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class RewardCoinsController : MonoBehaviour
{
    private bool potycoinContabilized = false;
    public void GetRewardCoins()
    {
        if (NetworkManager.Instance.isNewDay)
        {
            GameObject canva = transform.GetChild(2).gameObject;
            AudioSource audio = canva.transform.GetChild(6).GetComponent<AudioSource>();
            Button button = canva.transform.GetChild(5).GetComponent<Button>();

            if (button != null && canva != null)
            {
                canva.SetActive(true);
                canva.GetComponent<FadeController>().FadeIn();
                audio.Play();
                button.onClick.AddListener(UpdatePotycoins);
            }
            NetworkManager.Instance.isNewDay = false;
        }
    }

    private void UpdatePotycoins()
    {
        if (!potycoinContabilized)
        {
            FindFirstObjectByType<PotyPlayerController>().SetPotycoins(50);
            transform.GetChild(2).GetComponent<FadeController>().FadeOutWithDeactivationOfGameObject(transform.GetChild(2).gameObject);
            potycoinContabilized = true;
            StartCoroutine("ResetBoolean");
        }
    }

    private IEnumerator ResetBoolean()
    {
        yield return new WaitForSeconds(2f);
        potycoinContabilized = false;
    }
}
