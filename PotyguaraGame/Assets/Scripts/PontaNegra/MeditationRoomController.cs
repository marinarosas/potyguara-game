using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MeditationRoomController : MonoBehaviour
{
    private string folderPath = Application.streamingAssetsPath + "/MeditationClasses"; // qnt de aulas
    private int countClasses = 1; // qnt de aulas
    private List<string> audioFiles = new List<string>(); // adiciona os files 
    private HashSet<string> knownFiles = new HashSet<string>(); //adiciona os audios já adicionados para que não haja duplicatas
    private bool StartedClass = false;

    [SerializeField] private GameObject magicCircles;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Font font;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckForNewFiles());
    }

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
    }

    #region checkingClasses
    public IEnumerator CheckForNewFiles()
    {
        while (true)
        {
            if (Directory.Exists(folderPath))
            {
                try
                {
                    string[] files = Directory.GetFiles(folderPath, "*.*");
                    foreach (string file in files)
                    {
                        if ((file.EndsWith(".wav") || file.EndsWith(".mp3") || file.EndsWith(".ogg")) && !knownFiles.Contains(file))
                        {
                            audioFiles.Add(file);
                            knownFiles.Add(file);
                            AddButton(file);
                            Debug.Log("Aula adicionada com sucesso!!!");
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("error " + e.GetType() + ": " + e);
                }
            }
            yield return new WaitForSeconds(15);
        }
    }
    #endregion

    #region ButtonCreation
    public void AddButton(string filePath)
    {
        GameObject buttonGo = new GameObject("Aula" + countClasses);
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

        button.onClick.AddListener(() => StartCoroutine(PlayClass(filePath)));
    }

    IEnumerator PlayClass(string filePath)
    {
        StartedClass = true;
        magicCircles.SetActive(true);
        transform.parent.parent.parent.parent.gameObject.SetActive(false);
        using (WWW www = new WWW("file://" + filePath))
        {
            yield return www;
            audioSource.clip = www.GetAudioClip();
            audioSource.Play();
        }
    }
    #endregion
}
