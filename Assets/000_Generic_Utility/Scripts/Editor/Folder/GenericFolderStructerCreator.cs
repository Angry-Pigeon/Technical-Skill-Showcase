using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GenericFolderStructerCreator
{
    private static string[] GenericFolders = new string[]
    {
        "Scripts",
        "Materials",
        "Textures",
        "Prefabs",
        "Scenes",
        "Animations",
        "Audio",
        "Fonts",
        "Models",
        "Resources",
        "Shaders",
        "StreamingAssets",
        "Editor",
        "Plugins",
        "Testing"
    };

    [MenuItem("Assets/Create/Custom/Create Generic Folders", false, 20)]
    private static void CreateGenericFolders()
    {
        // Get the currently selected folder in the Project window
        if (GetCurrentPath(out string selectedPath)) return;

        // Create folders inside the selected folder
        CreateFolders(GenericFolders, selectedPath);

        // Refresh the AssetDatabase to reflect the changes
        AssetDatabase.Refresh();
    }
    
    public static bool GetCurrentPath(out string selectedPath)
    {

        selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);

        // If no folder is selected or selection is invalid, show a warning
        if (string.IsNullOrEmpty(selectedPath) || !Directory.Exists(selectedPath))
        {
            EditorUtility.DisplayDialog("Invalid Selection", "Please select a valid folder.", "OK");
            return true;
        }
        return false;
    }
    
    public static void CreateFolders(string[] foldersToCreate, string selectedPath)
    {

        foreach (string folder in foldersToCreate)
        {
            string folderPath = Path.Combine(selectedPath, folder);
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(selectedPath, folder);
            }
        }
    }
}