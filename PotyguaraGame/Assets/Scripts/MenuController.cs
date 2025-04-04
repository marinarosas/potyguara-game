using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private TransitionController transitionController;

    [SerializeField] private List<Sprite> galleryImages;
    [SerializeField] private Transform content;

    public Toggle toggleTutorial;
    public Toggle toggleWeather;

    void Start()
    {
        toggleTutorial.isOn = NetworkManager.Instance.modeTutorialOn;
        toggleWeather.isOn = NetworkManager.Instance.modeWeatherOn;
        int count = 0;
        foreach (var image in galleryImages) 
        { 
            content.GetChild(count).GetComponent<Image>().sprite = image;
            count++;
        }

        for (int ii = 0; ii < transform.childCount; ii++)
            content.GetChild(ii).gameObject.SetActive(true);

        transitionController = FindFirstObjectByType<TransitionController>();

        if (SceneManager.GetActiveScene().buildIndex == 2) // ponta Negra
        {
            content.GetChild(0).gameObject.SetActive(false);
            content.GetChild(1).GetComponent<Button>().onClick.AddListener(GoToGallery);
            content.GetChild(2).GetComponent<Button>().onClick.AddListener(GoToMeditationRoom);
            content.GetChild(3).GetComponent<Button>().onClick.AddListener(LoadForte);
            int potycoins = FindFirstObjectByType<PotyPlayerController>().GetPotycoins();
            if (potycoins >= 10)
            {
                content.GetChild(4).GetComponent<Button>().onClick.AddListener(GoToGameForte);
                content.GetChild(5).GetComponent<Button>().onClick.AddListener(GoToGameForteZombieMode);
                content.GetChild(6).GetComponent<Button>().onClick.AddListener(LoadHoverBunda);
            }
            else
            {
                content.GetChild(4).GetComponent<Button>().interactable = false;
                content.GetChild(5).GetComponent<Button>().interactable = false;
                content.GetChild(6).GetComponent<Button>().interactable = false;
            }
            content.GetChild(7).GetComponent<Button>().onClick.AddListener(LoadAvatarScene);
            content.GetChild(8).GetComponent<Button>().onClick.AddListener(ExitGame);
        }
        if (SceneManager.GetActiveScene().buildIndex == 3) // Reis Magos
        {
            content.GetChild(0).GetComponent<Button>().onClick.AddListener(LoadPontaNegra);
            content.GetChild(1).gameObject.SetActive(false);
            content.GetChild(2).gameObject.SetActive(false);
            content.GetChild(3).gameObject.SetActive(false);
            content.GetChild(4).gameObject.SetActive(false);
            content.GetChild(5).gameObject.SetActive(false);
            int potycoins = FindFirstObjectByType<PotyPlayerController>().GetPotycoins();
            if (potycoins >= 10)
                content.GetChild(6).GetComponent<Button>().onClick.AddListener(LoadHoverBunda);
            else
                content.GetChild(6).GetComponent<Button>().interactable = false;
            content.GetChild(7).GetComponent<Button>().onClick.AddListener(LoadAvatarScene);
            content.GetChild(8).GetComponent<Button>().onClick.AddListener(ExitGame);
        }
        if (SceneManager.GetActiveScene().buildIndex == 4) // HoverBunda
        {
            content.GetChild(0).GetComponent<Button>().onClick.AddListener(LoadPontaNegra);
            content.GetChild(1).gameObject.SetActive(false);
            content.GetChild(2).gameObject.SetActive(false);
            content.GetChild(3).GetComponent<Button>().onClick.AddListener(LoadForte);
            int potycoins = FindFirstObjectByType<PotyPlayerController>().GetPotycoins();
            if (potycoins >= 10)
            {
                content.GetChild(4).GetComponent<Button>().onClick.AddListener(GoToGameForte);
                content.GetChild(5).GetComponent<Button>().onClick.AddListener(GoToGameForteZombieMode);
            }
            else
            {
                content.GetChild(4).GetComponent<Button>().interactable = false;
                content.GetChild(5).GetComponent<Button>().interactable = false;
            }
            content.GetChild(6).gameObject.SetActive(false);
            content.GetChild(7).GetComponent<Button>().onClick.AddListener(LoadAvatarScene);
            content.GetChild(8).GetComponent<Button>().onClick.AddListener(ExitGame);
        }
    }

    public void SendModeWeather(bool value)
    {
        NetworkManager.Instance.SendModeWeather(value);
    }

    public void SendModeTutorial(bool value)
    {
        NetworkManager.Instance.SendModeTutorial(value);
    }

    void LoadAvatarScene()
    {
        transitionController.LoadSceneAsync(1);
    }
    void GoToMeditationRoom()
    {
        if (FindFirstObjectByType<TransitionController>().isInShowArea && SceneManager.GetActiveScene().buildIndex == 2)
        {
            FindFirstObjectByType<LiftShowController>().hasTicket = false;
            FindFirstObjectByType<MenuShowController>().showLiberated = false;
            FindFirstObjectByType<MenuShowController>().gameObject.transform.GetChild(0).gameObject.SetActive(true);
            FindFirstObjectByType<TransitionController>().isInShowArea = false;
            Destroy(GameObject.Find("Dragon"));
            Destroy(GameObject.Find("Guitaura"));
            FindFirstObjectByType<LiftShowController>().GoOutFromTheShow();
            FindFirstObjectByType<LiftShowController>().BlockLift();
        }
        FindFirstObjectByType<MeditationRoomController>().StopClass();
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
            transitionController.LoadSceneAsync(4);
        }
    }

    void GoToGallery()
    {
        if (FindFirstObjectByType<TransitionController>().isInShowArea && SceneManager.GetActiveScene().buildIndex == 2)
        {
            FindFirstObjectByType<LiftShowController>().hasTicket = false;
            FindFirstObjectByType<MenuShowController>().showLiberated = false;
            FindFirstObjectByType<MenuShowController>().gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Destroy(GameObject.Find("Dragon"));
            Destroy(GameObject.Find("Guitaura"));
            FindFirstObjectByType<TransitionController>().isInShowArea = false;
            FindFirstObjectByType<LiftShowController>().GoOutFromTheShow();
            FindFirstObjectByType<LiftShowController>().BlockLift();
        }

        transitionController.TeleportGallery();
        FindFirstObjectByType<MeditationRoomController>().StopClass();
        GameObject.FindWithTag("Player").transform.GetChild(1).gameObject.SetActive(true);
    }

    void GoToGameForte()
    {
        int potycoins = FindFirstObjectByType<PotyPlayerController>().GetPotycoins();
        if (potycoins >= 10)
        {
            transitionController.TeleporGameForteNormalMode();
        }
    }

    void GoToGameForteZombieMode()
    {
        int potycoins = FindFirstObjectByType<PotyPlayerController>().GetPotycoins();
        if (potycoins >= 10)
        {
            transitionController.TeleportGameForteZombieMode();
        }
    }

    void ExitGame()
    {
        transitionController.LoadSceneAsync(0);
    }
}
