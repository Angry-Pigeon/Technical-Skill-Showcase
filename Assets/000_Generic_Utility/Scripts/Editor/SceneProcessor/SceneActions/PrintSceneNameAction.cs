// Example scene action

using System;
using UnityEngine;
// [CreateAssetMenu(menuName = "Scene Processor/Actions/Print Scene Name")]
[Serializable]
public class PrintSceneNameAction : BaseSceneAction
{
    public override void Execute(string scenePath, System.Action onComplete)
    {
        Debug.Log($"Processing scene: {scenePath}");
        onComplete?.Invoke();
    }
    
}