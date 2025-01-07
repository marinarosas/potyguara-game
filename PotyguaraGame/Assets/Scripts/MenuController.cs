using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Ponta Negra
    // galeria
    // enter show
    // Forte dos Reis
    // Hover Bunda
    // Sair do Jogo

    private bool updatedMenu = false;
    private int currentScene;
    private TransitionController transitionController;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        transitionController = FindFirstObjectByType<TransitionController>();
    }
    // Update is called once per frame
    void Update()
    {
        if(currentScene != SceneManager.GetActiveScene().buildIndex)
        {
            updatedMenu = false;
        }
        if (!updatedMenu)
        {
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                transform.GetChild(0).GetComponent<Button>().interactable = false;
                transform.GetChild(1).GetComponent<Button>().onClick.AddListener(GoToGallery);
                transform.GetChild(2).GetComponent<Button>().onClick.AddListener(GoToShow);
                transform.GetChild(3).GetComponent<Button>().onClick.AddListener(LoadForte);
                transform.GetChild(4).GetComponent<Button>().onClick.AddListener(ExitGame);
                updatedMenu = true;
            }
            if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                transform.GetChild(0).GetComponent<Button>().onClick.AddListener(LoadPontaNegra);
                transform.GetChild(1).GetComponent<Button>().interactable = false;
                transform.GetChild(2).GetComponent<Button>().interactable = false;
                transform.GetChild(3).GetComponent<Button>().interactable = false;
                transform.GetChild(4).GetComponent<Button>().onClick.AddListener(ExitGame);
                updatedMenu = true;
            }
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                transform.GetChild(0).GetComponent<Button>().onClick.AddListener(LoadPontaNegra);
                transform.GetChild(1).GetComponent<Button>().interactable = false;
                transform.GetChild(2).GetComponent<Button>().interactable = false;
                transform.GetChild(3).GetComponent<Button>().onClick.AddListener(LoadForte);
                transform.GetChild(4).GetComponent<Button>().onClick.AddListener(ExitGame);
                updatedMenu = true;
            }
        }
    }

    void LoadPontaNegra()
    {
        transitionController.LoadSceneAsync(2);
    }

    void LoadForte()
    {
        transitionController.LoadSceneAsync(3);
    }

    void GoToGallery()
    {
        transitionController.TeleportGallery();
    }

    void GoToShow()
    {
        transitionController.TeleportEnterShow();
    }

    void ExitGame()
    {
        transitionController.ExitGame();
    }
}
