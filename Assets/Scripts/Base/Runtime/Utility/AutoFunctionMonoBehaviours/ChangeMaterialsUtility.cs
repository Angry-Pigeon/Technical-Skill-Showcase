using Game.GameLogic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
namespace Base.Runtime.Utility
{
    public static class ChangeMaterialsUtility 
    {
        
        [MenuItem("Tools/Switch Materials")]
        public static void FindMaterialsAndChange()
        {
            GameObject selectedObject = Selection.activeGameObject;
            
            Renderer[] renderers = selectedObject.GetComponentsInChildren<Renderer>();
            //if the renderer material is same as Glass or A1 or A2, change it to GlassTrans or A1Trans or A2Opaque
            foreach (var renderer in renderers)
            {
                if (renderer.sharedMaterial == LevelSettingsDatabase.Instance().Glass)
                {
                    renderer.sharedMaterial = LevelSettingsDatabase.Instance().GlassTrans;
                }
                else if (renderer.sharedMaterial == LevelSettingsDatabase.Instance().A1)
                {
                    renderer.sharedMaterial = LevelSettingsDatabase.Instance().A1Trans;
                }
                else if (renderer.sharedMaterial == LevelSettingsDatabase.Instance().A2)
                {
                    renderer.sharedMaterial = LevelSettingsDatabase.Instance().A2Trans;
                }
            }
        }
    }
}