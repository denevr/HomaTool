using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class StoreToolWindow : EditorWindow
{
    //string shopItemName = "Enter item name here";
    bool groupEnabled;

    [MenuItem("Tools/Store Tool Window")]
    static void Init()
    {
        StoreToolWindow window = (StoreToolWindow)EditorWindow.GetWindow(typeof(StoreToolWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Instructions", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Write instructions here");
        EditorGUILayout.HelpBox("Please make sure you have read instructions clearly before continuing.", MessageType.Warning);

        groupEnabled = EditorGUILayout.BeginToggleGroup("I have read the instructions and completed each step as requested.", groupEnabled);
        if (GUILayout.Button("Button"))
        {
            SetStoreItems();
        }
        EditorGUILayout.EndToggleGroup();

        GUILayout.Label("v0.0.1", EditorStyles.miniLabel);
    }

    public void SetStoreItems()
    {
        // For testing purposes only, delete later
        var newShopItemName = "example_store_item_1";

        // Get all fbx and turn it into proper prefab with attributes
        GeneratePrefabFromFBXModel(newShopItemName);

        // TODO: Get sprites and make necessary settings
        // TODO: Match prefabs with UI icons and add to store.instance.storeitems as new storeItem
    }

    static void GeneratePrefabFromFBXModel(string itemName)
    {
        string[] sourcePathArr =
        {
            "Assets","Resources", "Models"
        };

        string[] destinationPathArr =
        {
            "Assets","2_Prefabs"
        };

        string sourcePath = Path.Combine(Path.Combine(sourcePathArr), itemName + ".fbx");
        string destinationPath = Path.Combine(Path.Combine(destinationPathArr), itemName + ".prefab");

        ModelImporter importer = AssetImporter.GetAtPath(sourcePath) as ModelImporter;
        importer.animationType = ModelImporterAnimationType.Human;
        importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();

        GameObject fbxToPrefab = AssetDatabase.LoadAssetAtPath(sourcePath, typeof(GameObject)) as GameObject;
        var tmp = Instantiate(fbxToPrefab);

        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAsset(tmp, destinationPath, out prefabSuccess);

        if (prefabSuccess == true)
        {
            Debug.Log("Prefab was saved successfully");

            //GameObject prefab = PrefabUtility.LoadPrefabContents(destinationPath);
            // TODO: Make prefab settings here.
        }
        else
            Debug.LogError("Prefab failed to save");

        DestroyImmediate(tmp);
    }
}
