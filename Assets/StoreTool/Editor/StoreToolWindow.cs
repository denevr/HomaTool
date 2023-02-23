using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class StoreToolWindow : EditorWindow
{
    string version = "v0.0.1";
    string inputString;
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
        inputString = EditorGUILayout.TextField("Enter item names here as comma separated values.", inputString);
        if (GUILayout.Button("Create Shop Items"))
        {
            if (!string.IsNullOrEmpty(inputString))
                SetStoreItems(inputString);
            else
                Debug.LogError("Please provide a valid input.");
        }
        EditorGUILayout.EndToggleGroup();

        GUILayout.Label(version, EditorStyles.miniLabel);
    }

    public void SetStoreItems(string commaSeparatedInput)
    {
        string[] newShopItemsArr = commaSeparatedInput.Split(',');
        string newShopItemName;

        for (int i = 0; i < newShopItemsArr.Length; i++)
        {
            newShopItemName = newShopItemsArr[i];

            GeneratePrefabFromFBXModel(newShopItemName);
            SetUIStoreItems(newShopItemName);
            AddItemToStore(newShopItemName);
        }

        // TODO: Show report for potentially forgotten assets.
        inputString = "Enter item names here as comma separated values.";
        groupEnabled = false;
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

    void SetUIStoreItems(string itemName)
    {
        string[] texturePathArr =
        {
            "Assets", "Resources", "Sprites"
        };

        string texturePath = Path.Combine(Path.Combine(texturePathArr), itemName + ".png");

        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(texturePath);
        TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        //importer.isReadable = true;
        importer.textureType = TextureImporterType.Sprite;
        //TODO: Size and scale optimisation.
        importer.wrapMode = TextureWrapMode.Clamp;
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
    }

    void AddItemToStore(string itemName)
    {
        // get store item prefab
        string[] prefabsPathArr =
        {
            "Assets","2_Prefabs"
        };

        string prefabPath = Path.Combine(Path.Combine(prefabsPathArr), itemName + ".prefab");

        // get store item icon
        string[] spritesPathArr =
        {
            "Assets", "Resources", "Sprites"
        };

        string spritesPath = Path.Combine(Path.Combine(spritesPathArr), itemName + ".png");

        StoreItem storeItem = new StoreItem();
        storeItem.Id = Store.Instance.StoreItems.Count;
        storeItem.Name = itemName;
        storeItem.Price = (Store.Instance.StoreItems.Count + 1) * 100;
        storeItem.Icon = AssetDatabase.LoadAssetAtPath<Sprite>(spritesPath);
        storeItem.Prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;

        bool isStoreItemDuplicate = false;

        for (int i = 0; i < Store.Instance.StoreItems.Count; i++)
        {
            if (Store.Instance.StoreItems[i].Name == storeItem.Name)
            {
                isStoreItemDuplicate = true;
                break;
            }
        }

        if (!isStoreItemDuplicate)
        {
            Store.Instance.StoreItems.Add(storeItem);
            Debug.Log(storeItem.Name + " is added to the store.");
        }
        else
            Debug.LogError("Store item with name " + storeItem.Name + " is already added.");
    }
}
