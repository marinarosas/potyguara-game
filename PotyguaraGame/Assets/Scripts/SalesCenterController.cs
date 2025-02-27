using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.XR;

public class SalesCenterController : MonoBehaviour
{
    [Header("Button Prefab")]
    [SerializeField] public GameObject buttonPrefab;

    [Header("Scriptable Objects")]
    [SerializeField] public Product[] potycoins;
    [SerializeField] public Product[] shows;
    [SerializeField] public Product[] meditationClasses;
    [SerializeField] public Product[] skins;

    [Header("Menu Container")]
    [SerializeField] private Transform content;
    public bool controlMenu = false;
    public bool changedStatus = false;

    private List<InputDevice> devices = new List<InputDevice>();
    private bool isWaiting = false;

    public void AddNewButton(Sprite image, string id, string description, string category)
    {
        GameObject newButton = Instantiate(buttonPrefab, content);
        newButton.GetComponent<Image>().sprite = image;
        newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
        newButton.GetComponent<Button>().onClick.AddListener(() => BuyProduct(id, description, category));
    }

    public Product[] GetShows()
    {
        return shows;
    }

    public void UpdateSalesCenter(string category)
    {
        foreach (Transform child in content) {
            Destroy(child.gameObject);
        }
        if (category == "moeda")
        {
            foreach (Product item in potycoins)
                AddNewButton(item.image, item.id, item.description, item.category);
        }
        if(category == "show")
        {
            foreach (Product item in shows)
                AddNewButton(item.image, item.id, item.description, item.category);
        }
        if (category == "class")
        {
            foreach (Product item in meditationClasses)
                AddNewButton(item.image, item.id, item.description, item.category);
        }
        if (category == "skin")
        {
            foreach (Product item in skins)
                AddNewButton(item.image, item.id, item.description, item.category);
        }
    }

    public void BuyProduct(string id, string description, string category)
    {
        Microtransaction.Instance.InitSale(id, description, category);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("MainCamera"))
        {
            InputDeviceCharacteristics rightHandCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(rightHandCharacteristics, devices);
            if (devices.Count != 0)
            {
                devices[0].TryGetFeatureValue(CommonUsages.secondaryButton, out bool Bbutton);
                if (Bbutton) // B button pressed
                {
                    GameObject menu = transform.GetChild(1).gameObject;
                    if (menu != null)
                    {
                        if (!changedStatus)
                        {
                            controlMenu = !controlMenu;
                            menu.SetActive(controlMenu);
                            changedStatus = true;
                            Invoke("ChangeStatus", .3f);
                        }
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space)) // B button pressed
                {
                    GameObject menu = transform.GetChild(1).gameObject;
                    if (menu != null)
                    {
                        if (!changedStatus)
                        {
                            controlMenu = !controlMenu;
                            menu.SetActive(controlMenu);
                            changedStatus = true;
                            Invoke("ChangeStatus", .3f);
                        }
                    }
                }
            }
        }
    }

    private void ChangeStatus()
    {
        changedStatus = !changedStatus;
    }
}
