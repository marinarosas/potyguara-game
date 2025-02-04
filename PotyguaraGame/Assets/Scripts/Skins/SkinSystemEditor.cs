using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

enum ArrowType { Mesh, Material }

#if UNITY_EDITOR
[CustomEditor(typeof(SkinSystem))]
public class SkinSystemEditor : Editor
{
    #region SerializedProps
    SerializedProperty skins;
    SerializedProperty currentSkin;
    SerializedProperty hair, head, chest, belly, arms, forearms, hands, hips, legs, ankles, feet;
    SerializedProperty skinContainer, rootBone;

    #endregion

    private void OnEnable(){
        skins = serializedObject.FindProperty("skins");
        currentSkin = serializedObject.FindProperty("currentSkin");

        hair = serializedObject.FindProperty("hair");
        head = serializedObject.FindProperty("head");
        chest = serializedObject.FindProperty("chest");
        belly = serializedObject.FindProperty("belly");
        arms = serializedObject.FindProperty("arms");
        forearms = serializedObject.FindProperty("forearms");
        hands = serializedObject.FindProperty("hands");
        hips = serializedObject.FindProperty("hips");
        legs = serializedObject.FindProperty("legs");
        ankles = serializedObject.FindProperty("ankles");
        feet = serializedObject.FindProperty("feet");

        skinContainer = serializedObject.FindProperty("skinContainer");
        rootBone = serializedObject.FindProperty("rootBone");
        currentSkin = skins.GetArrayElementAtIndex(0);
    }

    public override void OnInspectorGUI()
    {
        SkinSystem _skinSystem = (SkinSystem)target;

        //base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.PropertyField(rootBone, new GUIContent("Root bone - Rig armature"));
        EditorGUILayout.PropertyField(skinContainer, new GUIContent("Skin container"));
        GUILayout.Space(15);

        EditorGUILayout.PropertyField(ankles, new GUIContent("ankles"));
        EditorGUILayout.PropertyField(arms, new GUIContent("arms"));
        EditorGUILayout.PropertyField(belly, new GUIContent("belly"));
        EditorGUILayout.PropertyField(chest, new GUIContent("chest"));
        EditorGUILayout.PropertyField(feet, new GUIContent("feet"));
        EditorGUILayout.PropertyField(forearms, new GUIContent("forearms"));
        EditorGUILayout.PropertyField(hair, new GUIContent("hair"));
        EditorGUILayout.PropertyField(hands, new GUIContent("hands"));
        EditorGUILayout.PropertyField(head, new GUIContent("head"));
        EditorGUILayout.PropertyField(hips, new GUIContent("hips"));
        EditorGUILayout.PropertyField(legs, new GUIContent("legs"));
        GUILayout.Space(15);

        EditorGUILayout.LabelField("SKIN SET", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(skins, new GUIContent("Skins", "Texture swapping is enabled when adding more than one material"));

        GUILayout.Space(15);
        EditorGUILayout.LabelField("SELECT SKIN", EditorStyles.boldLabel);

        #region POPUP FIELD (ENUM-LIKE)
        List<string> skinNames = new List<string>();
            for (int i = 0; i < skins.arraySize; i++){
                SerializedProperty skin = skins.GetArrayElementAtIndex(i);
                string skinName = _skinSystem.getSkinName(i);
                skinNames.Add(skinName);
            }
            int selectedIndex = skinNames.IndexOf(_skinSystem.currentSkin.getName());
            if (selectedIndex < 0) selectedIndex = 0;
            selectedIndex = EditorGUILayout.Popup(selectedIndex, skinNames.ToArray());

            if (selectedIndex >= 0 && selectedIndex < skinNames.Count)
            {
                _skinSystem.changeMesh(selectedIndex);
                EditorUtility.SetDirty(_skinSystem);
            }
        #endregion


        EditorGUILayout.LabelField("Or use arrows");

        #region ARROWS
            GUILayout.BeginHorizontal();
                Arrows(ArrowType.Mesh);
                //GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            //GUILayout.BeginHorizontal();
            //    GUILayout.Label("", GUILayout.Width(20));
            //    GUILayout.BeginVertical();
            //        EditorGUILayout.LabelField("EDIT CURRENT SKIN:");
            //        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentSkin"));
            //    GUILayout.EndVertical();
            //GUILayout.EndHorizontal();

        #endregion

        //CAMPO CONDICIONAL
        if (_skinSystem.currentSkin.materialsSize() > 1)
        {
            GUILayout.Space(10);
            int index = _skinSystem.getMaterialIndex();
            EditorGUILayout.LabelField("SELECT TEXTURE/COLOR: " + _skinSystem.currentSkin.getMaterialName(index), EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            Arrows(ArrowType.Material);

            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);
            GUILayout.Label("Reset skin system:", EditorStyles.boldLabel);
            if (GUILayout.Button("Reset"))
            {
                currentSkin = skins.GetArrayElementAtIndex(0);
                //_skinSystem.disableMeshes();
                _skinSystem.changeMesh(0); //reset
                _skinSystem.currentSkin = _skinSystem.skins[0];
                _skinSystem.changeMaterial(0);
                _skinSystem.setMaterialIndex(0);
                Debug.Log("Skin system reseted for this character");

                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(_skinSystem);
            }

        EditorUtility.SetDirty(_skinSystem);
        serializedObject.ApplyModifiedProperties();
    }

    private void Arrows(ArrowType type)
    {
        SkinSystem _skinSystem = (SkinSystem)target;
        int currentIndex = _skinSystem.getIndex();
        int skinCount = skins.arraySize;
        int materialIndex = _skinSystem.getMaterialIndex();
        int materialCount = _skinSystem.currentSkin.materialsSize();

        if (type == ArrowType.Mesh)
        {
            GUI.enabled = currentIndex > 0;
        }
        else if (type == ArrowType.Material)
        {
            GUI.enabled = materialIndex > 0;
        }

        if (GUILayout.Button("<", GUILayout.Width(45), GUILayout.Height(45)))
        {
            if (type == ArrowType.Mesh)
                _skinSystem.changeMesh(DIRECTION.Decrease);
            else
                _skinSystem.changeMaterial(DIRECTION.Decrease);
        }

        if (type == ArrowType.Mesh)
        {
            GUI.enabled = currentIndex < skinCount - 1;
        }
        else if (type == ArrowType.Material)
        {
           GUI.enabled = materialIndex < materialCount - 1;
        }

        if (GUILayout.Button(">", GUILayout.Width(45), GUILayout.Height(45)))
        {
            if (type == ArrowType.Mesh)
                _skinSystem.changeMesh(DIRECTION.Increase);
            else
                _skinSystem.changeMaterial(DIRECTION.Increase);
        }

        EditorUtility.SetDirty(_skinSystem);
        Repaint();
        GUI.enabled = true;
    }

}
#endif