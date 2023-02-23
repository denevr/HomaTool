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

    void GeneratePrefabFromFBXModel(string itemName)
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

            SetPrefabComponents(destinationPath);
            SetPrefabColors(destinationPath, "Purple");
        }
        else
            Debug.LogError("Prefab failed to save");

        DestroyImmediate(tmp);
    }

    void SetPrefabComponents(string prefabPath)
    {
        GameObject prefabToModify = PrefabUtility.LoadPrefabContents(prefabPath);

        // Add CapsuleCollider component
        CapsuleCollider capsuleCollider = prefabToModify.AddComponent<CapsuleCollider>();
        capsuleCollider.center = new Vector3(0, .55f, 0);
        capsuleCollider.radius = .2f;
        capsuleCollider.height = 1.1f;

        // Add Animator component
        string[] controllerPath =
        {
            "Assets", "1_Graphics", "AnimatorControllers"
        };

        string animatorControllerPath = Path.Combine(Path.Combine(controllerPath), "Controller.controller");
        Animator animator = prefabToModify.GetComponent<Animator>();

        if (animator == null)
            animator = prefabToModify.AddComponent<Animator>();

        RuntimeAnimatorController runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(animatorControllerPath);

        if (runtimeAnimatorController != null)
            animator.runtimeAnimatorController = runtimeAnimatorController;
        else
            Debug.LogError("Animator controller named " + runtimeAnimatorController + " has not been found in path: " + animatorControllerPath);

        PrefabUtility.SaveAsPrefabAsset(prefabToModify, prefabPath);
    }

    void SetPrefabColors(string prefabPath, string materialName)
    {
        GameObject prefabToModify = PrefabUtility.LoadPrefabContents(prefabPath);

        string[] materialsPath =
        {
            "Assets", "1_Graphics", "Materials"
        };

        string meshRendererMaterialsPath = Path.Combine(Path.Combine(materialsPath), materialName + ".mat");
        Material targetMaterial = AssetDatabase.LoadAssetAtPath<Material>(meshRendererMaterialsPath);

        if (targetMaterial != null)
        {
            SkinnedMeshRenderer smr = prefabToModify.GetComponentInChildren<SkinnedMeshRenderer>();
            var mats = smr.sharedMaterials;

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = targetMaterial;
            }

            smr.sharedMaterials = mats;
            PrefabUtility.SaveAsPrefabAsset(prefabToModify, prefabPath);
        }
        else
            Debug.LogError("Material named " + materialName + " has not been found in path: " + meshRendererMaterialsPath);
    }
}
