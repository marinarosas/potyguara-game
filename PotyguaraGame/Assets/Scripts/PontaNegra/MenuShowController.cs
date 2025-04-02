using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WaypointsFree;

public class MenuShowController : MonoBehaviour
{
    [Header("Button Prefab")]
    [SerializeField] public GameObject buttonPrefab;

    [Header("Menu Container")]
    [SerializeField] private Show[] shows;

    [Header("Menu Container")]
    [SerializeField] private Transform content;

    public bool showLiberated = false;

    private void Start()
    {
        foreach (Show show in shows)
        {
            CreateButton(show.image, show.description);
        }
        CheckTickets();
    }
    private void CreateButton(Sprite image, string description)
    {
        GameObject newButton = Instantiate(buttonPrefab, content);
        newButton.GetComponent<Image>().sprite = image;
        newButton.GetComponent<Button>().interactable = false;
        Destroy(newButton.transform.GetChild(1).gameObject);
    }

    public void UnclockShow(string id)
    {
        for (int ii = 0; ii < shows.Length; ii++)
        {
            if (shows[ii].id == id)
            {
                content.GetChild(ii).GetComponent<Button>().interactable = true;
                content.GetChild(ii).GetComponent<Button>().onClick.AddListener(() => UnclockDeck(shows[ii]));
                return;
            }
        }
    }

    public void CheckTickets()
    {
        List<string> tickets = FindFirstObjectByType<PotyPlayerController>().GetTickets();
        if (tickets.Count != 0)
        {
            foreach(string ticket in tickets)
                FindFirstObjectByType<MenuShowController>().UnclockShow(ticket);
        }
    }

    private void UnclockDeck(Show show)
    {
        FindObjectOfType<LiftShowController>().UnleashLift();
        FindObjectOfType<LiftShowController>().OpenCatraca2();

        if (!showLiberated)
        {
            GameObject banda = Instantiate(show.banda);
            banda.transform.position = new Vector3(180.46f, 6.89f, 251.19f);
            banda.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            GameObject showGo = new GameObject("show");
            banda.transform.parent = showGo.transform;

            foreach (GameObject extra in show.extras) {
                GameObject obj = Instantiate(extra);
                obj.GetComponent<WaypointsTraveler>().Move(true);
                obj.transform.parent = showGo.transform;
            }

            banda.GetComponent<BandController>().StartShow(show.clip);
            FindFirstObjectByType<LiftShowController>().hasTicket = true;

            transform.GetChild(0).GetComponent<FadeController>().FadeOutWithDeactivationOfGameObject(transform.GetChild(0).gameObject);
            showLiberated = true;
        }
    }
}
