using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public enum Option { GENDER, SKIN, VARIANT };

public class AvatarOptionController : MonoBehaviour
{
    [SerializeField] private Option option;
    [Header("Menu items")]
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private PaginationManager pagination;
    [SerializeField] private AvatarMenuController menuController;
    public int index = 0;

    private SkinSystem editSkin = null;
    private List<GameObject> bodies;
    private List<Skin> skins;
    private List<SkinMaterial> materials;

    private void Start()
    {
        switch (option)
        {
            case Option.GENDER:
                left.onClick.AddListener(() => PreviousMenu(bodies));
                right.onClick.AddListener(() => NextMenu(bodies));
                break;

            case Option.SKIN:
                left.onClick.AddListener(() => PreviousMenu(skins));
                right.onClick.AddListener(() => NextMenu(skins));
                break;

            case Option.VARIANT:
                left.onClick.AddListener(() => PreviousMenu(materials));
                right.onClick.AddListener(() => NextMenu(materials));
                break;
        }
    }

    public string GetOption()
    {
        switch (option)
        {
            case Option.GENDER:
                return editSkin.name;
            case Option.SKIN:
                return index.ToString();
            case Option.VARIANT:
                return index.ToString();
            default:
                return "";
        }
    }

    public void setList(int index){
        this.index = index;
        editSkin = menuController.editSkin;
        switch (option)
        {
            case Option.GENDER:
                bodies = menuController.bodies;
                pagination.SetPagination(bodies.Count);
                break;

            case Option.SKIN:
                skins = menuController.skins;
                pagination.SetPagination(skins.Count);
                break;

            case Option.VARIANT:
                materials = menuController.materials;
                pagination.SetPagination(materials.Count);
                break;
        }
        SetOption();
    }

    void NextMenu<T>(List<T> menu)
    {
        if (index == menu.Count - 1)
            index = 0;
        else
            index++;

        SetOption();
    }

    void PreviousMenu<T>(List<T> menu)
    {
        if (index <= 0)
            index = menu.Count - 1;
        else
            index--;

        SetOption();
    }

    void SetOption()
    {
        pagination.UpdatePagination(index);

        switch (option)
        {
            case Option.GENDER:
                refreshGenders(index);
                break;
            case Option.SKIN:
                editSkin.changeMesh(index);
                menuController.SetVariantList(index, 0);
                editSkin.GetComponent<Animator>().Play("Look at yourself", -1, 0f);
                break;
            case Option.VARIANT:
                editSkin.changeMaterial(index);
                break;
        }
        UpdateText();
    }

    private void UpdateText()
    {
        switch (option)
        {
            case Option.GENDER:
                label.text = editSkin.name;
                break;
            case Option.SKIN:
                label.text = skins[index].name;
                break;
            case Option.VARIANT:
                label.text = materials[index].name;
                break;
        }
    }

    public void refreshGenders(int index)
    {
        editSkin = bodies[index].GetComponent<SkinSystem>();
        for (int i = 0; i < bodies.Count; i++)
            bodies[i].SetActive(i == index);
    }
}
