using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkin : MonoBehaviour
{
    public List<GameObject> bodies;

    public void SetSkinAvatar(int skinIndex, int skinMaterial, int skinGender)
    {
        bool serverHasSavedSkin = (skinIndex > -1 && skinMaterial > -1 && skinGender > -1);
        if (serverHasSavedSkin)
        {
            try
            {
                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).gameObject.SetActive(i == skinGender);
                SkinSystem editSkin = transform.GetChild(skinGender).GetComponent<SkinSystem>();
                editSkin.changeMesh(skinIndex);
                editSkin.changeMaterial(skinMaterial);
                Debug.Log("Recuperando skin... "+ skinGender+" "+ skinIndex+" "+ skinMaterial);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error when trying to set skin: {ex.Message}");
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            SkinSystem editSkin = transform.GetChild(skinGender).GetComponent<SkinSystem>();
            editSkin.changeMesh(0);
            editSkin.changeMaterial(0);
        }
    }
}
