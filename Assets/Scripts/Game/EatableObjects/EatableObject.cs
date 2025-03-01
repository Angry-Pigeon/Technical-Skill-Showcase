using System;
using Game.SceneDataLogic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
namespace Game.EatableObjects
{
    public class EatableObject : MonoBehaviour
    {
        public Collider[] Colliders;

        private Rigidbody rb;
        
        [Inject]
        private SceneData sceneData;
        
        public int MinimumLevelRequired = 1;
        
        public int Experience = 1; 
        public bool CanBeEaten = true;

        protected virtual void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            if(rb == null) return;
            rb.isKinematic = true;
        }

        public virtual void Eat()
        {
            if (!CanBeEaten) return;
            CanBeEaten = false;
        }
        
        public void SetIgnoreCollisionWithGround(bool value)
        {
            if (value)
            {
                rb.isKinematic = false;
            }
            foreach (var collider in Colliders)
            {
                Physics.IgnoreCollision(collider, sceneData.GroundCollider, value);
            }
        }
        
        [Button]
        public void GetAllColliders()
        {
            Colliders = GetComponentsInChildren<Collider>();
        }
    }
}