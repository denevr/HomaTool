using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class StoreToolPopup : EditorWindow
{
    Vector2 scrollPosition = Vector2.zero;

    public List<string> fbxPathList;
    public List<string> pngPathList;
    public List<string> prefabPathList;

    public List<string> potentiallyMissingFbxPathList = new List<string>();
    public List<string> potentiallyMissingPngPathList = new List<string>();
    public List<string> potentiallyMissingPrefabPathList = new List<string>();
    public List<string> potentiallyMissingItems = new List<string>();

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

        //potentially missing files
        GUI.enabled = potentiallyMissingItems.Count > 0 ? true : false;

        if (potentiallyMissingItems.Count > 0)
        {
            GUILayout.Space(20);
            GUI.color = Color.yellow;
            GUILayout.Label("Potentially missing items:", EditorStyles.boldLabel);
            GUI.color = Color.white;
            for (int i = 0; i < potentiallyMissingItems.Count; i++)
                EditorGUILayout.LabelField(potentiallyMissingItems[i], EditorStyles.wordWrappedLabel);

            if (GUILayout.Button("Add Item(s)", GUILayout.Height(20)))
            {
                StoreToolWindow window = (StoreToolWindow)EditorWindow.GetWindow(typeof(StoreToolWindow));
                var commaSeparatedMissingItems = string.Join(",", potentiallyMissingItems);
                window.SetStoreItems(commaSeparatedMissingItems);
            }
        }

        GUI.enabled = true;
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

    public void CheckForPotentiallyMissingStoreItems()
    {
        potentiallyMissingItems.Clear();

        // make single iteration with another fcn
        string[] fbxFilesProcessed = new string[fbxPathList.Count];
        string[] pngFilesProcessed = new string[pngPathList.Count];

        // get names of all processed items
        for (int i = 0; i < fbxFilesProcessed.Length; i++)
            fbxFilesProcessed[i] = Path.GetFileNameWithoutExtension(fbxPathList[i]);
        for (int i = 0; i < pngFilesProcessed.Length; i++)
            pngFilesProcessed[i] = Path.GetFileNameWithoutExtension(pngPathList[i]);

        // get names of all fbx files in the project
        var fbxFiles = Directory.EnumerateFiles("Assets", "*.*", SearchOption.AllDirectories)
            .Where(s => s.ToLower().EndsWith(".fbx")).ToArray();
        for (int i = 0; i < fbxFiles.Length; i++)
            fbxFiles[i] = Path.GetFileNameWithoutExtension(fbxFiles[i]);

        // get names of all png files in the project
        var pngFiles = Directory.EnumerateFiles("Assets", "*.*", SearchOption.AllDirectories)
            .Where(s => s.ToLower().EndsWith(".png")).ToArray();
        for (int i = 0; i < pngFiles.Length; i++)
            pngFiles[i] = Path.GetFileNameWithoutExtension(pngFiles[i]);

        // filter for unique elements
        fbxFiles.GroupBy(i => i).Where(g => g.Count() == 1).Select(g => g.Key);
        pngFiles.GroupBy(i => i).Where(g => g.Count() == 1).Select(g => g.Key);

        var intersect = fbxFiles.Intersect(pngFiles);
        //var currentStoreItems = Store.Instance.StoreItems.

        // if an fbx and png file has the same name and is missing in the current store, classify it as a potentially forgotten element
        foreach (var commonFile in intersect)
        {
            if (!fbxFilesProcessed.Contains(commonFile) || !pngFilesProcessed.Contains(commonFile))
            {
                if (Store.Instance.StoreItems.FindAll(x => x.Name == commonFile).Count == 0)
                    potentiallyMissingItems.Add(commonFile);
            }
        }
    }
}