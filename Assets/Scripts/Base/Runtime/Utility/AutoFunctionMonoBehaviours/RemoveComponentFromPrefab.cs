using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEditor;
using UnityEngine;
namespace Base.Runtime.Utility
{
    public class RemoveComponentFromPrefab
    {
        [MenuItem("Tools/Remove Colliders from Prefab")]
        public static void RemoveComponent()
        {
            Object[] objects = Selection.objects;

            foreach (Object o in objects)
            {
                RemoveComponent(o);
            }
        }
        
        public static void RemoveComponent(Object obj)
        {
            Object selectedObject = obj;
            if (selectedObject == null)
            {
                Debug.LogWarning("No object selected. Please select a prefab in the Project window.");
                return;
            }
        
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
            
            GameObject[] AllObjectsInPrefab = GetAllObjectsInPrefab(prefabRoot);
            
            foreach (var o in AllObjectsInPrefab)
            {
                RemoveCollidersFromOneObject(o);
            }
            
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }
        
        public static GameObject[] GetAllObjectsInPrefab(GameObject prefabRoot)
        {
            List<GameObject> allObjects = new List<GameObject>();

            allObjects.Add(prefabRoot);

            AddChildren(prefabRoot.transform, allObjects);

            return allObjects.ToArray();
        }

        private static void AddChildren(Transform parent, List<GameObject> list)
        {
            foreach (Transform child in parent)
            {
                list.Add(child.gameObject);
                AddChildren(child, list);
            }
        }
        
        private static void RemoveCollidersFromOneObject(GameObject prefabRoot)
        {

            // Get all Collider components in the prefab (including children).
            Collider[] colliders = prefabRoot.GetComponentsInChildren<Collider>();
            if (colliders.Length > 0)
            {
                // Remove each collider.
                for (int i = colliders.Length - 1; i >= 0; i--)
                {
                    Object.DestroyImmediate(colliders[i], true);
                }
                Debug.Log($"Colliders were removed from the {prefabRoot.name}.");
            }
            else
            {
                Debug.Log($"No colliders were found on the prefab{prefabRoot.name}.");
            }
        }
    }

}