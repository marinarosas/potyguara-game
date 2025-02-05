using UnityEngine;
using System;

public class EditSkin : SkinSystem
{
    void Start()
    {
        Debug.Log("Modo de edicao de skin");
    }

    public void saveSkin() //TODO: Mudar retorno pra bool dps de pronta
    {
    //    TODO: mandar pro server
    //    try
    //    {
    //        indexSkin --> server
    //        indexMaterial --> server
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError($"Error when trying to save skin: {ex.Message}");
    //        return false;
    //    }
    //    return true;
    }
}
