using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        GUILayout.Button("Button");
        EditorGUILayout.EndToggleGroup();
    }
}
