using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;


public class AvatarMenuController : MonoBehaviour
{
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private TextMeshProUGUI label;
    public int index = 0;

    // puxar da AWS
    private List<string> skins = new List<string> { "padrão", "fantasma", "papai noel", "surfista", "alien" };
    private List<string> materials = new List<string> { "preto", "amarelo", "azul", "branco", "roxo", "verde" };

    private void Start()
    {
        if (transform.name == "Skin") {
            left.onClick.AddListener(() => PreviousMenu(skins));
            right.onClick.AddListener(() => NextMenu(skins));
        }
        else
        {
            left.onClick.AddListener(() => PreviousMenu(materials));
            right.onClick.AddListener(() => NextMenu(materials));
        }
    }

    private void Update()
    {
        if(transform.name == "Skin")
            label.text = skins[index];
        else
            label.text = materials[index];
    }

    void NextMenu(List<string> menu)
    {
        if (index == menu.Count - 1)
            index = 0;
        else
            index++;
    }

    void PreviousMenu(List<string> menu)
    {
        if (index <= 0)
            index = menu.Count - 1;
        else
            index--;
    }
}
