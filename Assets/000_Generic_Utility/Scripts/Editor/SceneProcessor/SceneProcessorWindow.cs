using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using Unity.EditorCoroutines.Editor;

public class SceneProcessorWindow : OdinEditorWindow
{
    [FolderPath(AbsolutePath = false, ParentFolder = "Assets")] // Odin Inspector folder selector
    [OnValueChanged("@RefreshScenes()")] // Call the RefreshScenes method when the value changes
    public string selectedFolder = "Assets";

    [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowPaging = false)]
    [ReadOnly]
    public List<string> scenes = new List<string>();

    // [InlineEditor(Expanded = true)]
    [ShowInInspector, HideLabel]
    public BaseSceneAction selectedAction;

    [ShowInInspector, HideLabel]
    [MinMaxSlider(0, "@maxLevelRange", ShowFields = true)] // Slider for level range with dynamic limits
    public Vector2Int levelRange = new Vector2Int(0, 10);
    
    private int maxLevelRange;

    [Button("Refresh Scenes", ButtonSizes.Medium)]
    private void RefreshScenes()
    {
        scenes.Clear();

        if (string.IsNullOrEmpty(selectedFolder))
        {
            Debug.LogError("No folder selected!");
            return;
        }

        // Ensure the path includes 'Assets' and resolve absolute path
        string absoluteFolderPath = Path.Combine(Application.dataPath, selectedFolder.Replace("Assets/", ""));

        if (!Directory.Exists(absoluteFolderPath))
        {
            Debug.LogError($"Directory does not exist: {absoluteFolderPath}");
            return;
        }

        try
        {
            string[] scenePaths = Directory.GetFiles(absoluteFolderPath, "*.unity", SearchOption.AllDirectories);
            scenes.AddRange(scenePaths.Select(path => path.Replace("\\", "/")));

            Debug.Log($"Found {scenes.Count} scenes in folder {selectedFolder}.");

            // Update the MinMaxSlider range dynamically
            maxLevelRange = scenes.Count - 1;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error accessing directory: {absoluteFolderPath}. Exception: {ex.Message}");
        }
    }



    [Button("Process All Scenes", ButtonSizes.Medium)]
    private void ProcessAllScenes()
    {
        if (selectedAction == null)
        {
            Debug.LogError("No action selected!");
            return;
        }

        var filteredScenes = GetScenesInLevelRange();

        if (filteredScenes.Count == 0)
        {
            Debug.LogError("No scenes match the selected level range!");
            return;
        }

        SceneProcessor processor = new SceneProcessor();
        processor.ProcessScenes(filteredScenes, selectedAction);
    }

    [Button("Process First Scene in Range", ButtonSizes.Medium)]
    private void ProcessFirstSceneInRange()
    {
        if (selectedAction == null)
        {
            Debug.LogError("No action selected!");
            return;
        }

        var filteredScenes = GetScenesInLevelRange();

        if (filteredScenes.Count == 0)
        {
            Debug.LogError("No scenes match the selected level range!");
            return;
        }

        SceneProcessor processor = new SceneProcessor();
        processor.ProcessScenes(new List<string> { filteredScenes.First() }, selectedAction);
    }

    private List<string> GetScenesInLevelRange()
    {
        return scenes.Where((scenePath, index) =>
            index >= levelRange.x && index <= levelRange.y).ToList();
    }

    [MenuItem("Tools/Scene Processor (Odin)")]
    public static void ShowWindow()
    {
        GetWindow<SceneProcessorWindow>("Scene Processor");
    }
}