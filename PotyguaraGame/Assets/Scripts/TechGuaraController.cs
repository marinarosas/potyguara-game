using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TechGuaraController : MonoBehaviour
{
    private Report report;
    private bool modeTutorial = true;
    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audios;

    public void SetMode(Boolean value)
    {
        modeTutorial = value;
    }

    public Boolean GetMode()
    {
        return modeTutorial;
    }
    void Start()
    {
        audioSource = transform.GetChild(2).GetComponent<AudioSource>();
        report = transform.GetChild(0).GetComponent<Report>();

        if (modeTutorial)
        {
            transform.gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                foreach (AudioClip clip in audios)
                {
                    if (clip.name.Equals("Techyguara.InicioDoJogo.CriaçãoDeCadastro+Avatar"))
                        audioSource.clip = clip;
                }
                transform.GetChild(0).GetComponent<FadeController>().FadeInForFadeOutWithDeactivationOfGameObject(audioSource.clip.length, transform.GetChild(0).gameObject);
                transform.position = new Vector3(0f, 2f, -32.35f);
                report.UpdateTitle("Bem-vindo(a) ao Potyguara Verse!");
                report.UpdateDescription("Você acaba de entrar em um mundo onde a cultura e a tecnologia se encontram em uma experiência imersiva única. Eu sou a Techyguara, sua guia," +
                    " e juntos vamos explorar esse universo cheio de novidades! No Potyguara Verse, você poderá participar de eventos incríveis, jogar minigames, visitar nossa loja exclusiva e muito mais.");
            }
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                foreach (AudioClip clip in audios)
                {
                    if (clip.name.Equals("Techyguara.CriaçãodePerfil+Avatar"))
                        audioSource.clip = clip;
                }
                transform.GetChild(0).GetComponent<FadeController>().FadeInForFadeOutWithDeactivationOfGameObject(audioSource.clip.length, transform.GetChild(0).gameObject);
                report.UpdateTitle("Crie seu Avatar!");
                report.UpdateDescription("Agora que você já se apresentou, é hora de criar seu avatar! Escolha suas características, como rosto, cabelo, roupas e acessórios para refletir sua personalidade no " +
                    "Potyguara Verse. Depois, você estará pronto para explorar este mundo como nunca antes!");
            }
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                foreach (AudioClip clip in audios)
                {
                    if (clip.name.Equals("Techyguara.ApresentaçãoPraiadePontaNegra"))
                        audioSource.clip = clip;
                }
                transform.GetChild(0).GetComponent<FadeController>().FadeInForFadeOutWithDeactivationOfGameObject(audioSource.clip.length, transform.GetChild(0).gameObject);
                transform.position = new Vector3(177f, 3f, 76f);
                report.UpdateTitle("Praia de Ponta Negra");
                report.UpdateDescription("Você está na famosa Praia de Ponta Negra, uma das mais conhecidas da Cidade do Natal, principalmente por conta do imponente Morro do Careca, com seus 110 metros de altura. " +
                    "Aqui, você encontrará diversos eventos, como shows, exposições e o emocionante minigame Hoverbunda. Sinta-se livre para caminhar pela praia, explorar as atrações e aproveitar os eventos incríveis!");
            }
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                if (!FindFirstObjectByType<TransitionController>().GetIsSkip())
                {
                    foreach (AudioClip clip in audios)
                    {
                        if (clip.name.Equals("Techyguara.ApresentaçãoFortalezaDosReisMagos"))
                            audioSource.clip = clip;
                    }
                    transform.GetChild(0).GetComponent<FadeController>().FadeInForFadeOutWithDeactivationOfGameObject(audioSource.clip.length, transform.GetChild(0).gameObject);
                    transform.position = new Vector3(804.55f, 10.34f, 400.19f);
                    report.UpdateTitle("Forte dos Reis Magos");
                    report.UpdateDescription("Agora, vamos à Fortaleza dos Reis Magos, um dos locais mais históricos da cidade de Natal. Este lugar foi palco de batalhas importantes que mudaram o rumo da nossa região." +
                        "Aqui, você poderá jogar minigames inspirados em épocas passadas. Sabia que, durante as invasões holandesas, a cidade de Natal foi chamada de Nova Amsterdã? Explore os muros de pedra e descubra " +
                        "mais sobre essa fascinante história enquanto se diverte!");
                }
            }
            if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                foreach (AudioClip clip in audios)
                {
                    if (clip.name.Equals("Techyguara.ApresentaçãoHoverbunda"))
                        audioSource.clip = clip;
                }
                transform.GetChild(0).GetComponent<FadeController>().FadeIn();
                transform.position = new Vector3(584.61f, 53.4f, -559.51f);
                report.UpdateTitle("HoverBunda");
                report.UpdateDescription("Prepare-se para a adrenalina no Hoverbunda, uma corrida emocionante onde você se lança no seu skibunda voador! Compita contra seus amigos e mostre que você é o melhor, pois " +
                    "apenas o mais rápido cruzará a linha de chegada!");
            }
            audioSource.Play();
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(audioSource.isPlaying && SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (audioSource.time >= 27.0)
            {
                report.UpdateTitle("Complete seu Perfil!");
                report.UpdateDescription("Antes de começarmos, vamos conhecer um pouco mais sobre você! Crie o seu avatar para começar a sua jornada!");
            }
        }
    }

    #region ReportConfig
    public void CreateReport(string title, string description, float duration)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
        transform.GetChild(0).GetComponent<FadeController>().FadeInForFadeOutWithDeactivationOfGameObject(duration, transform.GetChild(0).gameObject);

        report.UpdateTitle(title);
        report.UpdateDescription(description); 
    }

    public void CreateReport(string title, string description, float duration, Vector3 pos, float direction)
    {
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, direction, 0f);

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
        transform.GetChild(0).GetComponent<FadeController>().FadeInForFadeOutWithDeactivationOfGameObject(duration, transform.GetChild(0).gameObject);

        report.UpdateTitle(title);
        report.UpdateDescription(description);
    }

    public AudioSource SelectReport(string name)
    {
        foreach (AudioClip clip in audios)
        {
            if (clip.name.Equals(name))
            {
                audioSource.clip = clip;
                return audioSource;
            }
        }
        return null;
    }
    #endregion
}
