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

        editSkin = bodies[bodyIndex].GetComponent<SkinSystem>();
        skins = editSkin.skins;
        materials = new List<SkinMaterial> (skins[skinIndex].skinMaterials);

        IniciateMenu(Option.GENDER, bodyIndex, skinIndex, skinMaterial);
    }

    public void IniciateMenu(Option option, int bodyIndex, int skinIndex, int skinMaterial)
    {
        SetGenderList(bodyIndex);
        SetSkinList(skinIndex);
        SetVariantList(skinIndex, skinMaterial);
    }

    public void SendChosenSkin()
    {
        if (!SteamManager.Initialized)
            return;

        Achievement.Instance.UnclockAchievement("criando_vida");

        string gender = options[(int)Option.GENDER].GetOption();
        int skinIndex = int.Parse(options[(int)Option.SKIN].GetOption());
        int variant = int.Parse(options[(int)Option.VARIANT].GetOption());

        if (gender.ToLower().Equals("masculino"))
        {
            FindFirstObjectByType<NetworkManager>().SendUpdateSkin(0, skinIndex, variant);
            FindFirstObjectByType<PotyPlayerController>().potyPlayer.SetSkin(0, skinIndex, variant);
        }
        else
        {
            FindFirstObjectByType<NetworkManager>().SendUpdateSkin(1, skinIndex, variant);
            FindFirstObjectByType<PotyPlayerController>().potyPlayer.SetSkin(1, skinIndex, variant);
        }
        FindFirstObjectByType<TransitionController>().LoadSceneAsync(2);
    }


    public void SetGenderList(int bodyIndex)
    {
        options[(int)Option.GENDER].setList(bodyIndex);
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
}
