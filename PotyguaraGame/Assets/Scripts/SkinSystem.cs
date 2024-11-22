using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinSystem : MonoBehaviour
{
    public GameObject standardSkin = null;
    public List<GameObject> skins;

    public struct Skin
    {
        public int id;
        public string name;

        private Mesh standardSkinMesh;
        private Material[] skin;

        private Tuple<Mesh, Material>[] acessories;
    }

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

    public GameObject GetSkinStandard()
    {
        return standardSkin;
    }



}
