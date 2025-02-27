using UnityEngine;
using System;

public class EditSkin : SkinSystem
{
    void Start()
    {
        Debug.Log("Modo de edicao de skin");
    }

    public void saveSkin() 
    {
        /*try
        {
            int skinIndex = FindFirstObjectByType<PotyPlayerController>().GetIndex();
            int skinMaterial = FindFirstObjectByType<PotyPlayerController>().GetVariant();
            int skinGender = FindFirstObjectByType<PotyPlayerController>().GetGender();

        }
        catch (Exception ex)
        {
            Debug.LogError($"Error when trying to save skin: {ex.Message}");
            return false;
        }
        return true;*/
    }
}
