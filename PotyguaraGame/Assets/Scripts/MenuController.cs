using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Ponta Negra
    // galeria
    // Forte dos Reis
    // Game Forte
    // Game Forte Zombie Mode
    // Hover Bunda
    // Sala de Meditação
    // Sair para o Menu

    private TransitionController transitionController;
    [SerializeField] private List<Sprite> galleryImages;

    void Start()
    {
        int count = 0;
        foreach (var image in galleryImages) 
        { 
            transform.GetChild(count).GetComponent<Image>().sprite = image;
            count++;
        }


        for (int ii = 0; ii < transform.childCount; ii++)
            transform.GetChild(ii).gameObject.SetActive(true);

        transitionController = FindFirstObjectByType<TransitionController>();

        if (SceneManager.GetActiveScene().buildIndex == 2) // ponta Negra
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).GetComponent<Button>().onClick.AddListener(GoToGallery);
            transform.GetChild(2).GetComponent<Button>().onClick.AddListener(GoToMeditationRoom);
            transform.GetChild(3).GetComponent<Button>().onClick.AddListener(LoadForte);
            transform.GetChild(4).GetComponent<Button>().onClick.AddListener(GoToGameForte);
            transform.GetChild(5).GetComponent<Button>().onClick.AddListener(GoToGameForteZombieMode);
            transform.GetChild(6).GetComponent<Button>().onClick.AddListener(LoadHoverBunda);
            transform.GetChild(7).GetComponent<Button>().onClick.AddListener(ExitGame);
        }
        if (SceneManager.GetActiveScene().buildIndex == 3) // Reis Magos
        {
            transform.GetChild(0).GetComponent<Button>().onClick.AddListener(LoadPontaNegra);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(false);
            transform.GetChild(7).gameObject.SetActive(false);
            transform.GetChild(6).GetComponent<Button>().onClick.AddListener(LoadHoverBunda);
            transform.GetChild(7).GetComponent<Button>().onClick.AddListener(ExitGame);
        }
        if (SceneManager.GetActiveScene().buildIndex == 4) // HoverBunda
        {
            transform.GetChild(0).GetComponent<Button>().onClick.AddListener(LoadPontaNegra);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).GetComponent<Button>().onClick.AddListener(LoadForte);
            transform.GetChild(4).GetComponent<Button>().onClick.AddListener(GoToGameForte);
            transform.GetChild(5).GetComponent<Button>().onClick.AddListener(GoToGameForteZombieMode);
            transform.GetChild(6).gameObject.SetActive(false);
            transform.GetChild(7).GetComponent<Button>().onClick.AddListener(ExitGame);
        }
    }

    void GoToMeditationRoom()
    {
        transitionController.TeleportMeditationRoom();
    }

    void LoadPontaNegra()
    {
        transitionController.LoadSceneAsync(2);
    }

    void LoadForte()
    {
        transitionController.LoadSceneAsync(3);
    }

    void LoadHoverBunda()
    {
        int potycoins = FindFirstObjectByType<PotyPlayerController>().GetPotycoins();
        if (potycoins >= 10)
        {
            FindFirstObjectByType<PotyPlayerController>().ConsumePotycoins(10);
            transitionController.LoadSceneAsync(4);
        }
        else
        {
            Transform mainCam = GameObject.FindWithTag("MainCamera").transform;
            mainCam.GetChild(6).GetChild(4).GetComponent<FadeController>().FadeInForFadeOut(2f);
        }
    }

    void GoToGallery()
    {
        transitionController.TeleportGallery();
        FindFirstObjectByType<MeditationRoomController>().StopClass();
        GameObject.FindWithTag("Player").transform.GetChild(1).gameObject.SetActive(true);
    }

    void GoToGameForte()
    {
        int potycoins = FindFirstObjectByType<PotyPlayerController>().GetPotycoins();
        if (potycoins >= 10)
        {
            FindFirstObjectByType<PotyPlayerController>().ConsumePotycoins(10);
            transitionController.TeleporGameForteNormalMode();
        }
        else
        {
            Transform mainCam = GameObject.FindWithTag("MainCamera").transform;
            mainCam.GetChild(6).GetChild(4).GetComponent<FadeController>().FadeInForFadeOut(2f);
        }
    }

    void GoToGameForteZombieMode()
    {
        int potycoins = FindFirstObjectByType<PotyPlayerController>().GetPotycoins();
        if (potycoins >= 10)
        {
            FindFirstObjectByType<PotyPlayerController>().ConsumePotycoins(10);
            transitionController.TeleportGameForteZombieMode();
        }
        else
        {
            Transform mainCam = GameObject.FindWithTag("MainCamera").transform;
            mainCam.GetChild(6).GetChild(4).GetComponent<FadeController>().FadeInForFadeOut(2f);
        }
    }

    void ExitGame()
    {
        transitionController.LoadSceneAsync(0);
    }
}
