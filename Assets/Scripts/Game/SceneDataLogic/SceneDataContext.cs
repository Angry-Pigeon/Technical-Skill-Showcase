using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Game.SceneDataLogic
{
    public class SceneDataContext : MonoBehaviour
    {
        public static SceneDataContext instance { get; private set; }
        
        public Collider GroundCollider;

        [field: SerializeField]
        public Collider WorldBoundsCollider { get; private set; }

        [field: SerializeField]
        public Bounds WorldBounds { get; private set; }

        private void Awake()
        {
            instance = this;
            
            if(instance == null)
                Debug.LogError("SceneDataContext instance is null");
        }

        private void OnDestroy()
        {
            instance = null;
        }

        [Button]
        public void SetWorldBounds()
        {
            WorldBounds = WorldBoundsCollider.bounds;
        }
    }
}