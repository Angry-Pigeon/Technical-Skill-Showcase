using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public class SceneProcessor
{
    private BaseSceneAction action;

    public void ProcessScenes(List<string> scenes, BaseSceneAction action)
    {
        this.action = action;
        EditorCoroutineUtility.StartCoroutine(PerformActions(scenes), this);
    }

    private IEnumerator PerformActions(List<string> scenes)
    {
        foreach (var scenePath in scenes)
        {
            bool taskCompleted = false;
            
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            yield return new WaitForSeconds(0.1f);
            Debug.Log("Scene opened: " + scenePath);
            action.Execute(scenePath, () =>
            {
                taskCompleted = true;
                Debug.Log($"Finished processing scene: {scenePath}");
            });
            yield return new WaitUntil(() => taskCompleted);

            yield return new WaitForSeconds(0.1f);
            
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            
            yield return  new WaitForSeconds(0.15f);
            
            while (EditorApplication.isPlaying)
                yield return null;
        }

        Debug.Log("Finished processing all scenes.");
    }
}