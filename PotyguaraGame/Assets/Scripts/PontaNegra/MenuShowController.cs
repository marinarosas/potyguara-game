using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuShowController : MonoBehaviour
{
    [Header("Button Prefab")]
    [SerializeField] public GameObject buttonPrefab;

    [Header("Menu Container")]
    [SerializeField] private Transform content;

    private void Start()
    {
        Product[] shows = FindFirstObjectByType<SalesCenterController>().GetShows();
        if (shows != null)
            foreach (Product show in shows)
                AddNewButton(show.image);

        if (!FindFirstObjectByType<NetworkManager>().isTheFirstAcess)
        {
            foreach (Product show in shows)
            {
                FindFirstObjectByType<NetworkManager>().RequestTickets(show.id);
            }
            FindFirstObjectByType<NetworkManager>().CheckTickets(content);

        }
    }
    private void AddNewButton(Sprite image)
    {
        GameObject newButton = Instantiate(buttonPrefab, content);
        newButton.GetComponent<Button>().interactable = false;
        newButton.GetComponent<Image>().sprite = image;
        Destroy(newButton.transform.GetChild(1).gameObject);
    }

    public void UnclockShow(string id)
    {
        Product[] shows = FindFirstObjectByType<SalesCenterController>().GetShows();
        for (int ii = 0; ii < shows.Length; ii++)
        {
            if (shows[ii].id == id)
            {
                content.GetChild(ii).GetComponent<Button>().interactable = true;
                content.GetChild(ii).GetComponent<Button>().onClick.AddListener(UnclockDeck);
            }
        }
    }

    private void UnclockDeck()
    {
        FindObjectOfType<LiftShowController>().UnleashLift();
        FindObjectOfType<LiftShowController>().OpenCatraca2();
    }
}
