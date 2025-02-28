using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Base.Runtime.Utility
{
    public class EditorObjectSpawner : MonoBehaviour
    {
        [Header("Grid Settings")]
        [Tooltip("Number of cells in the X (columns) and Y (rows) directions.")]
        public Vector2Int GridSize = new Vector2Int(5, 5);
        [Tooltip("Offset position (local X,Z) for the grid start.")]
        public Vector2 GridOffset = Vector2.zero;
        [Tooltip("Size of each grid cell (width, depth).")]
        public Vector2 GridCellSize = new Vector2(2f, 2f);
        [Tooltip("Padding to subtract from the drawn spawn cube's size. This value directly defines the gap between adjacent cubes.")]
        public Vector2 GridCellPadding = new Vector2(0.2f, 0.2f);

        [Header("Spawn Options")]
        [TableList]
        [Tooltip("List of prefabs and their spawn weight.")]
        public List<EditorObjectSpawnerData> ObjectSpawnerData;
        [Tooltip("Randomize the spawn position within each cell?")]
        public bool RandomizePosition;
        [Tooltip("Range for randomizing the spawn position (applied to X and Z).")]
        public float RandomizePositionRange;
        [Tooltip("Randomize the Y-axis rotation of spawned objects?")]
        public bool RandomizeRotation;
        [Tooltip("Rotation range (min/max Y-rotation in degrees).")]
        public Vector2 RandomizeRotationRange = new Vector2(0, 360);

        [Button("Generate Objects")]
        private void GenerateObjects()
        {
            // Clear previous objects.
            ClearObjects();

            // Calculate total weight for weighted random selection.
            int totalWeight = 0;
            foreach (var data in ObjectSpawnerData)
            {
                totalWeight += Mathf.Max(data.Weight, 0);
            }
            if (totalWeight <= 0)
            {
                Debug.LogWarning("Total weight is zero. Please assign positive weights to the prefabs.");
                return;
            }

            // Iterate through each cell in the grid.
            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    // Calculate the cell's center in local space.
                    Vector3 spawnLocalPosition = GetCellLocalCenter(x, y);

                    // Optionally randomize the local position.
                    if (RandomizePosition)
                    {
                        spawnLocalPosition.x += UnityEngine.Random.Range(-RandomizePositionRange, RandomizePositionRange);
                        spawnLocalPosition.z += UnityEngine.Random.Range(-RandomizePositionRange, RandomizePositionRange);
                    }

                    // Optionally randomize rotation (we use local rotation).
                    Quaternion spawnRotation = Quaternion.identity;
                    if (RandomizeRotation)
                    {
                        float rotY = UnityEngine.Random.Range(RandomizeRotationRange.x, RandomizeRotationRange.y);
                        spawnRotation = Quaternion.Euler(0, rotY, 0);
                    }

                    // Pick a prefab using weighted random selection.
                    GameObject prefab = GetWeightedRandomPrefab(totalWeight);
                    if (prefab == null)
                        continue;

                    GameObject spawned = null;
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        // Instantiate as child using PrefabUtility so that prefab connection is maintained.
                        spawned = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);
                    }
                    else
                    {
                        spawned = Instantiate(prefab, transform);
                    }
#else
                    spawned = Instantiate(prefab, transform);
#endif
                    // Set the local position and rotation.
                    spawned.transform.localPosition = spawnLocalPosition;
                    spawned.transform.localRotation = spawnRotation;
                }
            }
        }

        [Button("Clear Objects")]
        private void ClearObjects()
        {
            // Gather all children (assumed to be generated objects).
            List<GameObject> children = new List<GameObject>();
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                children.Add(transform.GetChild(i).gameObject);
            }
            // Remove them safely.
            foreach (var child in children)
            {
#if UNITY_EDITOR
                DestroyImmediate(child);
#else
                Destroy(child);
#endif
            }
        }

        private GameObject GetWeightedRandomPrefab(int totalWeight)
        {
            int randomWeight = UnityEngine.Random.Range(0, totalWeight);
            foreach (var data in ObjectSpawnerData)
            {
                int weight = Mathf.Max(data.Weight, 0);
                randomWeight -= weight;
                if (randomWeight < 0)
                {
                    return data.Prefab;
                }
            }
            return null;
        }

        /// <summary>
        /// Calculates the local center position for a given cell in the grid.
        /// </summary>
        private Vector3 GetCellLocalCenter(int x, int y)
        {
            float posX = GridOffset.x + x * (GridCellSize.x + GridCellPadding.x) + GridCellSize.x * 0.5f;
            float posZ = GridOffset.y + y * (GridCellSize.y + GridCellPadding.y) + GridCellSize.y * 0.5f;
            return new Vector3(posX, 0, posZ);
        }


        /// <summary>
        /// Returns the size of the drawn spawn cube within each cell.
        /// The cube size is the full cell size reduced by the padding, which directly defines the gap between adjacent cubes.
        /// </summary>
        private Vector3 GetSpawnCubeSize()
        {
            return new Vector3(
                GridCellSize.x,
                0.1f,
                GridCellSize.y
            );
        }

        private void OnDrawGizmosSelected()
        {
            // Visualize the grid in the Scene view by converting local positions to world positions.
            Gizmos.color = Color.green;
            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    Vector3 cellCenterLocal = GetCellLocalCenter(x, y);
                    Vector3 cellCenterWorld = transform.TransformPoint(cellCenterLocal);
                    Vector3 spawnCubeSize = GetSpawnCubeSize();
                    Gizmos.DrawWireCube(cellCenterWorld, spawnCubeSize);
                }
            }
        }
    }

    [Serializable]
    public class EditorObjectSpawnerData
    {
        [Tooltip("The prefab to spawn.")]
        public GameObject Prefab;
        [Tooltip("Relative weight for this prefab. Higher values increase the chance of being selected.")]
        public int Weight = 1;
    }
}
