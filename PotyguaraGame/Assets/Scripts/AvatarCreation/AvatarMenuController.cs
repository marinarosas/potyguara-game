using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMenuController : MonoBehaviour
{
    [Header("Chars")]
    [SerializeField] private AvatarOptionController[] options;
    public SkinSystem editSkin;
    public List<GameObject> bodies;
    public List<Skin> skins;
    public List<SkinMaterial> materials;

    private int bodyIndex = 0;
    private int skinIndex = 0;
    private int skinMaterial = 0;

    private void Start()
    {
        //TODO: aqui entra condicao pra puxar do banco ou deixar padrao (0)

        // -1 significa dado nao salvo
        //bool serverHasSavedSkin = (skinIndex > -1 && skinMaterial > -1);
        //if (skinContainer.childCount == 0 && serverHasSavedSkin)
        //{
        //    setSkin(skinIndex, skinMaterial);
        //    Debug.Log("Recuperando skin...");
        SkinSystem skinSystem = bodies[0].GetComponent<SkinSystem>();
        if (skinSystem != null)
        {
            foreach (Transform child in skinSystem.skinContainer)
            {
                GameObject.Destroy(child.gameObject);
            }
        }


        editSkin = bodies[bodyIndex].GetComponent<SkinSystem>();
        skins = editSkin.skins;
        materials = new List<SkinMaterial> (skins[skinIndex].skinMaterials);

        IniciateMenu(Option.BODY, bodyIndex, skinIndex, skinMaterial);
    }

    public void IniciateMenu(Option option, int bodyIndex, int skinIndex, int skinMaterial)
    {
        SetBodyList(bodyIndex);
        SetSkinList(skinIndex);
        SetVariantList(skinIndex, skinMaterial);
    }

    public void SendChosenSkin()
    {
        if (!SteamManager.Initialized)
            return;

        Achievement.Instance.UnclockAchievement("criando_vida");

        int skinIndex = int.Parse(options[(int)Option.SKIN].GetOption());
        int variant = int.Parse(options[(int)Option.VARIANT].GetOption());

        FindFirstObjectByType<NetworkManager>().SendUpdateSkin(bodyIndex, skinIndex, variant);
        FindFirstObjectByType<PotyPlayerController>().SetSkin(bodyIndex, skinIndex, variant);
        FindFirstObjectByType<TransitionController>().LoadSceneAsync(2);
    }


    public void SetBodyList(int bodyIndex)
    {
        options[(int)Option.BODY].setList(bodyIndex);
    }

    public void SetSkinList(int skinIndex)
    {
        options[(int)Option.SKIN].setList(skinIndex);
    }

    public void SetVariantList(int skinIndex, int skinMaterial)
    {
        materials = new List<SkinMaterial>(skins[skinIndex].skinMaterials);
        options[(int)Option.VARIANT].setList(skinMaterial);
    }

    public void refreshBody(int index)
    {
        bodyIndex = index;
        skinIndex = 0;
        skinMaterial = 0;

        editSkin = bodies[bodyIndex].GetComponent<SkinSystem>();
        skins = editSkin.skins;
        materials = new List<SkinMaterial>(skins[skinIndex].skinMaterials);

        for (int i = 0; i < bodies.Count; i++)
            bodies[i].SetActive(i == index);

        SetSkinList(skinIndex);
        SetVariantList(skinIndex, skinMaterial);
        editSkin.changeMesh(skinIndex);
        options[(int)Option.BODY].refreshController();
        options[(int)Option.SKIN].refreshController();
        options[(int)Option.BODY].UpdateText();
    }
}
