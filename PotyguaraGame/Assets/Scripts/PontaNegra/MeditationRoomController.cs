using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MeditationRoomController : MonoBehaviour
{
    private int countClasses = 1; // qnt de aulas
    private bool StartedClass = false;

    [SerializeField] private GameObject magicCircles;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audios;
    [SerializeField] private Font font;
    // Start is called before the first frame update

    void Update()
    {
        if (audioSource != null)
        {
            if (!audioSource.isPlaying && StartedClass)
            {
                transform.parent.parent.parent.parent.parent.GetChild(0).gameObject.SetActive(true);
                StartedClass = false;
            }
        }
    }

    public void StopClass()
    {
        audioSource.Stop();
        GameObject.Find("MeditationRoom").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("MeditationRoom").transform.parent.GetChild(1).gameObject.SetActive(false);
    }

    #region ButtonCreation
    public void AddButton(int index)
    {
        AudioClip clip = audios[index];
        GameObject buttonGo = new GameObject("Meditação " + countClasses);
        buttonGo.transform.SetParent(transform);

        RectTransform rectTransform = buttonGo.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(230, 216);

        Vector3 localPos = rectTransform.localPosition;
        localPos.z = 0f;
        rectTransform.localPosition = localPos;

        rectTransform.localScale = new Vector3(1, 1, 1);

        Image image = buttonGo.AddComponent<Image>();
        Button button = buttonGo.AddComponent<Button>();

        GameObject textGo = new GameObject("Text");
        textGo.transform.SetParent(buttonGo.transform);

        RectTransform rectTransform2 = textGo.AddComponent<RectTransform>();
        rectTransform2.sizeDelta = new Vector2(200, 100);

        Vector3 localPos2 = rectTransform2.localPosition;
        localPos2.z = 0f;
        localPos2.x = 0f;
        rectTransform2.localPosition = localPos2;

        rectTransform2.localScale = new Vector3(1, 1, 1);

        Text text = textGo.AddComponent<Text>();
        text.font = font;
        text.fontSize = 36;
        text.text = "Aula " + countClasses;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = new Color(0.9245283f, 0.3079158f, 0, 1);
        countClasses++;

        button.onClick.AddListener(() => PlayClass(clip));
    }

    private void PlayClass(AudioClip clip)
    {
        StartedClass = true;
        magicCircles.SetActive(true);
        transform.parent.parent.parent.parent.gameObject.SetActive(false);
        audioSource.clip = clip;
        audioSource.Play();
        GameObject.FindWithTag("Player").transform.GetChild(1).gameObject.SetActive(false);
    }
    #endregion
}
