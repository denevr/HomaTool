using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StoreToolPopup : EditorWindow
{
    Vector2 scrollPosition = Vector2.zero;

    public List<string> fbxPathList;
    public List<string> pngPathList;
    public List<string> prefabPathList;

    public List<string> potentiallyMissingFbxPathList = new List<string>();
    public List<string> potentiallyMissingPngPathList = new List<string>();
    public List<string> potentiallyMissingPrefabPathList = new List<string>();

    void OnGUI()
    {
        EditorGUILayout.LabelField("Store Tool Report", EditorStyles.wordWrappedLabel);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        //generated/used files
        GUILayout.Space(20);
        GUI.color = Color.green;
        GUILayout.Label("Models (" + fbxPathList.Count + ") processed in the process:", EditorStyles.boldLabel);
        GUI.color = Color.white;
        for (int i = 0; i < fbxPathList.Count; i++)
            EditorGUILayout.LabelField(fbxPathList[i], EditorStyles.wordWrappedLabel);

        GUILayout.Space(20);
        GUI.color = Color.green;
        GUILayout.Label("2D UIs (" + pngPathList.Count + ") processed in the process:", EditorStyles.boldLabel);
        GUI.color = Color.white;
        for (int i = 0; i < pngPathList.Count; i++)
            EditorGUILayout.LabelField(pngPathList[i], EditorStyles.wordWrappedLabel);

        GUILayout.Space(20);
        GUI.color = Color.green;
        GUILayout.Label("Prefabs (" + prefabPathList.Count + ") generated in the process:", EditorStyles.boldLabel);
        GUI.color = Color.white;
        for (int i = 0; i < prefabPathList.Count; i++)
            EditorGUILayout.LabelField(prefabPathList[i], EditorStyles.wordWrappedLabel);

        //potentially missing files
        GUILayout.Space(20);
        GUI.color = Color.red;
        GUILayout.Label("All unprocessed .fbx files in the project:", EditorStyles.boldLabel);
        GUI.color = Color.white;
        for (int i = 0; i < potentiallyMissingFbxPathList.Count; i++)
            EditorGUILayout.LabelField(potentiallyMissingFbxPathList[i], EditorStyles.wordWrappedLabel);

        GUILayout.Space(20);
        GUI.color = Color.red;
        GUILayout.Label("All unprocessed .png files in the project:", EditorStyles.boldLabel);
        GUI.color = Color.white;
        for (int i = 0; i < potentiallyMissingPngPathList.Count; i++)
            EditorGUILayout.LabelField(potentiallyMissingPngPathList[i], EditorStyles.wordWrappedLabel);

        GUILayout.Space(20);
        GUI.color = Color.red;
        GUILayout.Label("All unprocessed .prefab files in the project:", EditorStyles.boldLabel);
        GUI.color = Color.white;
        for (int i = 0; i < potentiallyMissingPrefabPathList.Count; i++)
            EditorGUILayout.LabelField(potentiallyMissingPrefabPathList[i], EditorStyles.wordWrappedLabel);

        GUILayout.Space(20);
        if (GUILayout.Button("OK", GUILayout.Height(50))) 
            this.Close();

        GUILayout.EndScrollView();
    }

    public void GetAllUnprocessedFileTypesOfExtension(string fileExtension, List<string> potentiallyMissingFileList, List<string> processedFileList)
    {
        potentiallyMissingFileList.Clear();

        foreach (string file in Directory.EnumerateFiles("Assets", "*"+ fileExtension, SearchOption.AllDirectories))
        {
            if (!processedFileList.Contains(file))
                potentiallyMissingFileList.Add(file);
        }
    }
}