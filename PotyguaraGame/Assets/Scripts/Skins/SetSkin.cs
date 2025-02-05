using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkin : SkinSystem
{
    public static SkinSystem SetSkinInstance = null;
    private void Awake()
    {
        if (SetSkinInstance == null)
            SetSkinInstance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //TODO: puxar do server

        //int skinIndex = ...
        //int skinMaterial = ...
        // -1 significa dado nao salvo
        //bool serverHasSavedSkin = (skinIndex > -1 && skinMaterial > -1);
        //if (skinContainer.childCount == 0 && serverHasSavedSkin)
        //{
        //    setSkin(skinIndex, skinMaterial);
        //    Debug.Log("Recuperando skin...");
        //}
    }

    public void setSkin(int skinIndex, int skinMaterial)
    {
        try
        {
            indexSkin = skinIndex;
            currentSkin = skins[skinIndex];
            toggleVisibleMeshes(true);
            UpdateBodyMesh(currentSkin);
            changeMaterial(skinMaterial);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error when trying to set skin: {ex.Message}");
        }
    }
}
