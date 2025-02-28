using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Game.SceneDataLogic
{
    public class SceneData : MonoBehaviour
    {
        public Collider GroundCollider;

        [field: SerializeField]
        public Joystick Joystick { get; private set; }

        [field: SerializeField]
        public Collider WorldBoundsCollider { get; private set; }

        [field: SerializeField]
        public Bounds WorldBounds { get; private set; }
        
        
        [Button]
        public void SetWorldBounds()
        {
            WorldBounds = WorldBoundsCollider.bounds;
        }
    }
}