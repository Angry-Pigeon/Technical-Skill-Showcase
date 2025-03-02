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
            Object[] selectedObject = Selection.objects;
            
            foreach (var obj in selectedObject)
            {
                if (obj is GameObject selectedGameObject)
                {
                    SwitchMaterialsOnObjectRenderers(selectedGameObject);
                }
            }
            

        }

        private static void SwitchMaterialsOnObjectRenderers(GameObject selectedObject)
        {
                        string prefabPath = AssetDatabase.GetAssetPath(selectedObject);
            if (string.IsNullOrEmpty(prefabPath))
            {
                Debug.LogWarning("Selected object does not have a valid asset path.");
                return;
            }
        
            if (PrefabUtility.GetPrefabAssetType(selectedObject) == PrefabAssetType.NotAPrefab)
            {
                Debug.LogWarning("Selected object is not a prefab.");
                return;
            }
        
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
            
            Renderer[] renderers = selectedObject.GetComponentsInChildren<Renderer>();
            //if the renderer material is same as Glass or A1 or A2, change it to GlassTrans or A1Trans or A2Opaque
            foreach (var renderer in renderers)
            {
                // if (renderer.sharedMaterial.name == LevelSettingsDatabase.Instance().Glass.name) //if the material is Glass
                // {
                //     renderer.sharedMaterial = LevelSettingsDatabase.Instance().GlassTrans;
                // }
                // else if (renderer.sharedMaterial.name == LevelSettingsDatabase.Instance().A1.name) //if the material is A1
                // {
                //     renderer.sharedMaterial = LevelSettingsDatabase.Instance().A1Trans;
                // }
                // else if (renderer.sharedMaterial.name == LevelSettingsDatabase.Instance().A2.name) //if the material is A2
                // {
                //     renderer.sharedMaterial = LevelSettingsDatabase.Instance().A2Trans;
                // }
                // if (renderer.sharedMaterial.name == LevelSettingsDatabase.Instance().A4.name) //if the material is A3
                // {
                //     renderer.sharedMaterial = LevelSettingsDatabase.Instance().A4Trans;
                // }
            }
            
            Debug.Log($"Materials switched on {selectedObject}.");
            
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }
    }
}