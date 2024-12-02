using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTION { Decrease, Increase }

[System.Serializable]
public class Skin{
    private int id;
    [SerializeField] private string name;
    [SerializeField] public SkinnedMeshRenderer skinMesh;

    [SerializeField] private SkinMaterial[] skinMaterials;
    //[SerializeField] private Tuple<Mesh, Material>[] acessories;

    public string getName() => name;

    public void toogleVisible(bool status) => skinMesh.gameObject?.SetActive(status);

    public int materialsSize() => skinMaterials.Length;

    public void changeMaterial(int materialIndex)
    {
        Material[] materials = skinMesh.sharedMaterials;
        materials[0] = skinMaterials[materialIndex].material;
        skinMesh.sharedMaterials = materials;
    }

    public string getMaterialName(int index) => skinMaterials[index].name;
}

[System.Serializable]
public class SkinMaterial
{
    public string name;
    public Material material;
}


public class SkinSystem : MonoBehaviour{
    [SerializeField] private GameObject defaultSkin = null;
    [SerializeField] public List<Skin> skins;
    [SerializeField] public Skin currentSkin;
    
    private int indexSkin = 0;
    private int oldIndexSkin = -1;
    private int indexMaterial = 0;

    public static SkinSystem Instance = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public GameObject GetSkinDefault()
    {
        return defaultSkin;
    }

    private void FixedUpdate()
    {
        //Debug.Log(currentSkin.getName());
    }

    #region publicFunctions

    public bool changeMesh(DIRECTION direction)
    {
        try {
            if ((direction == DIRECTION.Decrease && indexSkin > 0) ||
                (direction == DIRECTION.Increase && indexSkin < skins.Count - 1))
            {
                skins[indexSkin].toogleVisible(false);

                indexSkin += (direction == DIRECTION.Increase) ? 1 : -1;
                oldIndexSkin = indexSkin;

                skins[indexSkin].toogleVisible(true);

                currentSkin = skins[indexSkin];

                indexMaterial = 0;
                skins[indexSkin].changeMaterial(indexMaterial);

                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error when trying to change skins ({direction}): {ex.Message}");
            return false;
        }
    }


    //TODO: VERIFICAO SE MUDOU!!!
    public void changeMesh(int index)
    {
        if (oldIndexSkin == index)
            return;

        oldIndexSkin = index;
        skins[indexSkin].toogleVisible(false);
        indexSkin = index;
        skins[indexSkin].toogleVisible(true);
        currentSkin = skins[indexSkin];

        indexMaterial = 0;
        skins[indexSkin].changeMaterial(indexMaterial);
    }

    public bool changeMaterial(DIRECTION direction)
    {
        try{
            if ((direction == DIRECTION.Decrease && indexMaterial > 0) || 
                (direction == DIRECTION.Increase && indexMaterial < currentSkin.materialsSize() - 1))
            {
                indexMaterial += (direction == DIRECTION.Increase) ? 1 : -1;
                currentSkin.changeMaterial(indexMaterial);

                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error when trying to change material ({direction}): {ex.Message}");
            return false;
        }
    }

    public int getMaterialIndex() => indexMaterial;

    #endregion
}
