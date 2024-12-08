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

    #endregion

    private void OnEnable(){
        skins = serializedObject.FindProperty("skins");
        currentSkin = serializedObject.FindProperty("currentSkin");

        SkinSystem _skinSystem = (SkinSystem)target;
        currentSkin = skins.GetArrayElementAtIndex(0);
    }

    public override void OnInspectorGUI()
    {
        SkinSystem _skinSystem = (SkinSystem)target;

        //base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.LabelField("SKIN SET", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(skins, new GUIContent("Skins", "Texture swapping is enabled when adding more than one material"));

        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("SELECT SKIN", EditorStyles.boldLabel);

        #region POPUP FIELD (ENUM-LIKE)
            List<string> skinNames = new List<string>();
            for (int i = 0; i < skins.arraySize; i++){
                SerializedProperty skin = skins.GetArrayElementAtIndex(i);
                string skinName = skin.FindPropertyRelative("name").stringValue;
                skinNames.Add(skinName);
            }
            int selectedIndex = skinNames.IndexOf(_skinSystem.currentSkin.getName());
            if (selectedIndex < 0) selectedIndex = 0;
            selectedIndex = EditorGUILayout.Popup(selectedIndex, skinNames.ToArray());

            if (selectedIndex >= 0 && selectedIndex < skinNames.Count){
                //_skinSystem.currentSkin = _skinSystem.skins[selectedIndex];
                //currentSkin.GetArrayElementAtIndex(selectedIndex);
                _skinSystem.changeMesh(selectedIndex);
                EditorUtility.SetDirty(_skinSystem);
            }
        #endregion

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Or use arrows");

        #region ARROWS
            GUILayout.BeginHorizontal();
                Arrows(ArrowType.Mesh);
                //GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
                GUILayout.Label("", GUILayout.Width(20));
                GUILayout.BeginVertical();
                    EditorGUILayout.LabelField("EDIT CURRENT SKIN:");
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("currentSkin"));
                GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        #endregion

        //CAMPO CONDICIONAL
        if (_skinSystem.currentSkin.materialsSize() > 1){
            GUILayout.Space(10);
            int index = _skinSystem.getMaterialIndex();
            EditorGUILayout.LabelField("SELECT TEXTURE/COLOR: "+_skinSystem.currentSkin.getMaterialName(index), EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
                Arrows(ArrowType.Material);

            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
            GUILayout.Label("Reset skin system:", EditorStyles.boldLabel);
            if (GUILayout.Button("Reset"))
            {
                currentSkin = skins.GetArrayElementAtIndex(0);
                _skinSystem.disableMeshes();
                _skinSystem.changeMesh(0); //reset
                _skinSystem.currentSkin = _skinSystem.skins[0];
                _skinSystem.resetMaterial();
                Debug.Log("Skin system reseted for this character");

                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(_skinSystem);
            }
        GUILayout.EndHorizontal();

        EditorUtility.SetDirty(_skinSystem);
        serializedObject.ApplyModifiedProperties();
    }

    //List, action1, action2
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
        GUI.enabled = true;
    }

}
#endif